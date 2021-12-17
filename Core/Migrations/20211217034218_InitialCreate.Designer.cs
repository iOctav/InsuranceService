﻿// <auto-generated />
using System;
using Core.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(InsuranceContext))]
    [Migration("20211217034218_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Core.Models.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("InsuranceContractId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OperatorId")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerId");

                    b.HasIndex("InsuranceContractId");

                    b.HasIndex("OperatorId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Core.Models.Compensation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ControllerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("InsuranceContractId")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Verdict")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerId");

                    b.HasIndex("ControllerId");

                    b.HasIndex("InsuranceContractId");

                    b.ToTable("Compensations");
                });

            modelBuilder.Entity("Core.Models.Consumer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CredentialId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CredentialId");

                    b.ToTable("Consumers");
                });

            modelBuilder.Entity("Core.Models.Credential", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Credential");
                });

            modelBuilder.Entity("Core.Models.DiseaseHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ControllerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerId");

                    b.HasIndex("ControllerId");

                    b.ToTable("DiseaseHistory");
                });

            modelBuilder.Entity("Core.Models.InsuranceAgent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CredentialId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Tariff")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CredentialId");

                    b.ToTable("InsuranceAgents");
                });

            modelBuilder.Entity("Core.Models.InsuranceContract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("InsuranceAgentId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InsuranceAmount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InsurancePremium")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerId");

                    b.HasIndex("InsuranceAgentId");

                    b.ToTable("InsuranceContracts");
                });

            modelBuilder.Entity("Core.Models.Operator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CredentialId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CredentialId");

                    b.ToTable("Operators");
                });

            modelBuilder.Entity("Core.Models.Application", b =>
                {
                    b.HasOne("Core.Models.Consumer", "Consumer")
                        .WithMany()
                        .HasForeignKey("ConsumerId");

                    b.HasOne("Core.Models.InsuranceContract", "InsuranceContract")
                        .WithMany()
                        .HasForeignKey("InsuranceContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.Operator", "Operator")
                        .WithMany("Applications")
                        .HasForeignKey("OperatorId");

                    b.Navigation("Consumer");

                    b.Navigation("InsuranceContract");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("Core.Models.Compensation", b =>
                {
                    b.HasOne("Core.Models.Consumer", null)
                        .WithMany("Compensations")
                        .HasForeignKey("ConsumerId");

                    b.HasOne("Core.Models.Operator", "Controller")
                        .WithMany("Compensations")
                        .HasForeignKey("ControllerId");

                    b.HasOne("Core.Models.InsuranceContract", "InsuranceContract")
                        .WithMany("Compensations")
                        .HasForeignKey("InsuranceContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Controller");

                    b.Navigation("InsuranceContract");
                });

            modelBuilder.Entity("Core.Models.Consumer", b =>
                {
                    b.HasOne("Core.Models.Credential", "Credential")
                        .WithMany()
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credential");
                });

            modelBuilder.Entity("Core.Models.DiseaseHistory", b =>
                {
                    b.HasOne("Core.Models.Consumer", "Consumer")
                        .WithMany("DiseaseHistories")
                        .HasForeignKey("ConsumerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.Operator", "Controller")
                        .WithMany("DiseaseHistories")
                        .HasForeignKey("ControllerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");

                    b.Navigation("Controller");
                });

            modelBuilder.Entity("Core.Models.InsuranceAgent", b =>
                {
                    b.HasOne("Core.Models.Credential", "Credential")
                        .WithMany()
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credential");
                });

            modelBuilder.Entity("Core.Models.InsuranceContract", b =>
                {
                    b.HasOne("Core.Models.Consumer", "Consumer")
                        .WithMany("InsuranceContracts")
                        .HasForeignKey("ConsumerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.InsuranceAgent", "InsuranceAgent")
                        .WithMany("InsuranceContracts")
                        .HasForeignKey("InsuranceAgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");

                    b.Navigation("InsuranceAgent");
                });

            modelBuilder.Entity("Core.Models.Operator", b =>
                {
                    b.HasOne("Core.Models.Credential", "Credential")
                        .WithMany()
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credential");
                });

            modelBuilder.Entity("Core.Models.Consumer", b =>
                {
                    b.Navigation("Compensations");

                    b.Navigation("DiseaseHistories");

                    b.Navigation("InsuranceContracts");
                });

            modelBuilder.Entity("Core.Models.InsuranceAgent", b =>
                {
                    b.Navigation("InsuranceContracts");
                });

            modelBuilder.Entity("Core.Models.InsuranceContract", b =>
                {
                    b.Navigation("Compensations");
                });

            modelBuilder.Entity("Core.Models.Operator", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Compensations");

                    b.Navigation("DiseaseHistories");
                });
#pragma warning restore 612, 618
        }
    }
}