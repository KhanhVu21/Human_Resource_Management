namespace HR.Application.Queries.Queries;

public static class OnLeaveSqlQueries
{
    public const string QueryInsertOnLeave = @"INSERT INTO [dbo].[OnLeave]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName)";

    public const string QueryUpdateOnLeave = @"UPDATE [dbo].[OnLeave] SET 
                                        Description = @Description,
                                        IdEmployee = @IdEmployee,
                                        IdUnit = @IdUnit,
                                        UnitName = @UnitName
                                        WHERE Id = @Id";

    public const string QueryUpdateStatusOnLeave = @"UPDATE [dbo].[OnLeave] SET 
                                 Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteOnLeave = "DELETE FROM [dbo].[OnLeave] WHERE Id IN @Ids";
    public const string QueryGetByIdOnLeave = "select * from [dbo].[OnLeave] where Id = @Id";
    public const string QueryGetOnLeaveByIds = "select * from [dbo].[OnLeave] where Id IN @Ids";
    public const string QueryGetAllOnLeave = "select *from [dbo].[OnLeave] order by CreatedDate desc";

    public const string QueryInsertOnLeaveDeleted = @"INSERT INTO [dbo].[Deleted_OnLeave]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName)";
}