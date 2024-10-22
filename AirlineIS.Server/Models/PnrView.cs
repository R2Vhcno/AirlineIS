﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirlineIS.Server.Models;

[Keyless]
public partial class PnrView
{
    public int Id { get; set; }

    [Required]
    [StringLength(92)]
    public string Name { get; set; }

    public DateOnly? BirthDate { get; set; }

    [Required]
    [StringLength(30)]
    public string ContactInfo { get; set; }

    [StringLength(3)]
    public string SeatName { get; set; }

    [StringLength(3)]
    public string OriginCode { get; set; }

    [StringLength(93)]
    public string OriginName { get; set; }

    [StringLength(3)]
    public string DestinationCode { get; set; }

    [StringLength(93)]
    public string DestinationName { get; set; }

    [Column("Departure Time", TypeName = "datetime")]
    public DateTime DepartureTime { get; set; }
}