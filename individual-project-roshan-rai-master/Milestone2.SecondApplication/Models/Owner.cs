﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Milestone2.SecondApplication.Models;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string OwnerName { get; set; }

    public virtual ICollection<CarOwnership> CarOwnerships { get; set; } = new List<CarOwnership>();

    public override string ToString()
    {
        return $"{OwnerName}";

    }
}