﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirlineIS.Diagram
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ArsContext : DbContext
    {
        public ArsContext()
            : base("name=ArsContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Passenger> Passengers { get; set; }
        public virtual DbSet<Plane> Planes { get; set; }
        public virtual DbSet<PlaneSeat> PlaneSeats { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
    }
}
