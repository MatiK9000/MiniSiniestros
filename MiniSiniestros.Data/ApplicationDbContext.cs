using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Data
{
    using Microsoft.EntityFrameworkCore;
    using MiniSiniestros.Entities;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Siniestro> Siniestros => Set<Siniestro>();

        public DbSet<Empleador> Empleadores => Set<Empleador>();

        public DbSet<Trabajador> Trabajadores => Set<Trabajador>();

        public DbSet<PrestadorMedico> PrestadoresMedicos => Set<PrestadorMedico>();

        public DbSet<SiniestroPrestador> SiniestrosPrestadores => Set<SiniestroPrestador>();

        public DbSet<HistorialEstadoSiniestro> HistorialEstadosSiniestros => Set<HistorialEstadoSiniestro>();

        public DbSet<NotificacionSrt> NotificacionesSrt => Set<NotificacionSrt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Empledor
            modelBuilder.Entity<Empleador>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Cuit)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.RazonSocial)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            //Trabajador
            modelBuilder.Entity<Trabajador>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Cuil)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(t => t.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(t => t.Apellido)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            //PrestadorMedico
            modelBuilder.Entity<PrestadorMedico>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            //Siniestro
            modelBuilder.Entity<Siniestro>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.NumeroSiniestro)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(s => s.FechaAlta)
                    .IsRequired();

                entity.Property(s => s.Estado)
                    .IsRequired();

                entity.HasOne(s => s.Empleador)
                    .WithMany(e => e.Siniestros)
                    .HasForeignKey(s => s.EmpleadorId);

                entity.HasOne(s => s.Trabajador)
                    .WithMany(t => t.Siniestros)
                    .HasForeignKey(s => s.TrabajadorId);
            });

            //SiniestroPrestador
            modelBuilder.Entity<SiniestroPrestador>(entity =>
            {
                entity.HasKey(sp => new
                {
                    sp.SiniestroId,
                    sp.PrestadorMedicoId
                });

                entity.Property(sp => sp.FechaAsignacion)
                    .IsRequired();

                entity.HasOne(sp => sp.Siniestro)
                    .WithMany(s => s.Prestadores)
                    .HasForeignKey(sp => sp.SiniestroId);

                entity.HasOne(sp => sp.PrestadorMedico)
                    .WithMany(p => p.Siniestros)
                    .HasForeignKey(sp => sp.PrestadorMedicoId);
            });

            //HistorialEstadoSiniestro
            modelBuilder.Entity<HistorialEstadoSiniestro>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(h => h.EstadoAnterior)
                    .IsRequired();

                entity.Property(h => h.EstadoNuevo)
                    .IsRequired();

                entity.Property(h => h.FechaCambio)
                    .IsRequired();

                entity.HasOne(h => h.Siniestro)
                    .WithMany(s => s.HistorialEstados)
                    .HasForeignKey(h => h.SiniestroId);
            });

            //NotificacionSrt
            modelBuilder.Entity<NotificacionSrt>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.FechaEnvio)
                    .IsRequired();

                entity.Property(n => n.Exitosa)
                    .IsRequired();

                entity.Property(n => n.MensajeError)
                    .HasMaxLength(500);

                entity.HasOne(n => n.Siniestro)
                    .WithMany(s => s.NotificacionesSrt)
                    .HasForeignKey(n => n.SiniestroId);
            });

        }
    }
}
