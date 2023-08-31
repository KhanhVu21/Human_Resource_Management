using HR.Domain.Core.Entities;

namespace HR.Application.Dto.Dto;

public class WorkflowTemplateDto
{
    public Guid Id { get; set; }
    public string? WorkflowName { get; set; }
    public string WorkflowCode { get; set; }
    public int Order { get; set; }
    public string? StartWorkflowButton { get; set; }
    public string? DefaultCompletedStatus { get; set; }
    public string TableName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? Status { get; set; }

    // public List<WorkflowInstancesDto> WorkflowInstancesList { get; set; }
    // public List<WorkflowStepDto> WorkflowSteps { get; set; }
}