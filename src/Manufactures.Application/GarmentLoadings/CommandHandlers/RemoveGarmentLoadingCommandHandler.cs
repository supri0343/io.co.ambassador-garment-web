using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentLoadings.CommandHandlers
{
    public class RemoveGarmentLoadingCommandHandler : ICommandHandler<RemoveGarmentLoadingCommand, GarmentLoading>
    {
        private readonly IStorage _storage;
        private readonly IGarmentLoadingRepository _garmentLoadingRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;

        public RemoveGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentLoading> Handle(RemoveGarmentLoadingCommand request, CancellationToken cancellationToken)
        {
            var loading = _garmentLoadingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentLoading(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == loading.UnitId && new GarmentComodityId(a.ComodityId) == loading.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<GarmentFinishedGoodStock, double> finGood = new Dictionary<GarmentFinishedGoodStock, double>();

            _garmentLoadingItemRepository.Find(o => o.LoadingId == loading.Identity).ForEach(async loadingItem =>
            {
                //push data to sewing do item to be updated
                if (sewingDOItemToBeUpdated.ContainsKey(loadingItem.SewingDOItemId))
                {
                    sewingDOItemToBeUpdated[loadingItem.SewingDOItemId] += loadingItem.Quantity;
                }
                else
                {
                    sewingDOItemToBeUpdated.Add(loadingItem.SewingDOItemId, loadingItem.Quantity);
                }

                //push data finished good to be updated
                if (loading.LoadingOutType == "BARANG JADI")
                {
                    //check garment finished good stock exist
                    var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                    a => a.RONo == loading.RONo &&
                        a.Article == loading.Article &&
                        a.BasicPrice == loadingItem.BasicPrice &&
                        new UnitDepartmentId(a.UnitId) == loading.UnitId &&
                        new SizeId(a.SizeId) == loadingItem.SizeId &&
                        new GarmentComodityId(a.ComodityId) == loading.ComodityId &&
                        new UomId(a.UomId) == loadingItem.UomId
                        && a.FinishedFrom == "LOADING"
                    ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                    //push data garment finished good stock
                    if (finGood.ContainsKey(garmentFinishedGoodExist))
                    {
                        finGood[garmentFinishedGoodExist] += loadingItem.Quantity;
                    }
                    else
                    {
                        finGood.Add(garmentFinishedGoodExist, loadingItem.Quantity);
                    }

                    //delete garment finished good stock history
                    GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.LoadingItemId == loadingItem.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                    garmentFinishedGoodStockHistory.Remove();
                    await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                }

                loadingItem.Remove();

                await _garmentLoadingItemRepository.Update(loadingItem);
            });

            //update sewing do item 
            foreach (var sewingDOItem in sewingDOItemToBeUpdated)
            {
                var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
                garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity + sewingDOItem.Value);
                garmentSewingDOItem.Modify();

                await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
            }

            //delete garment sewing in
            if (loading.LoadingOutType == "SEWING")
            {
                var garmentSewingIn = _garmentSewingInRepository.Query.Where(o => o.LoadingId == request.Identity).Select(o => new GarmentSewingIn(o)).Single();
                var garmentSewingInItems = _garmentSewingInItemRepository.Find(x => x.SewingInId == garmentSewingIn.Identity);

                foreach (var item in garmentSewingInItems)
                {
                    item.Remove();
                    await _garmentSewingInItemRepository.Update(item);
                }
                garmentSewingIn.Remove();
                await _garmentSewingInRepository.Update(garmentSewingIn);
            }  

            //update finished good stock
            else if (loading.LoadingOutType == "BARANG JADI")
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

            loading.Remove();
           
            await _garmentLoadingRepository.Update(loading);
           

            _storage.Save();

            return loading;
        }
    }
}
