﻿using IotFlow.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IotFlow.DataAccess
{
    public class ServerContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ServerContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceMethod> DevicesMethods { get; set; }
        public DbSet<DeviceMethodParameter> DeviceMethodParameters { get; set; }
        public DbSet<MethodRegister> MethodRegister { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("SQLProviderConnectionString"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>()
                .HasAlternateKey(d => d.DeviceGuid);

            modelBuilder.Entity<DeviceMethod>()
                .Property(p => p.MethodType)
                .HasConversion<string>();

            modelBuilder.Entity<DeviceMethodParameter>()
                .Property(p => p.ParameterType)
                .HasConversion<string>();

            modelBuilder.Entity<MethodRegister>()
                .HasAlternateKey(d => d.CorrelationId);

            modelBuilder.Entity<MethodRegister>()
                .HasOne(mr => mr.Method)
                .WithMany()
                .HasForeignKey(mr => mr.MethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
