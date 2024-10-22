﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AirlineIS.Server.Models;

public partial class PlaneSeat
{
    [Key]
    public int Id { get; set; }

    public int PlaneId { get; set; }

    [Required]
    [StringLength(15)]
    public string SeatName { get; set; }

    public int? ClassId { get; set; }

    [Column(TypeName = "money")]
    public decimal Fare { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("PlaneSeats")]
    public virtual Class Class { get; set; }

    [ForeignKey("PlaneId")]
    [InverseProperty("PlaneSeats")]
    [JsonIgnore]
    public virtual Plane Plane { get; set; }

    [InverseProperty("PlaneSeat")]
    [JsonIgnore]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}