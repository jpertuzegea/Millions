//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;

namespace Commons.Dtos.Domain
{
    public class OwnerModel
    {
        public int? IdOwner { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }


        public IFormFile? File { get; set; }
        public byte[]? Photo { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }


        public DateTime? Birthday { get; set; }

    }
}
