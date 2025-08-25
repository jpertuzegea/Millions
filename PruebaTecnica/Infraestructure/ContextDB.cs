
//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infraestructure
{
    public class ContextDB : DbContext
    {
        private readonly IConfiguration _configuration;

        public ContextDB(DbContextOptions<ContextDB> options, IConfiguration configuration) : base(options)
        {
            this._configuration = configuration;
        }

        public ContextDB(IConfiguration _IConfiguration)
        {
            _configuration = _IConfiguration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _configuration.GetConnectionString("BDConnetion"),
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                );
        }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<Property> Property { get; set; }
        public DbSet<PropertyImage> PropertyImage { get; set; }
        public DbSet<PropertyTrace> PropertyTrace { get; set; }
        //   

    }
}
