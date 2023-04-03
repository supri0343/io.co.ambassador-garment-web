using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetCustomsByRO
{
    public class GetCustomsByROQuery:IQuery<GetCustomsByROViewModel>
    {
        public string Ro { get; private set; }

        public GetCustomsByROQuery(string Ro, string token)
        {
            this.Ro = Ro;
        }
    }
}
