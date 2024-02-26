using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
    public class Context : DbContext
    {
        private string? _connectionString;

        //Creation
        public Context(DbContextOptions<Context> options) : base(options)
        {
            
        }

        public Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_connectionString != null)
            {
                optionsBuilder.UseSqlite(_connectionString);
            }
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

            modelBuilder.Entity<Role>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<Script>()
                .HasOne(x => x.Instructor)
                .WithMany(x => x.ScriptsInstructor)
                .HasForeignKey(x => x.InstructorId);

            modelBuilder.Entity<Script>()
                .HasOne(x => x.Trainee)
                .WithMany(x => x.ScriptsTrainee)
                .HasForeignKey(x => x.TraineeId);

            modelBuilder.Entity<Script>()
                .HasOne(x => x.Task)
                .WithOne(x => x.Script)
                .HasForeignKey<Models.Task>(x => x.ScriptId);
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
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<EmergencySituation> EmergencySituations => Set<EmergencySituation>();
        public DbSet<Script> Scripts => Set<Script>();
        public DbSet<Models.Task> Tasks => Set<Models.Task>();
        #endregion
    }
}