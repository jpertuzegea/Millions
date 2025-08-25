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
    public class PropertyTrace
    {
        [Key]
        public int? IdPropertyTrace { get; set; }
        public DateTime? DateSale { get; set; }
        public string? Name { get; set; }
        public decimal? Value { get; set; }
        public decimal? Tax { get; set; }

        // Foreign key
        [ForeignKey("Property")]
        public int IdProperty { get; set; }

        // Navigation property
        public Property Property { get; set; }
    }
}
