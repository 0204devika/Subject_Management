using Microsoft.EntityFrameworkCore;
using SubManagement.Models;

namespace SubManagement.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<SubTopic> Subtopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Topic>()
                .HasMany(t => t.Subtopics)
                .WithOne(st => st.Topic)
                .HasForeignKey(st => st.TopicId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
