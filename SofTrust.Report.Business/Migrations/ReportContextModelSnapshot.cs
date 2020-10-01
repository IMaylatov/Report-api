﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SofTrust.Report.Business;

namespace SofTrust.Report.Business.Migrations
{
    [DbContext(typeof(ReportContext))]
    partial class ReportContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SofTrust.Report.Business.Model.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<int?>("TemplateId")
                        .HasColumnName("template_id")
                        .HasColumnType("integer");

                    b.Property<int>("TypeId")
                        .HasColumnName("type_id")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("pk_reports");

                    b.HasIndex("TemplateId")
                        .HasName("ix_reports_template_id");

                    b.HasIndex("TypeId")
                        .HasName("ix_reports_type_id");

                    b.ToTable("reports");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.ReportType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_report_types");

                    b.ToTable("report_types");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Malibu"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ClosedXml"
                        });
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte[]>("Data")
                        .HasColumnName("data")
                        .HasColumnType("bytea");

                    b.HasKey("Id")
                        .HasName("pk_templates");

                    b.ToTable("templates");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Report", b =>
                {
                    b.HasOne("SofTrust.Report.Business.Model.Template", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .HasConstraintName("fk_reports_templates_template_id");

                    b.HasOne("SofTrust.Report.Business.Model.ReportType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .HasConstraintName("fk_reports_report_types_type_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
