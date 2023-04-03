using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Manufactures.Application.GarmentPreparings.Queries.GetCustomsByRO
{
    public class GetCustomsByROQueryHandler :  IQueryHandler<GetCustomsByROQuery, GetCustomsByROViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentPreparingRepository garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
        public GetCustomsByROQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();


            _http = serviceProvider.GetService<IHttpClientService>();
        }
        public async Task<GetCustomsByROViewModel> Handle(GetCustomsByROQuery request, CancellationToken cancellationToken)
        {
            var ro = request.Ro.Contains(",") ? request.Ro.Split(",").ToList() : new List<string> { request.Ro };

            var Query = (from a in garmentPreparingRepository.Query
                        join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
                        where ro.Contains(b.ROSource) && b.CustomsCategory == "Fasilitas"
                        select new GetCustomsByRODto
                        {
                            RONo = b.ROSource,
                        }).Distinct().ToList();
            GetCustomsByROViewModel GetCustomsByROViewModel = new GetCustomsByROViewModel();
            GetCustomsByROViewModel.getCustomsByRO = Query;
            return GetCustomsByROViewModel;
                        
        }
    }
}
