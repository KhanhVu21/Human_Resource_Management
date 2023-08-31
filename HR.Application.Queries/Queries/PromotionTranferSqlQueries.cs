namespace HR.Application.Queries.Queries;

public class PromotionTranferSqlQueries
{
    public const string QueryInsertPromotionTranfer = @"INSERT INTO [dbo].[PromotionTranfer]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdUnit]
           ,[IdEmployee]
           ,[UnitName]
           ,[IdPosition]
           ,[PositionName])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdUnit, @IdEmployee, @UnitName,
                              @IdPosition, @PositionName)";

    public const string QueryUpdatePromotionTranfer = @"UPDATE [dbo].[PromotionTranfer] SET 
                                        Status = @Status
                                        WHERE Id = @Id";

    public const string QueryUpdateStatusPromotionTranfer = @"UPDATE [dbo].[PromotionTranfer] SET 
                                        Description = @Description,
                                        IdUnit = @IdUnit,
                                        IdEmployee = @IdEmployee,
                                        UnitName = @UnitName,
                                        IdPosition = @IdPosition,
                                        PositionName = @PositionName
                                        WHERE Id = @Id";

    public const string QueryDeletePromotionTranfer = "DELETE FROM [dbo].[PromotionTranfer] WHERE Id IN @Ids";
    public const string QueryGetByIdPromotionTranfer = "select * from [dbo].[PromotionTranfer] where Id = @Id";
    public const string QueryGetPromotionTranferByIds = "select * from [dbo].[PromotionTranfer] where Id IN @Ids";
    public const string QueryGetAllPromotionTranfer = "select *from [dbo].[PromotionTranfer] order by CreatedDate desc";

    public const string QueryInsertPromotionTranferDeleted = @"INSERT INTO [dbo].[Deleted_PromotionTranfer]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdUnit]
           ,[IdEmployee]
           ,[UnitName]
           ,[IdPosition]
           ,[PositionName])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdUnit, @IdEmployee, @UnitName,
                              @IdPosition, @PositionName)";
}