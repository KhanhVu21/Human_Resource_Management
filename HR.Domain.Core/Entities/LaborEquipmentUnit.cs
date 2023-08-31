﻿namespace HR.Domain.Core.Entities;

public class LaborEquipmentUnit
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid IdEmployee { get; set; }
    public Guid IdTicketLaborEquipment { get; set; }
    public Guid IdCategoryLaborEquipment { get; set; }
    public Guid IdUnit { get; set; }
    public string EquipmentCode { get; set; }
}