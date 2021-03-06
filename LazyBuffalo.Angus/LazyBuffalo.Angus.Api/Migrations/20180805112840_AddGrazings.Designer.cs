﻿// <auto-generated />
using System;
using LazyBuffalo.Angus.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LazyBuffalo.Angus.Api.Migrations
{
    [DbContext(typeof(AngusDbContext))]
    [Migration("20180805112840_AddGrazings")]
    partial class AddGrazings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.Coordinate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GrazingId");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("Id");

                    b.HasIndex("GrazingId");

                    b.ToTable("Coordinate");
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.Cow", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HardwareSerial");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Cows");
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.GpsEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CowId");

                    b.Property<DateTime>("DateTime");

                    b.Property<double>("LatitudeDeg");

                    b.Property<string>("LatitudeDirection")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<double>("LatitudeMinutes");

                    b.Property<double>("LatitudeMinutesDecimals");

                    b.Property<double>("LongitudeDeg");

                    b.Property<string>("LongitudeDirection")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<double>("LongitudeMinutes");

                    b.Property<double>("LongitudeMinutesDecimals");

                    b.HasKey("Id");

                    b.HasIndex("CowId");

                    b.ToTable("GpsEntries");
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.Grazing", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Grazings");
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.PositionEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CowId");

                    b.Property<DateTime>("DateTime");

                    b.Property<bool>("IsUp");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.Property<int>("Z");

                    b.HasKey("Id");

                    b.HasIndex("CowId");

                    b.ToTable("PositionEntry");
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.TemperatureEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CowId");

                    b.Property<DateTime>("DateTime");

                    b.Property<float>("Temperature");

                    b.HasKey("Id");

                    b.HasIndex("CowId");

                    b.ToTable("TemperatureEntries");
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.Coordinate", b =>
                {
                    b.HasOne("LazyBuffalo.Angus.Api.Models.Grazing", "Grazing")
                        .WithMany("Coordinates")
                        .HasForeignKey("GrazingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.GpsEntry", b =>
                {
                    b.HasOne("LazyBuffalo.Angus.Api.Models.Cow", "Cow")
                        .WithMany("GpsEntries")
                        .HasForeignKey("CowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.PositionEntry", b =>
                {
                    b.HasOne("LazyBuffalo.Angus.Api.Models.Cow", "Cow")
                        .WithMany("PositionEntries")
                        .HasForeignKey("CowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LazyBuffalo.Angus.Api.Models.TemperatureEntry", b =>
                {
                    b.HasOne("LazyBuffalo.Angus.Api.Models.Cow", "Cow")
                        .WithMany("TemperatureEntries")
                        .HasForeignKey("CowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
