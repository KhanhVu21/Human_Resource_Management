﻿namespace HR.Application.Queries.Queries;

public class BusinessTripSqlQueries
{
    public const string QueryInsertBusinessTrip = @"INSERT INTO [dbo].[BusinessTrip]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdUnit]
           ,[UnitName]
           ,[StartDate]
           ,[EndDate])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdUnit, @UnitName, @StartDate, @EndDate)";

    public const string QueryUpdateBusinessTrip = @"UPDATE [dbo].[BusinessTrip] SET 
                                        Description = @Description,
                                        IdUnit = @IdUnit,
                                        StartDate = @StartDate,
                                        EndDate = @EndDate,
                                        UnitName = @UnitName
                                        WHERE Id = @Id";

    public const string QueryUpdateStatusBusinessTrip = @"UPDATE [dbo].[BusinessTrip] SET 
                                 Status = @Status
                                        WHERE Id = @Id";

    public const string QueryDeleteBusinessTrip = "DELETE FROM [dbo].[BusinessTrip] WHERE Id IN @Ids";
    public const string QueryGetByIdBusinessTrip = "select * from [dbo].[BusinessTrip] where Id = @Id";
    public const string QueryGetBusinessTripByIds = "select * from [dbo].[BusinessTrip] where Id IN @Ids";
    public const string QueryGetAllBusinessTrip = "select *from [dbo].[BusinessTrip] order by CreatedDate desc";

    public const string QueryInsertBusinessTripDeleted = @"INSERT INTO [dbo].[Deleted_BusinessTrip]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[Description]
           ,[IdUserRequest]
           ,[IdUnit]
           ,[UnitName]
           ,[StartDate]
           ,[EndDate])
                         VALUES (@Id, @CreatedDate, @Status, @Description, @IdUserRequest, @IdUnit, @UnitName, @StartDate, @EndDate)";
}