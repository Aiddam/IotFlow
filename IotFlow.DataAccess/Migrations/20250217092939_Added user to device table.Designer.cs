﻿// <auto-generated />
using System;
using IotFlow.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IotFlow.DataAccess.Migrations
{
    [DbContext(typeof(ServerContext))]
    [Migration("20250217092939_Added user to device table")]
    partial class Addedusertodevicetable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IotFlow.Models.DB.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("DeviceGuid")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAlive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastSeen")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasAlternateKey("DeviceGuid");

                    b.HasIndex("UserId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("IotFlow.Models.DB.DeviceMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DeviceId")
                        .HasColumnType("integer");

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("DevicesMethods");
                });

            modelBuilder.Entity("IotFlow.Models.DB.DeviceMethodParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DefaultValue")
                        .HasColumnType("text");

                    b.Property<int>("DeviceMethodId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("ParameterName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ParameterType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DeviceMethodId");

                    b.ToTable("DeviceMethodParameters");
                });

            modelBuilder.Entity("IotFlow.Models.DB.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("IotFlow.Models.DB.Device", b =>
                {
                    b.HasOne("IotFlow.Models.DB.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("IotFlow.Models.DB.DeviceMethod", b =>
                {
                    b.HasOne("IotFlow.Models.DB.Device", "Device")
                        .WithMany("Methods")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("IotFlow.Models.DB.DeviceMethodParameter", b =>
                {
                    b.HasOne("IotFlow.Models.DB.DeviceMethod", "DeviceMethod")
                        .WithMany("Parameters")
                        .HasForeignKey("DeviceMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceMethod");
                });

            modelBuilder.Entity("IotFlow.Models.DB.Device", b =>
                {
                    b.Navigation("Methods");
                });

            modelBuilder.Entity("IotFlow.Models.DB.DeviceMethod", b =>
                {
                    b.Navigation("Parameters");
                });
#pragma warning restore 612, 618
        }
    }
}
