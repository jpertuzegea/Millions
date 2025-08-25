//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Entities
{
    public class Owner
    {
        [Key]
        public int? IdOwner { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

       
        // public string Photo { get; set; }
        public byte[]? Photo { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }


        public DateTime? Birthday { get; set; }

        // Navigation property
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
