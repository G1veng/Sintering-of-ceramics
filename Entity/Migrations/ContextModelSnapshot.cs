﻿// <auto-generated />
using System;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entity.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("EmergencySituationTask", b =>
                {
                    b.Property<int>("EmergencySituationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TasksId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EmergencySituationId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("EmergencySituationTask");
                });

            modelBuilder.Entity("Entity.Models.ControlAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Unit")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ControlActions");
                });

            modelBuilder.Entity("Entity.Models.EmergencySituation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EmergencySituations");
                });

            modelBuilder.Entity("Entity.Models.EmpiricalModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Formula")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("TypeId");

                    b.ToTable("EmpiricalModels");
                });

            modelBuilder.Entity("Entity.Models.EmpiricalModelCoeff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("EmpiricalModelId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("EmpiricalModelId");

                    b.ToTable("EmpiricalModelCoeffs");
                });

            modelBuilder.Entity("Entity.Models.EmpiricalModelType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UnitAlias")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EmpiricalModelTypes");
                });

            modelBuilder.Entity("Entity.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("ChargeWightLoad")
                        .HasColumnType("REAL");

                    b.Property<string>("EquipmentBrand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EquipmentType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("HeaterType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("Entity.Models.ExperimentalData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("FirstActionValue")
                        .HasColumnType("REAL");

                    b.Property<int>("FirstControlActionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QualitiesId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("QualitiesValue")
                        .HasColumnType("REAL");

                    b.Property<double>("SecondActionValue")
                        .HasColumnType("REAL");

                    b.Property<int>("SecondControlActionId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ThirdActionValue")
                        .HasColumnType("REAL");

                    b.Property<int>("ThirdControlActionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FirstControlActionId");

                    b.HasIndex("QualitiesId");

                    b.HasIndex("SecondControlActionId");

                    b.HasIndex("ThirdControlActionId");

                    b.ToTable("ExperimentalDatas");
                });

            modelBuilder.Entity("Entity.Models.MM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Fisher")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("MMs");
                });

            modelBuilder.Entity("Entity.Models.MMCoefficient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MMId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("MMId");

                    b.ToTable("MMCoefficients");
                });

            modelBuilder.Entity("Entity.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("AvarageGrainSize")
                        .HasColumnType("REAL");

                    b.Property<double>("CompactMaterialDensity")
                        .HasColumnType("REAL");

                    b.Property<double>("CompactMaterialViscosity")
                        .HasColumnType("REAL");

                    b.Property<int>("MaterialType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Porosity")
                        .HasColumnType("REAL");

                    b.Property<double>("SpecificSurfaceEnergy")
                        .HasColumnType("REAL");

                    b.Property<double>("SurfaceLayerThickness")
                        .HasColumnType("REAL");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Entity.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MMId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QualityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("MMId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("QualityId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("Entity.Models.ParamRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("MaxValue")
                        .HasColumnType("REAL");

                    b.Property<double>("MinValue")
                        .HasColumnType("REAL");

                    b.Property<int?>("ModelId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Step")
                        .HasColumnType("REAL");

                    b.Property<int>("UnitId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.HasIndex("UnitId");

                    b.ToTable("ParamsRanges");
                });

            modelBuilder.Entity("Entity.Models.ParamRangeUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LetterAlias")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ParamsRangesUnits");
                });

            modelBuilder.Entity("Entity.Models.Qualities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Unit")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Qualities");
                });

            modelBuilder.Entity("Entity.Models.Regime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("MaxCuringTime")
                        .HasColumnType("REAL");

                    b.Property<double>("MaxFinalTempretare")
                        .HasColumnType("REAL");

                    b.Property<double>("MaxGasPressure")
                        .HasColumnType("REAL");

                    b.Property<double>("MaxSinteringTime")
                        .HasColumnType("REAL");

                    b.Property<double>("MinCuringTime")
                        .HasColumnType("REAL");

                    b.Property<double>("MinFinalTempretare")
                        .HasColumnType("REAL");

                    b.Property<double>("MinGasPressure")
                        .HasColumnType("REAL");

                    b.Property<double>("MinSinteringTime")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId")
                        .IsUnique();

                    b.ToTable("Regimes");
                });

            modelBuilder.Entity("Entity.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Entity.Models.Script", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("InstructorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Protocol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TaskId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TraineeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("InstructorId");

                    b.HasIndex("TraineeId");

                    b.ToTable("Scripts");
                });

            modelBuilder.Entity("Entity.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmergencySituationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OvenTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QualityId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RegimeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScriptId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("OvenTypeId");

                    b.HasIndex("QualityId");

                    b.HasIndex("RegimeId");

                    b.HasIndex("ScriptId")
                        .IsUnique();

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Entity.Models.TheoreticalMMParams", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("GrainBoundaryDiffusionActivationEnergy")
                        .HasColumnType("REAL");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("PreExponentialFactorOfGraindBoundaryDiffusionCoefficient")
                        .HasColumnType("REAL");

                    b.Property<double>("PreExponentialFactorOfSurfaceSelfCoefficient")
                        .HasColumnType("REAL");

                    b.Property<double>("SurfaceSelfDiffusionActivationEnergy")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId")
                        .IsUnique();

                    b.ToTable("TheoreticalMMParams");
                });

            modelBuilder.Entity("Entity.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EmergencySituationTask", b =>
                {
                    b.HasOne("Entity.Models.EmergencySituation", null)
                        .WithMany()
                        .HasForeignKey("EmergencySituationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entity.Models.EmpiricalModel", b =>
                {
                    b.HasOne("Entity.Models.Equipment", "Equipment")
                        .WithMany("EmpiricalModels")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Material", "Material")
                        .WithMany("EmpiricalModels")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.EmpiricalModelType", "Type")
                        .WithMany("EmpiricalModels")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Material");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Entity.Models.EmpiricalModelCoeff", b =>
                {
                    b.HasOne("Entity.Models.EmpiricalModel", "EmpiricalModel")
                        .WithMany("EmpiricalModelCoeffs")
                        .HasForeignKey("EmpiricalModelId");

                    b.Navigation("EmpiricalModel");
                });

            modelBuilder.Entity("Entity.Models.ExperimentalData", b =>
                {
                    b.HasOne("Entity.Models.ControlAction", "FirstControlAction")
                        .WithMany("FirstExperimentalDatas")
                        .HasForeignKey("FirstControlActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Qualities", "Qualities")
                        .WithMany("ExperimentalDatas")
                        .HasForeignKey("QualitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.ControlAction", "SecondControlAction")
                        .WithMany("SecondExperimentalDatas")
                        .HasForeignKey("SecondControlActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.ControlAction", "ThirdControlAction")
                        .WithMany("ThirdExperimentalDatas")
                        .HasForeignKey("ThirdControlActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FirstControlAction");

                    b.Navigation("Qualities");

                    b.Navigation("SecondControlAction");

                    b.Navigation("ThirdControlAction");
                });

            modelBuilder.Entity("Entity.Models.MMCoefficient", b =>
                {
                    b.HasOne("Entity.Models.MM", "MM")
                        .WithMany("MMCoefficients")
                        .HasForeignKey("MMId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MM");
                });

            modelBuilder.Entity("Entity.Models.Model", b =>
                {
                    b.HasOne("Entity.Models.Equipment", "Equipment")
                        .WithMany("Models")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.MM", "MM")
                        .WithMany("Models")
                        .HasForeignKey("MMId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Material", "Material")
                        .WithMany("Models")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Qualities", "Qualities")
                        .WithMany("Models")
                        .HasForeignKey("QualityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("MM");

                    b.Navigation("Material");

                    b.Navigation("Qualities");
                });

            modelBuilder.Entity("Entity.Models.ParamRange", b =>
                {
                    b.HasOne("Entity.Models.EmpiricalModel", "Model")
                        .WithMany("ParamsRanges")
                        .HasForeignKey("ModelId");

                    b.HasOne("Entity.Models.ParamRangeUnit", "Unit")
                        .WithMany("ParamRanges")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Entity.Models.Regime", b =>
                {
                    b.HasOne("Entity.Models.Equipment", "Equipment")
                        .WithOne("Regime")
                        .HasForeignKey("Entity.Models.Regime", "EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("Entity.Models.Script", b =>
                {
                    b.HasOne("Entity.Models.User", "Instructor")
                        .WithMany("ScriptsInstructor")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.User", "Trainee")
                        .WithMany("ScriptsTrainee")
                        .HasForeignKey("TraineeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instructor");

                    b.Navigation("Trainee");
                });

            modelBuilder.Entity("Entity.Models.Task", b =>
                {
                    b.HasOne("Entity.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Equipment", "OvenType")
                        .WithMany()
                        .HasForeignKey("OvenTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Qualities", "Quality")
                        .WithMany()
                        .HasForeignKey("QualityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Regime", "Regime")
                        .WithMany()
                        .HasForeignKey("RegimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Models.Script", "Script")
                        .WithOne("Task")
                        .HasForeignKey("Entity.Models.Task", "ScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("OvenType");

                    b.Navigation("Quality");

                    b.Navigation("Regime");

                    b.Navigation("Script");
                });

            modelBuilder.Entity("Entity.Models.TheoreticalMMParams", b =>
                {
                    b.HasOne("Entity.Models.Material", "Material")
                        .WithOne("TheoreticalMMParam")
                        .HasForeignKey("Entity.Models.TheoreticalMMParams", "MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");
                });

            modelBuilder.Entity("Entity.Models.User", b =>
                {
                    b.HasOne("Entity.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Entity.Models.ControlAction", b =>
                {
                    b.Navigation("FirstExperimentalDatas");

                    b.Navigation("SecondExperimentalDatas");

                    b.Navigation("ThirdExperimentalDatas");
                });

            modelBuilder.Entity("Entity.Models.EmpiricalModel", b =>
                {
                    b.Navigation("EmpiricalModelCoeffs");

                    b.Navigation("ParamsRanges");
                });

            modelBuilder.Entity("Entity.Models.EmpiricalModelType", b =>
                {
                    b.Navigation("EmpiricalModels");
                });

            modelBuilder.Entity("Entity.Models.Equipment", b =>
                {
                    b.Navigation("EmpiricalModels");

                    b.Navigation("Models");

                    b.Navigation("Regime")
                        .IsRequired();
                });

            modelBuilder.Entity("Entity.Models.MM", b =>
                {
                    b.Navigation("MMCoefficients");

                    b.Navigation("Models");
                });

            modelBuilder.Entity("Entity.Models.Material", b =>
                {
                    b.Navigation("EmpiricalModels");

                    b.Navigation("Models");

                    b.Navigation("TheoreticalMMParam")
                        .IsRequired();
                });

            modelBuilder.Entity("Entity.Models.ParamRangeUnit", b =>
                {
                    b.Navigation("ParamRanges");
                });

            modelBuilder.Entity("Entity.Models.Qualities", b =>
                {
                    b.Navigation("ExperimentalDatas");

                    b.Navigation("Models");
                });

            modelBuilder.Entity("Entity.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Entity.Models.Script", b =>
                {
                    b.Navigation("Task")
                        .IsRequired();
                });

            modelBuilder.Entity("Entity.Models.User", b =>
                {
                    b.Navigation("ScriptsInstructor");

                    b.Navigation("ScriptsTrainee");
                });
#pragma warning restore 612, 618
        }
    }
}
