﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirlineIS.Server.Models;

[Keyless]
public partial class AirportView
{
    public int Id { get; set; }

    [StringLength(3)]
    public string Code { get; set; }

    [Required]
    [StringLength(94)]
    public string Name { get; set; }
}