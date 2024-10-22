﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AirlineIS.Server.Models;

public partial class ArsContext : DbContext
{
    public ArsContext(DbContextOptions<ArsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<AirportView> AirportViews { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightView> FlightViews { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Plane> Planes { get; set; }

    public virtual DbSet<PlaneSeat> PlaneSeats { get; set; }

    public virtual DbSet<PlaneView> PlaneViews { get; set; }

    public virtual DbSet<PnrView> PnrViews { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC078E760811");

            entity.Property(e => e.City).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Country).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Iatacode)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LocalCode)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<AirportView>(entity =>
        {
            entity.ToView("AirportView");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC074634FE9F");

            entity.Property(e => e.ClassName)
                .HasDefaultValue("Unnamed")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC071B037DE7");

            entity.HasOne(d => d.Destination).WithMany(p => p.FlightDestinations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightsTo_ToAirports");

            entity.HasOne(d => d.Origin).WithMany(p => p.FlightOrigins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlightsFrom_ToAirports");

            entity.HasOne(d => d.Plane).WithMany(p => p.Flights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flights_ToPlanes");
        });

        modelBuilder.Entity<FlightView>(entity =>
        {
            entity.ToView("FlightView");

            entity.Property(e => e.DestinationCode)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DestinationName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.OriginCode)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OriginName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0767C70E86");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Invoices).HasConstraintName("FK_Invoices_ToTickets");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0725EC3B1F");

            entity.Property(e => e.BirthDate).HasDefaultValue(new DateOnly(1900, 1, 1));
            entity.Property(e => e.Email)
                .HasDefaultValue("email@email.com")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FirstName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LastName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone)
                .HasDefaultValue("88001234567")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SurName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Planes__3214EC07553D95D5");

            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<PlaneSeat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07F4B05E6A");

            entity.Property(e => e.SeatName)
                .HasDefaultValue("Unnamed")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Class).WithMany(p => p.PlaneSeats)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PlaneSeats_ToClasses");

            entity.HasOne(d => d.Plane).WithMany(p => p.PlaneSeats).HasConstraintName("FK_PlaneSeats_ToPlanes");
        });

        modelBuilder.Entity<PlaneView>(entity =>
        {
            entity.ToView("PlaneView");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<PnrView>(entity =>
        {
            entity.ToView("PnrView");

            entity.Property(e => e.ContactInfo).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DestinationCode)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DestinationName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OriginCode)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OriginName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SeatName)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC079243CBE3");

            entity.HasOne(d => d.Flight).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_ToFlights");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Tickets).HasConstraintName("FK_Tickets_ToPassengers");

            entity.HasOne(d => d.PlaneSeat).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_ToPlaneSeats");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}