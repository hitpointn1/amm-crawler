using AMMCrawler.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace AMMCrawler
{
    public class CrawlerContext : DbContext
    {
        public CrawlerContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<ResourceLink> ResourceLinks { get; set; }
        public DbSet<ResourceCrawlMapping> ResourceMappings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(CrawlerApplication.Configuration.GetConnectionString(nameof(CrawlerContext)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResourceCrawlMapping>()
                .HasKey(x => new { x.FoundLinkID, x.CrawledLinkID });

            modelBuilder.Entity<ResourceCrawlMapping>()
                .HasOne(x => x.FoundLink)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ResourceCrawlMapping>()
                .HasOne(x => x.CrawledLink)
                .WithMany(x => x.Crawls)
                .HasForeignKey(x => x.FoundLinkID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
