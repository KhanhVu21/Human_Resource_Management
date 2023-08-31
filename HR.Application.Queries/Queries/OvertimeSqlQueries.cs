namespace HR.Application.Queries.Queries;

public static class OvertimeSqlQueries
{
    public const string QueryInsertOvertime = @"INSERT INTO [dbo].[Overtime]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdEmployee]
           ,[IdUnit]
           ,[UnitName])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdEmployee, @IdUnit, @UnitName)";

    public const string QueryUpdateOvertime = @"UPDATE [dbo].[Overtime] SET 
                                        Description = @Description,
                                        IdEmployee = @IdEmployee,
                                        IdUnit = @IdUnit,
                                        UnitName = @UnitName
                                        WHERE Id = @Id";

    public const string QueryUpdateStatusOvertime = @"UPDATE [dbo].[Overtime] SET 
                                 Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteOvertime = "DELETE FROM [dbo].[Overtime] WHERE Id IN @Ids";
    public const string QueryGetByIdOvertime = "select * from [dbo].[Overtime] where Id = @Id";
    public const string QueryGetOvertimeByIds = "select * from [dbo].[Overtime] where Id IN @Ids";
    public const string QueryGetAllOvertime = "select *from [dbo].[Overtime] order by CreatedDate desc";

    public const string QueryInsertOvertimeDeleted = @"INSERT INTO [dbo].[Deleted_Overtime]
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