//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entities
{
    public class Property
    {
        [Key]
        public int? IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int? Year { get; set; }

        [ForeignKey("Owner")]
        public int IdOwner { get; set; }

        // Navigation properties
        public Owner Owner { get; set; }
        public ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
    }
}
