using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System.Net.Http;
using System.Text;
using System.Net;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries
{
	public class GarmentExpenditureGoodQueryHandler : IQueryHandler<GetMonitoringExpenditureGoodQuery, GarmentMonitoringExpenditureGoodListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
        public GarmentExpenditureGoodQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
		}
 
		class monitoringView
		{
            public Guid expenditureGoodItemId { get; internal set; }
            public string expenditureGoodNo { get; internal set; }
			public string expenditureGoodType { get; internal set; }
			public string buyerCode { get; internal set; }
			public DateTimeOffset expenditureDate { get; internal set; }
			public string roNo { get; internal set; }
			public string buyerArticle { get; internal set; }
			public string colour { get; internal set; }
			public string name { get; internal set; }
            public string unitname { get; internal set; }
            public string comodityCode { get; internal set; }
            public string comodityName { get; internal set; }
            public string uomUnit { get; internal set; }
            public double qty { get; internal set; }
			public string invoice { get; internal set; }
			public string productCode { get; internal set; }
			public decimal price { get; internal set; }
			public double fc { get; internal set; }
            public string UId { get; internal set; }
        }

        public async Task<PEBResult> GetDataPEB(List<string> invoice, string token)
        {
            PEBResult pEB = new PEBResult();

            var listInvoice = string.Join(",", invoice.Distinct());
            var stringcontent = new StringContent(JsonConvert.SerializeObject(listInvoice), Encoding.UTF8, "application/json");

            var garmentProductionUri = CustomsDataSettings.Endpoint + $"customs-reports/getPEB";
            var httpResponse = await _http.SendAsync(HttpMethod.Get, garmentProductionUri, token, stringcontent);



            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();

                var listdata = JsonConvert.DeserializeObject<List<PEBResultViewModel>>(dataString);

                foreach (var i in listdata)
                {
                    pEB.data.Add(i);
                }
                //garmentProduct.data = listdata;



            }

            return pEB;
        }


        public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
            var stringcontent = new StringContent(JsonConvert.SerializeObject(listRO), Encoding.UTF8, "application/json");
			var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data";

            //var httpResponse = await _http.GetAsync(costCalculationUri, token);
            var httpResponse = await _http.SendAsync(HttpMethod.Get, costCalculationUri, token, stringcontent);

			var freeRO = new List<string>();

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();
				var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.ro == item);
					if (data != null)
					{
						costCalculationGarmentDataProductionReport.data.Add(data);
					}
					else
					{
						freeRO.Add(item);
					}
				}
			}
 
			return costCalculationGarmentDataProductionReport;
		}

        private async Task<List<GarmentProductViewModel>> GetProductCode(string codes, string token)
        { 
            var garmentProductionUri = MasterDataSettings.Endpoint + $"master/garmentProducts/byCode?code=" + codes;
            var listInvoice = string.Join(",", codes.Distinct());
            var stringcontent = new StringContent(JsonConvert.SerializeObject(listInvoice), Encoding.UTF8, "application/json");
            var httpResponse = await _http.SendAsync(HttpMethod.Get, garmentProductionUri, token, stringcontent);
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = httpResponse.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                List<GarmentProductViewModel> viewModel;
                if (result.GetValueOrDefault("data") == null)
                {
                    viewModel = new List<GarmentProductViewModel>();
                }
                else
                {
                    viewModel = JsonConvert.DeserializeObject<List<GarmentProductViewModel>>(result.GetValueOrDefault("data").ToString());

                }
                return viewModel;
            }
            else
            {
                List<GarmentProductViewModel> viewModel = new List<GarmentProductViewModel>();
                return viewModel;
            }
        }

        class ViewFC
		{
			public string RO { get; internal set; }
			public double FC { get; internal set; }
			public int Count { get; internal set; }
			//Enhance Jason Aug 2021
			public double AvgFC { get; set; }

		}
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal BasicPrice { get; internal set; }
			public int Count { get; internal set; }
			//Enhance Jason Aug 2021
			public double AvgBasicPrice { get; set; }

		}
		public async Task<GarmentMonitoringExpenditureGoodListViewModel> Handle(GetMonitoringExpenditureGoodQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7);


            //var QueryRo = (from a in garmentExpenditureGoodRepository.Query
            //						where a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.ExpenditureDate  >= dateFrom && a.ExpenditureDate <= dateTo
            //						select a.RONo).Distinct();

            //List<string> _ro = new List<string>();
            //foreach (var item in QueryRo)
            //{
            //	_ro.Add(item);
            //}
            //CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
            GarmentMonitoringExpenditureGoodListViewModel listViewModel = new GarmentMonitoringExpenditureGoodListViewModel();
            List<GarmentMonitoringExpenditureGoodDto> monitoringDtos = new List<GarmentMonitoringExpenditureGoodDto>();
            //var sumbasicPrice = (from a in garmentPreparingRepository.Query
            //					 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
            //					 where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) &&*/
            //					 a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)
            //					 select new { a.RONo, b.BasicPrice })
            //			.GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
            //			{
            //				RO = key.RONo,
            //				BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
            //				Count = group.Count(),
            //				AvgBasicPrice = (double)(Convert.ToDecimal(group.Sum(s => s.BasicPrice)) / group.Count())
            //                     });

            //var sumFCs = (from a in garmentCuttingInRepository.Query
            //			  where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && */ a.CuttingType == "Main Fabric" &&
            //			 a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.CuttingInDate <= dateTo
            //			  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
            //			  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
            //			  select new { a.FC, a.RONo, FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC), c.CuttingInQuantity })
            //		   .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
            //		   {
            //			   RO = key.RONo,
            //			   FC = group.Sum(s => (s.FCs)),
            //			   Count = group.Sum(s => s.CuttingInQuantity),
            //			   AvgFC = group.Sum(s => (s.FCs)) / group.Sum(s => s.CuttingInQuantity)
            //		   });
            //      var Query = from a in (from aa in garmentExpenditureGoodRepository.Query
            //		   where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ExpenditureDate >= dateFrom && aa.ExpenditureDate <= dateTo
            //		   select aa)
            //		   join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
            //where a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo

            ////Enhance Jason Aug 2021
            ////select new monitoringView { fc = (from aa in sumFCs where aa.RO == a.RONo select aa.FC / aa.Count).FirstOrDefault(),
            //select new monitoringView { fc = (from aa in sumFCs where aa.RO == a.RONo select aa.AvgFC).FirstOrDefault(),
            //	//price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()),
            //	price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.AvgBasicPrice).FirstOrDefault()),
            //	buyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(),
            //                      buyerArticle = a.BuyerCode + " " + a.Article, roNo = a.RONo, expenditureDate = a.ExpenditureDate, expenditureGoodNo = a.ExpenditureGoodNo,
            //                      expenditureGoodType = a.ExpenditureType, invoice = a.Invoice, colour = b.Description, qty = b.Quantity,
            //                      name = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(),
            //                      unitname = a.UnitName};

            var Query = (from a in (from aa in garmentExpenditureGoodRepository.Query
                                   where aa.ExpenditureDate.AddHours(7).Date >= dateFrom.Date && aa.ExpenditureDate.AddHours(7).Date <= dateTo.Date
                                   select aa)
                        join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                        join c in garmentPreparingRepository.Query on a.RONo equals c.RONo
                        join d in garmentPreparingItemRepository.Query on c.Identity equals d.GarmentPreparingId
                        where d.CustomsCategory == "FASILITAS"              
                        //select new monitoringView { fc = (from aa in sumFCs where aa.RO == a.RONo select aa.FC / aa.Count).FirstOrDefault(),
                        select new monitoringView
                        {
                            //fc = (from aa in sumFCs where aa.RO == a.RONo select aa.AvgFC).FirstOrDefault(),
                            //price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()),
                            //price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.AvgBasicPrice).FirstOrDefault()),
                            //buyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(),
                            expenditureDate = a.ExpenditureDate,
                            expenditureGoodNo = a.ExpenditureGoodNo,
                            expenditureGoodItemId = b.Identity,
                            //buyerArticle = a.BuyerCode + " " + a.Article,
                            comodityCode = a.ComodityCode,
                            comodityName = a.ComodityName,
                            uomUnit = b.UomUnit,
                            //itemCode = 
                            //roNo = a.RONo,
                            //expenditureGoodType = a.ExpenditureType,
                            invoice = a.Invoice,
                            //colour = b.Description,
                            qty = b.Quantity,
                            productCode = d.ProductCode,
                            UId = a.UId
                            //name = (from cost in costCalculation.data where cost.ro == a.RONo select cost.comodityName).FirstOrDefault(),
                            //unitname = a.UnitName
                        }).Distinct();

            var querySum = Query.ToList().GroupBy(x => new {x.expenditureDate, x.expenditureGoodNo, x.invoice,x.comodityCode,x.comodityName,x.uomUnit,x.productCode,x.UId }, (key, group) => new
			{
				//ros = key.roNo,
				//buyer = key.buyerArticle,
				expenditureDates = key.expenditureDate,
				qty = group.Sum(s => s.qty),
				expendituregoodNo = key.expenditureGoodNo,
				//expendituregoodTypes = key.expenditureGoodType,
				//color = key.colour,
				//price= group.Sum(s=>s.price),
				//buyerC= key.buyerCode,
				//names = key.name,
    //            unitname = key.unitname,
				invoices = key.invoice,
                //fcs = key.fc
                comodityCode = key.comodityCode,
                comodityName = key.comodityName,
                uomUnit = key.uomUnit,
                productCode = key.productCode,
                key.UId
                

            }).OrderBy(s => s.expendituregoodNo);

            var Pebs = await GetDataPEB(querySum.Select(x => x.invoices).ToList(), request.token);
            var Codes = await GetProductCode(string.Join(",", querySum.Select(x => x.productCode).ToHashSet()), request.token);
            string[] exceptionBonNo = { "EGEAG223100009", "EGEAG223100060" };
            foreach (var item in querySum)
			{
                var peb = Pebs.data.FirstOrDefault(x => x.BonNo.Trim() == item.invoices);
                DateTime? non = null;

                var remark = Codes.FirstOrDefault(x => x.Code == item.productCode);

                var finalRemark = remark != null ? " - " + remark.Composition/* + " " + remark.Width + " " + remark.Const + " " + remark.Yarn*/ : "";

                GarmentMonitoringExpenditureGoodDto dto = new GarmentMonitoringExpenditureGoodDto
                {
                    //roNo = item.ros,
                    //buyerArticle = item.buyer,
                    //expenditureGoodType = item.expendituregoodTypes,
                    pebDate = peb == null ? "-" : peb.BCDate.ToString("dd MMM yyyy"),
                    pebNo = peb == null ? "-": peb.BCNo,
                    buyerName = peb == null ? "-" : peb.BuyerName,
                    country = peb == null ? "-" : peb.Country,
                    currencyCode = peb == null ? "-" : peb.CurrencyCode,
                    expenditureGoodNo = item.expendituregoodNo,
					expenditureDate = item.expenditureDates,
					qty = item.qty,
                    comodityCode = item.comodityCode,
                    comodityName = item.comodityName + " - " + (exceptionBonNo.Contains(item.expendituregoodNo) ? item.UId:  finalRemark),
                    uomUnit = item.uomUnit,
                    price = (decimal)((peb == null ? 0 : peb.Nominal) * (peb == null ? 0 : peb.Quantity)),
                    //colour = item.color,
                    //name = item.names,
                    //unitname = item.unitname,
                    invoice = item.invoices,
					//price= Math.Round(Convert.ToDecimal(Convert.ToDouble( Math.Round(item.price,2)) * Math.Round(item.fcs,2)),2),
					//buyerCode=item.buyerC

				};
				monitoringDtos.Add(dto);
			}
			listViewModel.garmentMonitorings = monitoringDtos;
			return listViewModel;
		}
	}
}
