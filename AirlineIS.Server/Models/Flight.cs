﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AirlineIS.Server.Models;

public partial class Flight
{
    [Key]
    public int Id { get; set; }

    public int OriginId { get; set; }

    public int DestinationId { get; set; }

    public int PlaneId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DepartureTime { get; set; }

    public TimeOnly Duration { get; set; }

    [ForeignKey("DestinationId")]
    [InverseProperty("FlightDestinations")]
    [JsonIgnore]
    public virtual Airport Destination { get; set; }

    [ForeignKey("OriginId")]
    [InverseProperty("FlightOrigins")]
    [JsonIgnore]
    public virtual Airport Origin { get; set; }

    [ForeignKey("PlaneId")]
    [InverseProperty("Flights")]
    [JsonIgnore]
    public virtual Plane Plane { get; set; }

    [InverseProperty("Flight")]
    [JsonIgnore]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}