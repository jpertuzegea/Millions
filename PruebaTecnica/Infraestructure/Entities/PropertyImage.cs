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
    public class PropertyImage
    {
        [Key]
        public int? IdPropertyImage { get; set; }
        [ForeignKey("Property")]
        public int IdProperty { get; set; }


        //  public string FilePath { get; set; }


        public byte[]? Photo { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }


        public bool Enabled { get; set; }
         


        public Property Property { get; set; }
    }
}
