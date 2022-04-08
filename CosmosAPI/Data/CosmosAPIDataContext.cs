using CosmosAPI.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using PieroDeTomi.EntityFrameworkCore.Identity.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosAPI.Data
{
    public class CosmosAPIDataContext : CosmosIdentityDbContext<Identity>
    {
        public CosmosAPIDataContext(DbContextOptions dbContextOptions, IOptions<OperationalStoreOptions> options) : base(dbContextOptions, options)
        {
            
    }
        public DbSet<Post> Posts { get; set; }
        public DbSet<CreatedBy> ArtCreators { get; set; }
        public DbSet<Files> Files { get; set; }
        //public DbSet<CosmosAPI.Models.FileType> FileTypes { get; set; }
        public DbSet<Medium> Mediums { get; set; }
        //public DbSet<CosmosAPI.Models.User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<CosmosAPI.Models.UserComment> UserComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // DO NOT REMOVE THIS LINE. If you do, your context won't work as expected.
            base.OnModelCreating(builder);

            // TODO: Add your own fluent mappings

            builder
            .Entity<CategoryIds>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<Category>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<Files>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<Medium>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<CreatedBy>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<CreatorIds>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<PostIds>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<MediumIds>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());

            builder
            .Entity<Post>()
            .Property(o => o.id)
            .HasConversion(new GuidIdValueConverter());




        }
    }


    public class GuidIdValueConverter : ValueConverter<Guid, string>
    {
        public GuidIdValueConverter(ConverterMappingHints mappingHints = null)
            : base(
                id => id.ToString(),
                value => new Guid(value),
                mappingHints
            )
        { }
    }
}

