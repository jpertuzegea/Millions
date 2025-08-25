//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------
namespace Commons.Dtos.Domain
{
    public class PropertyDto
    {
        public int? IdProperty { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal Price { get; set; }
        public string? CodeInternal { get; set; }
        public int? Year { get; set; }

        public int IdOwner { get; set; }
        public string? OwnerName { get; set; }
         
    }
}
