﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Milestone2.FirstApplication.Models;

public partial class CarOwnership
{
    public int OwnershipId { get; set; }

    public int OwnerId { get; set; }

    public string Vin { get; set; }

    public DateTime PurchaseDate { get; set; }

    public DateTime? SaleDate { get; set; }

    public virtual Owner Owner { get; set; }

    public virtual Car VinNavigation { get; set; }
}