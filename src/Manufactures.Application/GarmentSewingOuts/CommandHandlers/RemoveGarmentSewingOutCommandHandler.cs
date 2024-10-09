using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.CommandHandlers
{
    public class RemoveGarmentSewingOutCommandHandler : ICommandHandler<RemoveGarmentSewingOutCommand, GarmentSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;

        public RemoveGarmentSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentSewingOut> Handle(RemoveGarmentSewingOutCommand request, CancellationToken cancellationToken)
        {
            var sewOut = _garmentSewingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSewingOut(o)).Single();

            if (sewOut.SewingTo == "CUTTING")
            {
                Guid cutInId = _garmentCuttingInItemRepository.Query.Where(a => a.SewingOutId == sewOut.Identity).First().CutInId;

                var cutIn = _garmentCuttingInRepository.Query.Where(a => a.Identity == cutInId).Select(a => new GarmentCuttingIn(a)).Single();

                _garmentCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
                {
                    _garmentCuttingInDetailRepository.Find(o => o.CutInItemId == cutInItem.Identity).ForEach(async cutInDetail =>
                    {

                        cutInDetail.Remove();
                        await _garmentCuttingInDetailRepository.Update(cutInDetail);
                    });

                    cutInItem.Remove();
                    await _garmentCuttingInItemRepository.Update(cutInItem);
                });

                cutIn.Remove();
                await _garmentCuttingInRepository.Update(cutIn);
            }

            if (sewOut.SewingTo == "SEWING")
            {
                Guid sewInId = Guid.Empty;
                _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
                {
                    var sewInItem = _garmentSewingInItemRepository.Query.Where(o => o.SewingOutItemId == sewOutItem.Identity).Select(o => new GarmentSewingInItem(o)).Single();
                    sewInId = sewInItem.SewingInId;
                    if (sewOut.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            sewInItem= _garmentSewingInItemRepository.Query.Where(o => o.SewingOutDetailId == sewOutDetail.Identity).Select(o => new GarmentSewingInItem(o)).Single();
                            
                            sewInItem.Remove();
                            await _garmentSewingInItemRepository.Update(sewInItem);
                        });
                    }
                    else
                    {
                        sewInItem.Remove();
                        await _garmentSewingInItemRepository.Update(sewInItem);
                    }
                });
                var sewIn= _garmentSewingInRepository.Query.Where(a=>a.Identity==sewInId).Select(o => new GarmentSewingIn(o)).Single();
                sewIn.Remove();
                await _garmentSewingInRepository.Update(sewIn);
            }

            if (sewOut.SewingTo == "FINISHING")
            {
                Guid finInId = Guid.Empty;
                _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
                {
                    var finInItem = _garmentFinishingInItemRepository.Query.Where(o => o.SewingOutItemId == sewOutItem.Identity).Select(o => new GarmentFinishingInItem(o)).Single();
                    finInId = finInItem.FinishingInId;
                    if (sewOut.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            finInItem = _garmentFinishingInItemRepository.Query.Where(o => o.SewingOutDetailId == sewOutDetail.Identity).Select(o => new GarmentFinishingInItem(o)).Single();

                            finInItem.Remove();
                            await _garmentFinishingInItemRepository.Update(finInItem);
                        });
                    }
                    else
                    {
                        finInItem.Remove();
                        await _garmentFinishingInItemRepository.Update(finInItem);
                    }
                });
                var finIn = _garmentFinishingInRepository.Query.Where(a => a.Identity == finInId).Select(o => new GarmentFinishingIn(o)).Single();
                finIn.Remove();
                await _garmentFinishingInRepository.Update(finIn);
            }

            if (sewOut.SewingTo == "BARANG JADI")
            {
                Dictionary<GarmentFinishedGoodStock, double> finGood = new Dictionary<GarmentFinishedGoodStock, double>();
                _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
                {
                    if (sewOut.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            //check garment finished good stock exist
                            var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                                a => a.RONo == sewOut.RONo &&
                                a.Article == sewOut.Article &&
                                a.BasicPrice == sewOutItem.BasicPrice &&
                                new UnitDepartmentId(a.UnitId) == sewOut.UnitId &&
                                new SizeId(a.SizeId) == sewOutDetail.SizeId &&
                                new GarmentComodityId(a.ComodityId) == sewOut.ComodityId &&
                                new UomId(a.UomId) == sewOutDetail.UomId
                                && a.FinishedFrom == "SEWING"
                            ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                            //push data garment finished good stock
                            if (finGood.ContainsKey(garmentFinishedGoodExist))
                            {
                                finGood[garmentFinishedGoodExist] += sewOutDetail.Quantity;
                            }
                            else
                            {
                                finGood.Add(garmentFinishedGoodExist, sewOutDetail.Quantity);
                            }

                            //delete garment finished good stock history
                            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.SewingOutDetailId == sewOutDetail.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                            garmentFinishedGoodStockHistory.Remove();
                            await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                        });
                    }
                    else
                    {
                        //check garment finished good stock exist
                        var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                            a => a.RONo == sewOut.RONo &&
                            a.Article == sewOut.Article &&
                            a.BasicPrice == sewOutItem.BasicPrice &&
                            new UnitDepartmentId(a.UnitId) == sewOut.UnitId &&
                            new SizeId(a.SizeId) == sewOutItem.SizeId &&
                            new GarmentComodityId(a.ComodityId) == sewOut.ComodityId &&
                            new UomId(a.UomId) == sewOutItem.UomId
                            && a.FinishedFrom == "SEWING"
                        ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                        //push data garment finished good stock
                        if (finGood.ContainsKey(garmentFinishedGoodExist))
                        {
                            finGood[garmentFinishedGoodExist] += sewOutItem.Quantity;
                        }
                        else
                        {
                            finGood.Add(garmentFinishedGoodExist, sewOutItem.Quantity);
                        }

                        //delete garment finished good stock history
                        GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                        garmentFinishedGoodStockHistory.Remove();
                        await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                    }
                });

                //update finished good stock
                foreach (var finGoodStock in finGood)
                {
                    var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                        a => a.Identity == finGoodStock.Key.Identity
                        ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                    var qty = garmentFinishedGoodExist.Quantity - finGoodStock.Value;
                    GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == sewOut.UnitId && new GarmentComodityId(a.ComodityId) == sewOut.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

                    garmentFinishedGoodExist.SetQuantity(qty);
                    garmentFinishedGoodExist.SetPrice((garmentFinishedGoodExist.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                    garmentFinishedGoodExist.Modify();

                    await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
                }

            }

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
            {
                if (sewOut.IsDifferentSize)
                {
                    _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                    {
                        if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SewingInItemId))
                        {
                            sewInItemToBeUpdated[sewOutItem.SewingInItemId] += sewOutDetail.Quantity;
                        }
                        else
                        {
                            sewInItemToBeUpdated.Add(sewOutItem.SewingInItemId, sewOutDetail.Quantity);
                        }

                        sewOutDetail.Remove();
                        await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                    });
                }
                else
                {
                    if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SewingInItemId))
                    {
                        sewInItemToBeUpdated[sewOutItem.SewingInItemId] += sewOutItem.Quantity;
                    }
                    else
                    {
                        sewInItemToBeUpdated.Add(sewOutItem.SewingInItemId, sewOutItem.Quantity);
                    }
                }


                sewOutItem.Remove();
                await _garmentSewingOutItemRepository.Update(sewOutItem);
            });

            foreach (var sewingInItem in sewInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + sewingInItem.Value);
                garmentSewInItem.Modify();
                await _garmentSewingInItemRepository.Update(garmentSewInItem);
            }

            sewOut.Remove();
            await _garmentSewingOutRepository.Update(sewOut);

            _storage.Save();

            return sewOut;
        }
    }
}
