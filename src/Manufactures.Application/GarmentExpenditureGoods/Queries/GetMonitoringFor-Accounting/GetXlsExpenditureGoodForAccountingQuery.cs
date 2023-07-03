using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Application.GarmentExpenditureGoods.Queries.GetMonitoringForAccounting
{
	public class GetXlsExpenditureGoodForAccountingQuery : IQuery<MemoryStream>
	{
		public int page { get; private set; }
		public int size { get; private set; }
		public string order { get; private set; }
		public string token { get; private set; }
		public string type { get; private set; }
        public int unit { get; private set; }
		public string unitname { get; private set; }
		public DateTime dateFrom { get; private set; }
		public DateTime dateTo { get; private set; }

		public GetXlsExpenditureGoodForAccountingQuery(int page, int size, string order, DateTime dateFrom, DateTime dateTo,string type,string unitname,int unit, string token)
		{
			this.page = page;
			this.size = size;
			this.order = order;
            this.unit = unit;
			this.unitname = unitname;
			this.type = type;
			this.dateFrom = dateFrom;
			this.dateTo = dateTo;
			this.token = token;
		}
	}
}
