using AMMCrawler.Core;
using AMMCrawler.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AMMCrawler.DAL
{
    public class CrawlerContext : DbContext
    {
        public CrawlerContext()
        {
        }

        public CrawlerContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<CrawlRunInfo> RunInfo { get; set; }
        public DbSet<ResourceLink> ResourceLinks { get; set; }
        public DbSet<ResourceCrawlMapping> ResourceMappings { get; set; }
        public DbSet<ETCLinkMetadata> ETCLinksMetadata { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(CrawlConfiguration.CrawlerContext);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResourceCrawlMapping>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<ResourceCrawlMapping>()
                .HasOne(x => x.FoundLink)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ResourceCrawlMapping>()
                .HasOne(x => x.CrawledLink)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
