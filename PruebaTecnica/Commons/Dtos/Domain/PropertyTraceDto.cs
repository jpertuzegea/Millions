//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

namespace Commons.Dtos.Domain
{
    public class PropertyTraceDto
    {

        public int? IdPropertyTrace { get; set; }
        public DateTime? DateSale { get; set; }
        public string? Name { get; set; }
        public decimal? Value { get; set; }
        public decimal? Tax { get; set; }


        public int IdProperty { get; set; }
        public string? IdPropertyName { get; set; }


    }
}
