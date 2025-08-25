//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

namespace Commons.Dtos.Domain
{
    public class SearchDto
    {

        public string? Name { get; set; }
        public string? Address { get; set; }

        public int? PriceMin { get; set; }
        public int? PriceMax { get; set; }

    }
}
