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

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.DataSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Data")
                        .HasColumnName("data")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_data_sets");

                    b.ToTable("data_sets");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.DataSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Data")
                        .HasColumnName("data")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_data_sources");

                    b.ToTable("data_sources");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_reports");

                    b.ToTable("reports");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.ReportDataSet", b =>
                {
                    b.Property<int>("ReportId")
                        .HasColumnName("report_id")
                        .HasColumnType("integer");

                    b.Property<int>("DataSetId")
                        .HasColumnName("data_set_id")
                        .HasColumnType("integer");

                    b.HasKey("ReportId", "DataSetId")
                        .HasName("pk_report_data_sets");

                    b.HasIndex("DataSetId")
                        .HasName("ix_report_data_sets_data_set_id");

                    b.ToTable("report_data_sets");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.ReportDataSource", b =>
                {
                    b.Property<int>("ReportId")
                        .HasColumnName("report_id")
                        .HasColumnType("integer");

                    b.Property<int>("DataSourceId")
                        .HasColumnName("data_source_id")
                        .HasColumnType("integer");

                    b.HasKey("ReportId", "DataSourceId")
                        .HasName("pk_report_data_sources");

                    b.HasIndex("DataSourceId")
                        .HasName("ix_report_data_sources_data_source_id");

                    b.ToTable("report_data_sources");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.ReportVariable", b =>
                {
                    b.Property<int>("ReportId")
                        .HasColumnName("report_id")
                        .HasColumnType("integer");

                    b.Property<int>("VariableId")
                        .HasColumnName("variable_id")
                        .HasColumnType("integer");

                    b.HasKey("ReportId", "VariableId")
                        .HasName("pk_report_variables");

                    b.HasIndex("VariableId")
                        .HasName("ix_report_variables_variable_id");

                    b.ToTable("report_variables");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte[]>("Data")
                        .HasColumnName("data")
                        .HasColumnType("bytea");

                    b.Property<int>("ReportId")
                        .HasColumnName("report_id")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("pk_templates");

                    b.HasIndex("ReportId")
                        .HasName("ix_templates_report_id");

                    b.ToTable("templates");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.Variable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Data")
                        .HasColumnName("data")
                        .HasColumnType("text");

                    b.Property<string>("Label")
                        .HasColumnName("label")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_variables");

                    b.ToTable("variables");
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.ReportDataSet", b =>
                {
                    b.HasOne("SofTrust.Report.Business.Model.Domain.DataSet", "DataSet")
                        .WithMany("ReportDataSets")
                        .HasForeignKey("DataSetId")
                        .HasConstraintName("fk_report_data_sets_data_sets_data_set_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SofTrust.Report.Business.Model.Domain.Report", "Report")
                        .WithMany("ReportDataSets")
                        .HasForeignKey("ReportId")
                        .HasConstraintName("fk_report_data_sets_reports_report_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.ReportDataSource", b =>
                {
                    b.HasOne("SofTrust.Report.Business.Model.Domain.DataSource", "DataSource")
                        .WithMany("ReportDataSources")
                        .HasForeignKey("DataSourceId")
                        .HasConstraintName("fk_report_data_sources_data_sources_data_source_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SofTrust.Report.Business.Model.Domain.Report", "Report")
                        .WithMany("ReportDataSources")
                        .HasForeignKey("ReportId")
                        .HasConstraintName("fk_report_data_sources_reports_report_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.ReportVariable", b =>
                {
                    b.HasOne("SofTrust.Report.Business.Model.Domain.Report", "Report")
                        .WithMany("ReportVariables")
                        .HasForeignKey("ReportId")
                        .HasConstraintName("fk_report_variables_reports_report_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SofTrust.Report.Business.Model.Domain.Variable", "Variable")
                        .WithMany("ReportVariables")
                        .HasForeignKey("VariableId")
                        .HasConstraintName("fk_report_variables_variables_variable_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SofTrust.Report.Business.Model.Domain.Template", b =>
                {
                    b.HasOne("SofTrust.Report.Business.Model.Domain.Report", "Report")
                        .WithMany("Templates")
                        .HasForeignKey("ReportId")
                        .HasConstraintName("fk_templates_reports_report_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
