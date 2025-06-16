﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.MonitoringProductionStockFlow;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods
{
    public class GetXlsMutationExpenditureGoodsQueryHandler : IQueryHandler<GetXlsMutationExpenditureGoodsQuery, MemoryStream>
    {
        private readonly IStorage _storage;
        private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceMonitoringProductionStockFlowRepository;
        private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
        private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
        private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutRepository garmentSampleCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository garmentSampleCuttingOutItemRepository;
        private readonly IGarmentSampleFinishingOutRepository garmentSampleFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository garmentSampleFinishingOutItemRepository;
        private readonly IGarmentSampleExpenditureGoodRepository garmentSampleExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository garmentSampleExpenditureGoodItemRepository;

        public GetXlsMutationExpenditureGoodsQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentBalanceMonitoringProductionStockFlowRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
            garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
            garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
            garmentSampleCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            garmentSampleCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            garmentSampleFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            garmentSampleFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            garmentSampleExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            garmentSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();

        }

        class mutationView
        {
            public Guid ItemGuid { get; internal set; }
            public double SaldoQtyFin { get; internal set; }
            public double QtyFin { get; internal set; }
            public double AdjFin { get; internal set; }
            public double Retur { get; internal set; }
            public double QtyExpend { get; internal set; }
            //public string Comodity { get; internal set; }
            public string ComodityCode { get; internal set; }
            public string ComodityName { get; internal set; }
        }

        public async Task<MemoryStream> Handle(GetXlsMutationExpenditureGoodsQuery request, CancellationToken cancellationToken)
        {
            GarmentMutationExpenditureGoodListViewModel expenditureGoodListViewModel = new GarmentMutationExpenditureGoodListViewModel();
            List<GarmentMutationExpenditureGoodDto> mutationExpenditureGoodDto = new List<GarmentMutationExpenditureGoodDto>();

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));
            DateTimeOffset dateBalance = (from a in garmentBalanceMonitoringProductionStockFlowRepository.Query
                                          select a.CreatedDate).FirstOrDefault();
            DateTimeOffset dateBalanceSample = new DateTimeOffset(2020, 08, 30, 0, 0, 0, new TimeSpan(7, 0, 0));

            var querybalance = from a in (from aa in garmentBalanceMonitoringProductionStockFlowRepository.Query
                                          where aa.CreatedDate < dateFrom
                                          select new { aa.BeginingBalanceExpenditureGood, aa.Ro, aa.Comodity })
                               join b in garmentCuttingOutRepository.Query on a.Ro equals b.RONo
                               //&& a.Comodity == "BOYS SHORTS"
                               select new mutationView
                               {
                                   SaldoQtyFin = a.BeginingBalanceExpenditureGood,
                                   AdjFin = 0,
                                   //Comodity = a.Comodity,
                                   ComodityName = a.Comodity,
                                   ComodityCode = b.ComodityCode,
                                   QtyExpend = 0,
                                   QtyFin = 0,
                                   Retur = 0,
                               };

            var adjust = (from a in (from aa in garmentAdjustmentRepository.Query
                                    where aa.AdjustmentDate.AddHours(7).Date >= dateBalance.Date && aa.AdjustmentDate.AddHours(7).Date <= dateTo.Date
                                    && aa.AdjustmentType == "FINISHING"
                                    select aa)
                         join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                         select new mutationView
                         {
                             ItemGuid = b.Identity,
                             SaldoQtyFin = a.AdjustmentDate.AddHours(7).Date < dateFrom.Date && a.AdjustmentDate.AddHours(7).Date > dateBalance.Date ? b.Quantity : 0,
                             //AdjFin = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                             ComodityCode = a.ComodityCode,
                             ComodityName = a.ComodityName,
                             QtyExpend = 0,
                             QtyFin = a.AdjustmentDate.AddHours(7).Date >= dateFrom.Date ? b.Quantity : 0,
                             Retur = 0,
                         }).Distinct();
            var returexpend = (from a in (from aa in garmentExpenditureGoodReturnRepository.Query
                                         where aa.ReturDate.AddHours(7).Date >= dateBalance.Date && aa.ReturDate.AddHours(7).Date <= dateTo.Date //&& aa.ComodityCode == "BR"
                                         select aa)
                              join b in garmentExpenditureGoodReturnItemRepository.Query on a.Identity equals b.ReturId
                              select new mutationView
                              {
                                  SaldoQtyFin = a.ReturDate.AddHours(7).Date < dateFrom.Date && a.ReturDate.AddHours(7).Date > dateBalance.Date ? b.Quantity : 0,
                                  AdjFin = 0,
                                  ComodityCode = a.ComodityCode,
                                  ComodityName = a.ComodityName,
                                  QtyExpend = 0,
                                  QtyFin = 0,
                                  Retur = a.ReturDate.AddHours(7).Date >= dateFrom.Date ? b.Quantity : 0,
                                  ItemGuid = b.Identity
                              }).Distinct();
            var finishingbarangjadi = (from a in (from aa in garmentFinishingOutRepository.Query
                                                 where aa.FinishingOutDate.AddHours(7).Date >= dateBalance && aa.FinishingOutDate.AddHours(7).Date <= dateTo
                                                 && aa.FinishingTo == "GUDANG JADI" //&& aa.ComodityCode == "BR"
                                                 select aa)
                                      join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                      select new mutationView
                                      {
                                          SaldoQtyFin = a.FinishingOutDate.AddHours(7).Date < dateFrom.Date && a.FinishingOutDate.AddHours(7).Date > dateBalance.Date ? b.Quantity : 0,
                                          AdjFin = 0,
                                          ComodityCode = a.ComodityCode,
                                          ComodityName = a.ComodityName,
                                          QtyExpend = 0,
                                          QtyFin = a.FinishingOutDate.AddHours(7).Date >= dateFrom.Date ? b.Quantity : 0,
                                          Retur = 0,
                                          ItemGuid = b.Identity
                                      }).Distinct();

            var factexpend = (from a in (from aa in garmentExpenditureGoodRepository.Query
                                        where aa.ExpenditureDate.AddHours(7).Date >= dateBalance.Date && aa.ExpenditureDate.AddHours(7).Date <= dateTo.Date //&& aa.ComodityCode == "BR"
                                        select aa)
                             join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                             select new mutationView
                             {
                                 SaldoQtyFin = a.ExpenditureDate.AddHours(7).Date < dateFrom.Date && a.ExpenditureDate.AddHours(7).Date > dateBalance.Date ? -b.Quantity : 0,
                                 AdjFin = 0,
                                 ComodityCode = a.ComodityCode,
                                 ComodityName = a.ComodityName,
                                 QtyExpend = a.ExpenditureDate.AddHours(7).Date >= dateFrom.Date ? b.Quantity : 0,
                                 QtyFin = 0,
                                 Retur = 0,
                                 ItemGuid = b.Identity
                             }).Distinct();

            var cuttingSample = from a in (from aa in garmentSampleCuttingOutRepository.Query
                                           where aa.CuttingOutDate >= dateBalanceSample && aa.CuttingOutDate <= dateTo
                                           select aa)
                                join b in garmentSampleCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
                                select new mutationView
                                {
                                    SaldoQtyFin = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalanceSample ? b.TotalCuttingOut : 0,
                                    AdjFin = 0,
                                    ComodityCode = a.ComodityCode,
                                    ComodityName = a.ComodityName,
                                    QtyExpend = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0,
                                    QtyFin = 0,
                                    Retur = 0,
                                };

            var finishingSample = (from a in (from aa in garmentSampleFinishingOutRepository.Query
                                             where aa.FinishingOutDate.AddHours(7).Date >= dateBalanceSample.Date && aa.FinishingOutDate.AddHours(7).Date <= dateTo.Date
                                             && aa.FinishingTo == "GUDANG JADI"
                                             select aa)
                                  join b in garmentSampleFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                  select new mutationView
                                  {
                                      SaldoQtyFin = a.FinishingOutDate.AddHours(7).Date < dateFrom.Date && a.FinishingOutDate.AddHours(7).Date > dateBalanceSample.Date ? b.Quantity : 0,
                                      AdjFin = 0,
                                      ComodityCode = a.ComodityCode,
                                      ComodityName = a.ComodityName,
                                      QtyExpend = 0,
                                      QtyFin = a.FinishingOutDate.AddHours(7).Date >= dateFrom.Date ? b.Quantity : 0,
                                      Retur = 0,
                                      ItemGuid = b.Identity
                                  }).Distinct();

            var expenditureGoodSample = (from a in (from aa in garmentSampleExpenditureGoodRepository.Query
                                                   where aa.ExpenditureDate.AddHours(7).Date >= dateBalanceSample.Date && aa.ExpenditureDate.AddHours(7).Date <= dateTo.Date
                                                   select aa)
                                        join b in garmentSampleExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                                        select new mutationView
                                        {
                                            SaldoQtyFin = a.ExpenditureDate.AddHours(7).Date < dateFrom.Date && a.ExpenditureDate.AddHours(7).Date > dateBalanceSample.Date ? -b.Quantity : 0,
                                            AdjFin = 0,
                                            ComodityCode = a.ComodityCode,
                                            ComodityName = a.ComodityName,
                                            QtyExpend = a.ExpenditureDate.AddHours(7).Date >= dateFrom.Date ? b.Quantity : 0,
                                            QtyFin = 0,
                                            Retur = 0,
                                            ItemGuid = b.Identity
                                        }).Distinct();

            var queryNow = adjust.Union(querybalance).Union(returexpend).Union(finishingbarangjadi).Union(factexpend).Union(cuttingSample).Union(finishingSample).Union(expenditureGoodSample).AsEnumerable();

            var mutationTemp = queryNow.GroupBy(x => new { x.ComodityCode,x.ComodityName }, (key, group) => new
            {
                kodeBarang = key.ComodityCode,
                namaBarang = key.ComodityName,
                //namaBarang = group.FirstOrDefault().Comodity,
                pemasukan = group.Sum(x => x.Retur + x.QtyFin),
                pengeluaran = group.Sum(x => x.QtyExpend),
                penyesuaian = 0,
                saldoAwal = group.Sum(x => x.SaldoQtyFin),
                //saldoBuku = group.Sum(x => x.SaldoQtyFin) + group.Sum(x => x.Retur + x.QtyFin) - group.Sum(x => x.QtyExpend),
                selisih = group.Sum(x => x.Retur + x.QtyFin) - group.Sum(x => x.QtyExpend),
                //stockOpname = 0,
                unitQtyName = "PCS",
                storage = "Gudang AG2"


            });

            foreach (var i in mutationTemp.Where(x => x.saldoAwal != 0 || x.pemasukan != 0 || x.pengeluaran != 0 || x.penyesuaian != 0 ))
            {
                //var comodity = (from a in garmentCuttingOutRepository.Query
                //                where a.ComodityCode == i.kodeBarang
                //                select a.ComodityName).FirstOrDefault();

                GarmentMutationExpenditureGoodDto dto = new GarmentMutationExpenditureGoodDto
                {
                    KodeBarang = i.kodeBarang,
                    NamaBarang = i.namaBarang,
                    Pemasukan = i.pemasukan,
                    Pengeluaran = i.pengeluaran,
                    //Penyesuaian = i.penyesuaian,
                    SaldoAwal = i.saldoAwal,
                    //SaldoBuku = i.saldoBuku,
                    Selisih = i.selisih,
                    //StockOpname = i.stockOpname,
                    UnitQtyName = i.unitQtyName,
                    Storage = i.storage
                };

                mutationExpenditureGoodDto.Add(dto);
            }

            expenditureGoodListViewModel.garmentMutations = mutationExpenditureGoodDto.OrderBy(x => x.KodeBarang).ToList();

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Sat", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Awal", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pemasukan", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pengeluaran", DataType = typeof(double) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Penyesuaian", DataType = typeof(double) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Buku", DataType = typeof(double) });
            //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Stock Opname", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(double) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Gudang", DataType = typeof(string) });
            int counter = 1;
            foreach (var report in expenditureGoodListViewModel.garmentMutations)
            {
                //reportDataTable.Rows.Add(counter, report.KodeBarang, report.NamaBarang, report.UnitQtyName, report.SaldoAwal, report.Pemasukan, report.Pengeluaran, report.Penyesuaian, report.SaldoBuku, report.StockOpname, report.Selisih);
                reportDataTable.Rows.Add(counter, report.KodeBarang, report.NamaBarang, report.UnitQtyName, report.SaldoAwal, report.Pemasukan, report.Pengeluaran, report.Selisih,report.Storage);

                counter++;
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A2"].LoadFromDataTable(reportDataTable, true, OfficeOpenXml.Table.TableStyles.Light16);

                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;

            }



        }
    }
}
