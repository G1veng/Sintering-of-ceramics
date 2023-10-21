using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Entity
{
    public class Context : DbContext
    {
        //Creation
        public Context(DbContextOptions<Context> options) : base(options)
        {
            
        }

        //Indexes
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TheoreticalMMParams>()
                .HasOne(x => x.Material)
                .WithOne(x => x.TheoreticalMMParam);

            modelBuilder.Entity<ExperimentalData>()
                .HasOne(x => x.FirstControlAction)
                .WithMany(x => x.FirstExperimentalDatas)
                .HasForeignKey(x => x.FirstControlActionId);

            modelBuilder.Entity<ExperimentalData>()
                .HasOne(x => x.SecondControlAction)
                .WithMany(x => x.SecondExperimentalDatas)
                .HasForeignKey(x => x.SecondControlActionId);

            modelBuilder.Entity<ExperimentalData>()
                .HasOne(x => x.ThirdControlAction)
                .WithMany(x => x.ThirdExperimentalDatas)
                .HasForeignKey(x => x.ThirdControlActionId);

            modelBuilder.Entity<Material>()
                .HasMany(x => x.Models)
                .WithOne(x => x.Material)
                .HasForeignKey(x => x.MaterialId);

            modelBuilder.Entity<MM>()
                .HasMany(x => x.MMCoefficients)
                .WithOne(x => x.MM);

            modelBuilder.Entity<MM>()
                .HasMany(x => x.Models)
                .WithOne(x => x.MM)
                .HasForeignKey(x => x.MMId);

            modelBuilder.Entity<Qualities>()
                .HasMany(x => x.ExperimentalDatas)
                .WithOne(x => x.Qualities);

            modelBuilder.Entity<Qualities>()
                .HasMany(x => x.Models)
                .WithOne(x => x.Qualities)
                .HasForeignKey(x => x.QualityId);

            modelBuilder.Entity<Equipment>()
                .HasMany(x => x.Models)
                .WithOne(x => x.Equipment)
                .HasForeignKey(x => x.EquipmentId);
        }

        //Tables
        #region Tables
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<TheoreticalMMParams> TheoreticalMMParams => Set<TheoreticalMMParams>();
        public DbSet<Model> Models => Set<Model>();
        public DbSet<Equipment> Equipments => Set<Equipment>();
        public DbSet<MM> MMs => Set<MM>();
        public DbSet<MMCoefficient> MMCoefficients => Set<MMCoefficient>();
        public DbSet<Regime> Regimes => Set<Regime>();
        public DbSet<ControlAction> ControlActions => Set<ControlAction>();
        public DbSet<ExperimentalData> ExperimentalDatas => Set<ExperimentalData>();
        public DbSet<Qualities> Qualities => Set<Qualities>();
        #endregion
    }
}