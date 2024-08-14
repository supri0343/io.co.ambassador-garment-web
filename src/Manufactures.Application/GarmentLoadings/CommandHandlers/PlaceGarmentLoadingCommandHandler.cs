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
    public class PlaceGarmentLoadingCommandHandler : ICommandHandler<PlaceGarmentLoadingCommand, GarmentLoading>
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

        public PlaceGarmentLoadingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            _garmentSewingDOItemRepository= storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();

            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentLoading> Handle(PlaceGarmentLoadingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).OrderBy(o => o.ModifiedDate).Select(s => new GarmentComodityPrice(s)).Last();

            GarmentLoading garmentLoading = new GarmentLoading(
                Guid.NewGuid(),
                GenerateLoadingNo(request),
                request.SewingDOId,
                request.SewingDONo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.LoadingDate,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.LoadingOutType
            );

            //jika loading out type = SEWING maka akan membuat data sewing in
            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();
            if (request.LoadingOutType == "SEWING")
            {
                GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                    Guid.NewGuid(),
                    GenerateSewingInNo(request),
                    "CUTTING",
                    garmentLoading.Identity,
                    garmentLoading.LoadingNo,
                    new UnitDepartmentId(request.Unit.Id),
                    request.Unit.Code,
                    request.Unit.Name,
                    new UnitDepartmentId(request.Unit.Id),
                    request.Unit.Code,
                    request.Unit.Name,
                    request.RONo,
                    request.Article,
                    new GarmentComodityId(request.Comodity.Id),
                    request.Comodity.Code,
                    request.Comodity.Name,
                    request.LoadingDate
                );

                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        GarmentLoadingItem garmentLoadingItem = new GarmentLoadingItem(
                            Guid.NewGuid(),
                            garmentLoading.Identity,
                            item.SewingDOItemId,
                            new SizeId(item.Size.Id),
                            item.Size.Size,
                            new ProductId(item.Product.Id),
                            item.Product.Code,
                            item.Product.Name,
                            item.DesignColor,
                            item.Quantity,
                            0,
                            item.BasicPrice,
                            new UomId(item.Uom.Id),
                            item.Uom.Unit,
                            item.Color,
                            item.Price
                        );

                        GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                            Guid.NewGuid(),
                            garmentSewingIn.Identity,
                            Guid.Empty,
                            Guid.Empty,
                            garmentLoadingItem.Identity,
                            Guid.Empty,
                            Guid.Empty,
                            new ProductId(item.Product.Id),
                            item.Product.Code,
                            item.Product.Name,
                            item.DesignColor,
                            new SizeId(item.Size.Id),
                            item.Size.Size,
                            item.Quantity,
                            new UomId(item.Uom.Id),
                            item.Uom.Unit,
                            item.Color,
                            item.Quantity,
                            item.BasicPrice,
                            item.Price
                        );

                        if (sewingDOItemToBeUpdated.ContainsKey(item.SewingDOItemId))
                        {
                            sewingDOItemToBeUpdated[item.SewingDOItemId] += item.Quantity;
                        }
                        else
                        {
                            sewingDOItemToBeUpdated.Add(item.SewingDOItemId, item.Quantity);
                        }

                        await _garmentLoadingItemRepository.Update(garmentLoadingItem);
                        await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                    }
                }

                await _garmentSewingInRepository.Update(garmentSewingIn);
            }

            //jika loading out type = BARANG JADI maka akan membuat atau update Finished Good Stock
            Dictionary<string, double> finGood = new Dictionary<string, double>();
            if (request.LoadingOutType == "BARANG JADI") 
            {
                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        GarmentLoadingItem garmentLoadingItem = new GarmentLoadingItem(
                            Guid.NewGuid(),
                            garmentLoading.Identity,
                            item.SewingDOItemId,
                            new SizeId(item.Size.Id),
                            item.Size.Size,
                            new ProductId(item.Product.Id),
                            item.Product.Code,
                            item.Product.Name,
                            item.DesignColor,
                            item.Quantity,
                            0,
                            item.BasicPrice,
                            new UomId(item.Uom.Id),
                            item.Uom.Unit,
                            item.Color,
                            item.Price
                        );

                        item.Id = garmentLoadingItem.Identity;
                        //push data sewingDOItem to be updated
                        if (sewingDOItemToBeUpdated.ContainsKey(item.SewingDOItemId))
                        {
                            sewingDOItemToBeUpdated[item.SewingDOItemId] += item.Quantity;
                        }
                        else
                        {
                            sewingDOItemToBeUpdated.Add(item.SewingDOItemId, item.Quantity);
                        }

                        //Push data finished good stock to be updated
                        string finStock = item.Size.Id + "~" + item.Size.Size + "~" + item.Uom.Id + "~" + item.Uom.Unit + "~" + item.BasicPrice;

                        if (finGood.ContainsKey(finStock))
                        {
                            finGood[finStock] += item.Quantity;
                        }
                        else
                        {
                            finGood.Add(finStock, item.Quantity);
                        }

                        await _garmentLoadingItemRepository.Update(garmentLoadingItem);
                    }
                }
            }

            //update data sewingDOItem
            foreach (var sewingDOItem in sewingDOItemToBeUpdated)
            {
                var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
                garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity - sewingDOItem.Value);
                garmentSewingDOItem.Modify();

                await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
            }

            //Update data finished good stock
            if (request.LoadingOutType == "BARANG JADI")
            {
                int count = 1;
                List<GarmentFinishedGoodStock> finGoodStocks = new List<GarmentFinishedGoodStock>();
                foreach (var finGoodStock in finGood)
                {
                    SizeId sizeId = new SizeId(Convert.ToInt32(finGoodStock.Key.Split("~")[0]));
                    string sizeName = finGoodStock.Key.Split("~")[1];
                    UomId uomId = new UomId(Convert.ToInt32(finGoodStock.Key.Split("~")[2]));
                    string uomUnit = finGoodStock.Key.Split("~")[3];
                    double basicPrice = Convert.ToDouble(finGoodStock.Key.Split("~")[4]);
                    var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                        a => a.RONo == request.RONo &&
                            a.Article == request.Article &&
                            a.BasicPrice == basicPrice &&
                            a.UnitId == request.Unit.Id &&
                            new SizeId(a.SizeId) == sizeId &&
                            a.ComodityId == request.Comodity.Id &&
                            new UomId(a.UomId) == uomId
                            && a.FinishedFrom == "LOADING"
                        ).Select(s => new GarmentFinishedGoodStock(s)).SingleOrDefault();

                    double qty = garmentFinishedGoodExist == null ? finGoodStock.Value : (finGoodStock.Value + garmentFinishedGoodExist.Quantity);

                    double price = (basicPrice + (double)garmentComodityPrice.Price) * qty;

                    if (garmentFinishedGoodExist == null)
                    {
                        var now = DateTime.Now;
                        var year = now.ToString("yy");
                        var month = now.ToString("MM");
                        var prefix = $"ST{request.Unit.Code.Trim()}{year}{month}";

                        var lastFnGoodNo = _garmentFinishedGoodStockRepository.Query.Where(w => w.FinishedGoodStockNo.StartsWith(prefix))
                        .OrderByDescending(o => o.FinishedGoodStockNo)
                        .Select(s => int.Parse(s.FinishedGoodStockNo.Replace(prefix, "")))
                        .FirstOrDefault();
                        var FinGoodNo = $"{prefix}{(lastFnGoodNo + count).ToString("D4")}";
                        GarmentFinishedGoodStock finishedGood = new GarmentFinishedGoodStock(
                                        Guid.NewGuid(),
                                        FinGoodNo,
                                        request.RONo,
                                        request.Article,
                                        new UnitDepartmentId(request.Unit.Id),
                                        request.Unit.Code,
                                        request.Unit.Name,
                                        new GarmentComodityId(request.Comodity.Id),
                                        request.Comodity.Code,
                                        request.Comodity.Name,
                                        sizeId,
                                        sizeName,
                                        uomId,
                                        uomUnit,
                                        qty,
                                        basicPrice,
                                        price,
                                        "LOADING"
                                        );
                        count++;
                        await _garmentFinishedGoodStockRepository.Update(finishedGood);
                        finGoodStocks.Add(finishedGood);
                    }
                    else
                    {
                        garmentFinishedGoodExist.SetQuantity(qty);
                        garmentFinishedGoodExist.SetPrice(price);
                        garmentFinishedGoodExist.Modify();

                        await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
                        finGoodStocks.Add(garmentFinishedGoodExist);
                    }

                }

                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                            var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                             a.Article == request.Article &&
                             a.BasicPrice == item.BasicPrice &&
                             a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                             a.SizeId == new SizeId(item.Size.Id) &&
                             a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                            a.UomId == new UomId(item.Uom.Id)).Single();

                            double price = (stock.BasicPrice + (double)garmentComodityPrice.Price) * item.Quantity;

                            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
                                    Guid.NewGuid(),
                                    stock.Identity,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    "IN",
                                    stock.RONo,
                                    stock.Article,
                                    stock.UnitId,
                                    stock.UnitCode,
                                    stock.UnitName,
                                    stock.ComodityId,
                                    stock.ComodityCode,
                                    stock.ComodityName,
                                    stock.SizeId,
                                    stock.SizeName,
                                    stock.UomId,
                                    stock.UomUnit,
                                    item.Quantity,
                                    stock.BasicPrice,
                                    price,
                                    Guid.Empty,
                                    Guid.Empty,
                                    garmentLoading.Identity,
                                    item.Id,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty
                                );
                            await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                        
                    }
                }
            }

            await _garmentLoadingRepository.Update(garmentLoading);
          
            _storage.Save();

            return garmentLoading;
        }

        private string GenerateLoadingNo(PlaceGarmentLoadingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"LD{unitcode}{year}{month}";

            var lastLoadingNo = _garmentLoadingRepository.Query.Where(w => w.LoadingNo.StartsWith(prefix))
                .OrderByDescending(o => o.LoadingNo)
                .Select(s => int.Parse(s.LoadingNo.Replace(prefix, "")))
                .FirstOrDefault();
            var loadingNo = $"{prefix}{(lastLoadingNo + 1).ToString("D4")}";

            return loadingNo;
        }

        private string GenerateSewingInNo(PlaceGarmentLoadingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"SI{request.Unit.Code}{year}{month}";

            var lastSewingInNo = _garmentSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }
    }
}
