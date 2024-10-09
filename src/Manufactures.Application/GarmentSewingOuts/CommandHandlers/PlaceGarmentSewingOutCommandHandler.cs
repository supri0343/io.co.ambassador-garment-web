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
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.CommandHandlers
{
    public class PlaceGarmentSewingOutCommandHandler : ICommandHandler<PlaceGarmentSewingOutCommand, GarmentSewingOut>
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
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;

        public PlaceGarmentSewingOutCommandHandler(IStorage storage)
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
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();

            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentSewingOut> Handle(PlaceGarmentSewingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            Guid sewingOutId = Guid.NewGuid();
            string sewingOutNo = GenerateSewOutNo(request);

            
            GarmentSewingOut garmentSewingOut = new GarmentSewingOut(
                sewingOutId,
                sewingOutNo,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new UnitDepartmentId(request.UnitTo.Id),
                request.UnitTo.Code,
                request.UnitTo.Name,
                request.SewingTo,
                request.SewingOutDate.GetValueOrDefault(),
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name, 
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.IsDifferentSize
            );

            Dictionary<Guid, double> sewingInItemToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentSewingOutItem garmentSewingOutItem = new GarmentSewingOutItem(
                        Guid.NewGuid(),
                        garmentSewingOut.Identity,
                        item.SewingInId,
                        item.SewingInItemId,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        request.IsDifferentSize? item.TotalQuantity : item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        request.SewingTo == "CUTTING" ? item.RemainingQuantity : 0,
                        item.BasicPrice,
                        item.Price
                    );
                    item.Id = garmentSewingOutItem.Identity;

                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(
                                Guid.NewGuid(),
                                garmentSewingOutItem.Identity,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                detail.Quantity,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit
                            );
                            detail.Id = garmentSewingOutDetail.Identity;

                            if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                            {
                                sewingInItemToBeUpdated[item.SewingInItemId] += detail.Quantity;
                            }
                            else
                            {
                                sewingInItemToBeUpdated.Add(item.SewingInItemId, detail.Quantity);
                            }

                            await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                        }
                    }
                    else
                    {
                        if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                        {
                            sewingInItemToBeUpdated[item.SewingInItemId] += item.Quantity;
                        }
                        else
                        {
                            sewingInItemToBeUpdated.Add(item.SewingInItemId, item.Quantity);
                        }
                    }
                    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                }
                
            }

            foreach (var sewInItem in sewingInItemToBeUpdated)
            {
                var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
                garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity - sewInItem.Value);
                garmentSewingInItem.Modify();

                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
            }


            await _garmentSewingOutRepository.Update(garmentSewingOut);

            #region CreateCuttingIn

            if (request.SewingTo == "CUTTING")
            {
                GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

                var now = DateTime.Now;
                var year = now.ToString("yy");
                var month = now.ToString("MM");

                var prefix = $"DC{request.UnitTo.Code.Trim()}{year}{month}";

                var lastCutInNo = _garmentCuttingInRepository.Query.Where(w => w.CutInNo.StartsWith(prefix))
                    .OrderByDescending(o => o.CutInNo)
                    .Select(s => int.Parse(s.CutInNo.Replace(prefix, "")))
                    .FirstOrDefault();
                var CutInNo = $"{prefix}{(lastCutInNo + 1).ToString("D4")}";

                GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(
                    Guid.NewGuid(),
                    CutInNo,
                    null,
                    "SEWING",
                    request.RONo,
                    request.Article,
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.SewingOutDate.GetValueOrDefault(),
                    0
                    );

                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(
                            Guid.NewGuid(),
                            garmentCuttingIn.Identity,
                            Guid.Empty,
                            0,
                            null,
                            sewingOutId,
                            sewingOutNo
                            );

                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(
                                    Guid.NewGuid(),
                                    garmentCuttingInItem.Identity,
                                    Guid.Empty,
                                    item.Id,
                                    detail.Id,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(detail.Quantity),
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit,
                                    detail.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price*25/100))*detail.Quantity ,
                                    0,
                                    item.Color
                                    );

                                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                            }
                        }
                        else
                        {
                            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(
                                    Guid.NewGuid(),
                                    garmentCuttingInItem.Identity,
                                    Guid.Empty,
                                    item.Id,
                                    Guid.Empty,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(item.Quantity),
                                    new UomId(item.Uom.Id),
                                    item.Uom.Unit,
                                    item.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * item.Quantity,
                                    0,
                                    item.Color
                                    );
                            await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                        }

                        await _garmentCuttingInItemRepository.Update(garmentCuttingInItem);
                    }
                }

                await _garmentCuttingInRepository.Update(garmentCuttingIn);
            }

            #endregion

            #region Create SewingIn
            if (request.SewingTo == "SEWING")
            {
                GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                    Guid.NewGuid(),
                    GenerateSewingInNo(request),
                    "SEWING",
                    Guid.Empty,
                    "",
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.RONo,
                    request.Article,
                    new GarmentComodityId(request.Comodity.Id),
                    request.Comodity.Code,
                    request.Comodity.Name,
                    request.SewingOutDate.GetValueOrDefault()
                );
                await _garmentSewingInRepository.Update(garmentSewingIn);
                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                                    Guid.NewGuid(),
                                    garmentSewingIn.Identity,
                                    item.Id,
                                    detail.Id,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit,
                                    item.Color,
                                    detail.Quantity,
                                    item.BasicPrice,
                                    item.Price
                                );
                                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                            }
                        }
                        else
                        {
                            GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                                    Guid.NewGuid(),
                                    garmentSewingIn.Identity,
                                    item.Id,
                                    Guid.Empty,
                                    Guid.Empty,
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
                            await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                        }
                    }
                }
            }
            #endregion

            #region Create FinishingIn
            if (request.SewingTo == "FINISHING")
            {
                GarmentFinishingIn garmentFinishingIn = new GarmentFinishingIn(
                    Guid.NewGuid(),
                    GenerateFinishingInNo(request),
                    "SEWING",
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.RONo,
                    request.Article,
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.SewingOutDate.GetValueOrDefault(),
                    new GarmentComodityId(request.Comodity.Id),
                    request.Comodity.Code,
                    request.Comodity.Name,
                    0,
                    null,
                    null
                );
                await _garmentFinishingInRepository.Update(garmentFinishingIn);
                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                GarmentFinishingInItem garmentFinishingInItem = new GarmentFinishingInItem(
                                    Guid.NewGuid(),
                                    garmentFinishingIn.Identity,
                                    item.Id,
                                    detail.Id,
                                    Guid.Empty,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    detail.Quantity,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit,
                                    item.Color,
                                    item.BasicPrice,
                                    item.Price
                                );
                                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                            }
                        }
                        else
                        {
                            GarmentFinishingInItem garmentFinishingInItem = new GarmentFinishingInItem(
                                Guid.NewGuid(),
                                garmentFinishingIn.Identity,
                                item.Id,
                                Guid.Empty,
                                Guid.Empty,
                                new SizeId(item.Size.Id),
                                item.Size.Size,
                                new ProductId(item.Product.Id),
                                item.Product.Code,
                                item.Product.Name,
                                item.DesignColor,
                                item.Quantity,
                                item.RemainingQuantity,
                                new UomId(item.Uom.Id),
                                item.Uom.Unit,
                                item.Color,
                                item.BasicPrice,
                                item.Price
                            );
                            await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                        }
                    }
                }
            }
            #endregion

            #region Create FinishedGoodStock and History
            if (request.SewingTo == "BARANG JADI")
            {
                GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
                Dictionary<string, double> finGood = new Dictionary<string, double>();
                //take Finished Good Stock to be update
                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                //Push data finished good stock to be updated
                                string finStock = detail.Size.Id + "~" + detail.Size.Size + "~" + detail.Uom.Id + "~" + detail.Uom.Unit + "~" + item.BasicPrice;

                                if (finGood.ContainsKey(finStock))
                                {
                                    finGood[finStock] += detail.Quantity;
                                }
                                else
                                {
                                    finGood.Add(finStock, detail.Quantity);
                                }

                            }
                        }
                        else
                        {
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
                        }
                    }
                }

                //update finished good stock
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
                            && a.FinishedFrom == "SEWING"
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
                                        "SEWING"
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

                //create finished goods stock history
                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (item.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                                    a.Article == request.Article &&
                                    a.BasicPrice == item.BasicPrice &&
                                    a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                                    a.SizeId == new SizeId(detail.Size.Id) &&
                                    a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                                    a.UomId == new UomId(detail.Uom.Id)).Single();

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
                                        detail.Quantity,
                                        stock.BasicPrice,
                                        price,
                                        Guid.Empty,
                                        Guid.Empty,
                                        Guid.Empty,
                                        Guid.Empty,
                                        sewingOutId,
                                        item.Id,
                                        detail.Id
                                    );
                                await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                            }
                        }
                        else
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
                                    Guid.Empty,
                                    Guid.Empty,
                                    sewingOutId,
                                    item.Id,
                                    Guid.Empty
                                );
                            await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                        }
                        

                    }
                }


            }

            #endregion
            _storage.Save();

            return garmentSewingOut;
        }

        private string GenerateSewOutNo(PlaceGarmentSewingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SO{request.Unit.Code.Trim()}{year}{month}";

            var lastSewOutNo = _garmentSewingOutRepository.Query.Where(w => w.SewingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingOutNo)
                .Select(s => int.Parse(s.SewingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewOutNo = $"{prefix}{(lastSewOutNo + 1).ToString("D4")}";

            return SewOutNo;
        }

        private string GenerateSewingInNo(PlaceGarmentSewingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"SI{request.UnitTo.Code}{year}{month}";

            var lastSewingInNo = _garmentSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }

        private string GenerateFinishingInNo(PlaceGarmentSewingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.UnitTo.Code;

            var prefix = $"FI{unitcode}{year}{month}";

            var lastFinishingInNo = _garmentFinishingInRepository.Query.Where(w => w.FinishingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingInNo)
                .Select(s => int.Parse(s.FinishingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastFinishingInNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}