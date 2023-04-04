using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetCustomsByRO
{
    public class GetCustomsByRODto
    {
        public GetCustomsByRODto()
        {
                
        }
        public string RONo { get; set; }
        public GetCustomsByRODto(GetCustomsByRODto data)
        {
            RONo = data.RONo;

        }
    }
}
