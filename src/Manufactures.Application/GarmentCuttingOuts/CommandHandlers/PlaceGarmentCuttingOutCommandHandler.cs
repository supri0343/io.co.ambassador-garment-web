using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.CommandHandlers
{
    public class PlaceGarmentCuttingOutCommandHandler : ICommandHandler<PlaceGarmentCuttingOutCommand, GarmentCuttingOut>
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

        public PlaceGarmentCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentComodityPriceRepository= storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        }

        public async Task<GarmentCuttingOut> Handle(PlaceGarmentCuttingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true && item.Details.Count() > 0).ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).OrderBy(o => o.ModifiedDate).Select(s => new GarmentComodityPrice(s)).Last();
            
            if(request.CuttingOutType == "LOADING")
            {
                request.CuttingOutType = "SEWING";
            }
            
            GarmentCuttingOut garmentCuttingOut = new GarmentCuttingOut(
                Guid.NewGuid(),
                GenerateCutOutNo(request),
                request.CuttingOutType,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.CuttingOutDate.GetValueOrDefault(),
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.IsUsed
            );

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();

            //jika CuttingOutType == "SEWING" maka buat GarmentSewingDO
            if (request.CuttingOutType == "SEWING")
            {
                GarmentSewingDO garmentSewingDO = new GarmentSewingDO(
                    Guid.NewGuid(),
                    GenerateSewingDONo(request),
                    garmentCuttingOut.Identity,
                    new UnitDepartmentId(request.UnitFrom.Id),
                    request.UnitFrom.Code,
                    request.UnitFrom.Name,
                    new UnitDepartmentId(request.Unit.Id),
                    request.Unit.Code,
                    request.Unit.Name,
                    request.RONo,
                    request.Article,
                    new GarmentComodityId(request.Comodity.Id),
                    request.Comodity.Code,
                    request.Comodity.Name,
                    request.CuttingOutDate.GetValueOrDefault()
                );

                foreach (var item in request.Items)
                {
                    GarmentCuttingOutItem garmentCuttingOutItem = new GarmentCuttingOutItem(
                        Guid.NewGuid(),
                        item.CuttingInId,
                        item.CuttingInDetailId,
                        garmentCuttingOut.Identity,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        item.TotalCuttingOutQuantity
                    );

                    foreach (var detail in item.Details)
                    {
                        GarmentCuttingOutDetail garmentCuttingOutDetail = new GarmentCuttingOutDetail(
                            Guid.NewGuid(),
                            garmentCuttingOutItem.Identity,
                            new SizeId(detail.Size.Id),
                            detail.Size.Size,
                            detail.Color.ToUpper(),
                            0,
                            detail.CuttingOutQuantity,
                            new UomId(detail.CuttingOutUom.Id),
                            detail.CuttingOutUom.Unit,
                            detail.BasicPrice,
                            detail.Price
                        );

                        if (cuttingInDetailToBeUpdated.ContainsKey(item.CuttingInDetailId))
                        {
                            cuttingInDetailToBeUpdated[item.CuttingInDetailId] += detail.CuttingOutQuantity;
                        }
                        else
                        {
                            cuttingInDetailToBeUpdated.Add(item.CuttingInDetailId, detail.CuttingOutQuantity);
                        }

                        await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                        double price = (detail.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * detail.CuttingOutQuantity;
                        GarmentSewingDOItem garmentSewingDOItem = new GarmentSewingDOItem(
                            Guid.NewGuid(),
                            garmentSewingDO.Identity,
                            garmentCuttingOutDetail.Identity,
                            garmentCuttingOutItem.Identity,
                            new ProductId(item.Product.Id),
                            item.Product.Code,
                            item.Product.Name,
                            item.DesignColor,
                            new SizeId(detail.Size.Id),
                            detail.Size.Size,
                            detail.CuttingOutQuantity,
                            new UomId(detail.CuttingOutUom.Id),
                            detail.CuttingOutUom.Unit,
                            detail.Color.ToUpper(),
                            detail.CuttingOutQuantity,
                            detail.BasicPrice,
                            price
                        );

                        await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);

                    }
                    await _garmentCuttingOutItemRepository.Update(garmentCuttingOutItem);
                }

                await _garmentSewingDORepository.Update(garmentSewingDO);

            }
            Dictionary<string, double> finGood = new Dictionary<string, double>();
            //jika CuttingOutType == "BARANG JADI" maka buat 
            if (request.CuttingOutType == "BARANG JADI")
            {

                foreach (var item in request.Items)
                {
                    GarmentCuttingOutItem garmentCuttingOutItem = new GarmentCuttingOutItem(
                        Guid.NewGuid(),
                        item.CuttingInId,
                        item.CuttingInDetailId,
                        garmentCuttingOut.Identity,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        item.TotalCuttingOutQuantity
                    );
                    item.Id = garmentCuttingOutItem.Identity;
                  
                    foreach (var detail in item.Details)
                    {
                        GarmentCuttingOutDetail garmentCuttingOutDetail = new GarmentCuttingOutDetail(
                            Guid.NewGuid(),
                            garmentCuttingOutItem.Identity,
                            new SizeId(detail.Size.Id),
                            detail.Size.Size,
                            detail.Color.ToUpper(),
                            0,
                            detail.CuttingOutQuantity,
                            new UomId(detail.CuttingOutUom.Id),
                            detail.CuttingOutUom.Unit,
                            detail.BasicPrice,
                            detail.Price
                        );

                        detail.Id = garmentCuttingOutDetail.Identity;
                        //Push data cutting in detail to be updated
                        if (cuttingInDetailToBeUpdated.ContainsKey(item.CuttingInDetailId))
                        {
                            cuttingInDetailToBeUpdated[item.CuttingInDetailId] += detail.CuttingOutQuantity;
                        }
                        else
                        {
                            cuttingInDetailToBeUpdated.Add(item.CuttingInDetailId, detail.CuttingOutQuantity);
                        }

                        //Push data finished good stock to be updated
                        string finStock = detail.Size.Id + "~" + detail.Size.Size + "~" + detail.CuttingOutUom.Id + "~" + detail.CuttingOutUom.Unit + "~" + detail.BasicPrice;

                        if (finGood.ContainsKey(finStock))
                        {
                            finGood[finStock] += detail.CuttingOutQuantity;
                        }
                        else
                        {
                            finGood.Add(finStock, detail.CuttingOutQuantity);
                        }

                        await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);
                    }
                    await _garmentCuttingOutItemRepository.Update(garmentCuttingOutItem);
                }
            }

            //Update data cutting in detail
            foreach (var cuttingInDetail in cuttingInDetailToBeUpdated)
            {
                var garmentCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInDetail.Key).Select(s => new GarmentCuttingInDetail(s)).Single();
                garmentCuttingInDetail.SetRemainingQuantity(garmentCuttingInDetail.RemainingQuantity - cuttingInDetail.Value);
                garmentCuttingInDetail.Modify();

                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
            }

            //Update data finished good stock
            if (request.CuttingOutType == "BARANG JADI")
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
                            && a.FinishedFrom == "CUTTING"
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
                                        "CUTTING"
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
                        foreach (var detail in item.Details)
                        {
                            var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                             a.Article == request.Article &&
                             a.BasicPrice == detail.BasicPrice &&
                             a.UnitId == new UnitDepartmentId(request.Unit.Id) &&
                             a.SizeId == new SizeId(detail.Size.Id) &&
                             a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                             a.UomId == new UomId(detail.CuttingOutUom.Id)).Single();

                            double price = (stock.BasicPrice + (double)garmentComodityPrice.Price) * detail.CuttingOutQuantity;

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
                                    detail.CuttingOutQuantity,
                                    stock.BasicPrice,
                                    price,
                                    item.Id,
                                    detail.Id,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty
                                );
                            await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                        }
                    }
                }
            }

            await _garmentCuttingOutRepository.Update(garmentCuttingOut);

            _storage.Save();

            return garmentCuttingOut;
        }

        private string GenerateCutOutNo(PlaceGarmentCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"CR{request.UnitFrom.Code}{year}{month}";

            var lastCutOutNo = _garmentCuttingOutRepository.Query.Where(w => w.CutOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutOutNo)
                .Select(s => int.Parse(s.CutOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutOutNo = $"{prefix}{(lastCutOutNo + 1).ToString("D4")}";

            return CutOutNo;
        }

        private string GenerateSewingDONo(PlaceGarmentCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"DS{request.Unit.Code}{year}{month}";

            var lastSewingDONo = _garmentSewingDORepository.Query.Where(w => w.SewingDONo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingDONo)
                .Select(s => int.Parse(s.SewingDONo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingDONo = $"{prefix}{(lastSewingDONo + 1).ToString("D4")}";

            return SewingDONo;
        }
    }
}