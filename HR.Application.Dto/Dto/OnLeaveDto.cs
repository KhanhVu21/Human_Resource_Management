﻿using HR.Domain.Core.Entities;

namespace HR.Application.Dto.Dto;

public class OnLeaveDto
{
    public Guid Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public Guid IdUserRequest { get; set; }
    public Guid IdEmployee { get; set; }
    public Guid IdUnit { get; set; }
    public string? UnitName { get; set; }

    public List<WorkflowInstances>? WorkflowInstances { get; set; }
    public int CountWorkFlowStep { get; set; }
    public WorkflowStep? CurrentWorkFlowStep { get; set; }

}