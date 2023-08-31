namespace HR.Application.Queries.Queries;

public static class WorkflowStepSqlQueries
{
    public const string QueryInsertWorkflowStep = @"INSERT INTO [dbo].[WorkflowStep]
           ([Id]
           ,[TemplateId]
           ,[StepName]
           ,[IdRoleAssign]
           ,[IdUnitAssign]
           ,[AllowTerminated]
           ,[RejectName]
           ,[Order]
           ,[OutCome]
           ,[CreatedDate]
           ,[Status]
           ,[ProcessWorkflowButton]
           ,[StatusColor])
       VALUES (@Id, @TemplateId, @StepName, @IdRoleAssign, @IdUnitAssign, @AllowTerminated, @RejectName, @Order,
    @OutCome, @CreatedDate, @Status, @ProcessWorkflowButton, @StatusColor)";

    public const string QueryUpdateWorkflowStep = @"UPDATE [dbo].[WorkflowStep] SET 
                                        TemplateId = @TemplateId,
                                        StepName = @StepName,
                                        IdRoleAssign = @IdRoleAssign,
                                        IdUnitAssign = @IdUnitAssign,
                                        AllowTerminated = @AllowTerminated,
                                        RejectName = @RejectName,
                                        [Order] = @Order,
                                        ProcessWorkflowButton = @ProcessWorkflowButton,
                                        StatusColor = @StatusColor,
                                        OutCome = @OutCome
                                        WHERE Id = @Id";

    public const string QueryDeleteWorkflowStep = "DELETE FROM [dbo].[WorkflowStep] WHERE Id IN @Ids";
    public const string QueryGetByIdWorkflowStep = "select * from [dbo].[WorkflowStep] where Id = @Id";
    public const string QueryGetWorkflowStepNotInIds = "select * from [dbo].[WorkflowStep] where Id NOT IN @Ids";
    public const string QueryGetWorkflowStepByTemplateId = "select * from [dbo].[WorkflowStep] where TemplateId IN @TemplateId";
    public const string QueryGetWorkflowStepByIds = "select * from [dbo].[WorkflowStep] where Id IN @Ids";
    public const string QueryGetAllWorkflowStep = "select *from [dbo].[WorkflowStep] order by CreatedDate desc";

    public const string QueryInsertWorkflowStepDeleted = @"INSERT INTO [dbo].[Deleted_WorkflowStep]
           ([Id]
           ,[TemplateId]
           ,[StepName]
           ,[IdRoleAssign]
           ,[IdUnitAssign]
           ,[AllowTerminated]
           ,[RejectName]
           ,[Order]
           ,[OutCome]
           ,[CreatedDate]
           ,[Status]
           ,[ProcessWorkflowButton]
           ,[StatusColor])
       VALUES (@Id, @TemplateId, @StepName, @IdRoleAssign, @IdUnitAssign, @AllowTerminated, @RejectName, @Order,
    @OutCome, @CreatedDate, @Status, @ProcessWorkflowButton, @StatusColor)";
}