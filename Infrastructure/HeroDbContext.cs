using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class HeroDbContext : DbContext
    {
        public HeroDbContext(DbContextOptions<HeroDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Hero>()
              .HasOne(i => i.Trainer)
              .WithMany(c => c.Heroes)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TrainingSession>()
               .HasOne(i => i.Hero)
               .WithMany(c => c.TrainingSessions)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TrainingSession>()
                .HasOne(i => i.Trainer)
                .WithMany(c => c.TrainingSessions)
                .OnDelete(DeleteBehavior.Restrict);
        }
       
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Hero> Heroes { get; set; } 
        public DbSet<TrainingSession> TrainingSessions { get; set; }
    }
}
