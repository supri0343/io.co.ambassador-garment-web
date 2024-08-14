using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Moonlay;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;

namespace Manufactures.Application.GarmentCuttingOuts.CommandHandlers
{
    public class RemoveGarmentCuttingOutCommandHandler : ICommandHandler<RemoveGarmentCuttingOutCommand, GarmentCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;

        public RemoveGarmentCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentCuttingOut> Handle(RemoveGarmentCuttingOutCommand request, CancellationToken cancellationToken)
        {
            var cutOut = _garmentCuttingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentCuttingOut(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == cutOut.UnitId && new GarmentComodityId(a.ComodityId) == cutOut.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();


            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<GarmentFinishedGoodStock, double> finGood = new Dictionary<GarmentFinishedGoodStock, double>();

            _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).ForEach(async cutOutItem =>
            {
                _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).ForEach(async cutOutDetail =>
                {
                    //push data cutting in detail to be updated
                    if (cuttingInDetailToBeUpdated.ContainsKey(cutOutItem.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[cutOutItem.CuttingInDetailId] += cutOutDetail.CuttingOutQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(cutOutItem.CuttingInDetailId, cutOutDetail.CuttingOutQuantity);
                    }

                    //push data finished good to be updated
                    if (cutOut.CuttingOutType == "BARANG JADI")
                    {
                        //check garment finished good stock exist
                        var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                        a => a.RONo == cutOut.RONo &&
                            a.Article == cutOut.Article &&
                            a.BasicPrice == cutOutDetail.BasicPrice &&
                            new UnitDepartmentId(a.UnitId) == cutOut.UnitId &&
                            new SizeId(a.SizeId) == cutOutDetail.SizeId &&
                            new GarmentComodityId(a.ComodityId) == cutOut.ComodityId &&
                            new UomId(a.UomId) == cutOutDetail.CuttingOutUomId
                            && a.FinishedFrom == "CUTTING"
                        ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                        //push data garment finished good stock
                        if (finGood.ContainsKey(garmentFinishedGoodExist))
                        {
                            finGood[garmentFinishedGoodExist] += cutOutDetail.CuttingOutQuantity;
                        }
                        else
                        {
                            finGood.Add(garmentFinishedGoodExist, cutOutDetail.CuttingOutQuantity);
                        }

                        //delete garment finished good stock history
                        GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.CuttingOutDetailId == cutOutDetail.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                        garmentFinishedGoodStockHistory.Remove();
                        await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

                    }

                    cutOutDetail.Remove();
                    await _garmentCuttingOutDetailRepository.Update(cutOutDetail);
                });

                cutOutItem.Remove();
                await _garmentCuttingOutItemRepository.Update(cutOutItem);
            });

            //update cutting in detail
            foreach (var cuttingInItem in cuttingInDetailToBeUpdated)
            {
                var garmentCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInItem.Key).Select(s => new GarmentCuttingInDetail(s)).Single();
                garmentCuttingInDetail.SetRemainingQuantity(garmentCuttingInDetail.RemainingQuantity + cuttingInItem.Value);
                garmentCuttingInDetail.Modify();
                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
            }

            //delete garment sewing do
            if (cutOut.CuttingOutType == "SEWING")
            {
                var sewingDO = _garmentSewingDORepository.Query.Where(o => o.CuttingOutId == request.Identity).Select(o => new GarmentSewingDO(o)).Single();
                _garmentSewingDOItemRepository.Find(o => o.SewingDOId == sewingDO.Identity).ForEach(async sewingDOItem =>
                {
                    sewingDOItem.Remove();
                    await _garmentSewingDOItemRepository.Update(sewingDOItem);
                });

                sewingDO.Remove();
                await _garmentSewingDORepository.Update(sewingDO);
            }

            //update finished good stock
            else if (cutOut.CuttingOutType == "BARANG JADI")
            {
                foreach (var finGoodStock in finGood)
                {
                    var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                        a => a.Identity == finGoodStock.Key.Identity
                        ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                    var qty = garmentFinishedGoodExist.Quantity - finGoodStock.Value;

                    garmentFinishedGoodExist.SetQuantity(qty);
                    garmentFinishedGoodExist.SetPrice((garmentFinishedGoodExist.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                    garmentFinishedGoodExist.Modify();

                    await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
                }
            }

            cutOut.Remove();
            await _garmentCuttingOutRepository.Update(cutOut);

            _storage.Save();

            return cutOut;
        }
    }
}