﻿namespace HR.Application.Queries.Queries;

public static class EmployeeSqlQueries
{
    public const string QueryInsertEmployee = @"INSERT INTO [dbo].[Employee]
                           ([Id]
                           ,[Code]
                           ,[Name]
                           ,[Sex]
                           ,[Birthday]
                           ,[Phone]
                           ,[Email]
                           ,[Address]
                           ,[IdCity]
                           ,[IdDistrict]
                           ,[IdWard]
                           ,[TaxNumber]
                           ,[AccountNumber]
                           ,[Note]
                           ,[Avatar]
                           ,[IdUnit]
                           ,[Status]
                           ,[CreatedDate]
                           ,[SalaryBase]
                           ,[SocialInsurancePercent]
                           ,[TaxPercent]
                           ,[JobGrade]
                           ,[TypeOfEmployee])
           VALUES (@Id, @Code, @Name, @Sex, @Birthday, @Phone, @Email, @Address, @IdCity, @IdDistrict, @IdWard, @TaxNumber,
                      @AccountNumber, @Note, @Avatar, @IdUnit, @Status, @CreatedDate, @SalaryBase, @SocialInsurancePercent,
                      @TaxPercent, @JobGrade, @TypeOfEmployee)";

    public const string QueryUpdateEmployee = @"UPDATE [dbo].[Employee] SET
                                        Name = @Name,
                                        Sex = @Sex,
                                        Birthday = @Birthday,
                                        Phone = @Phone,
                                        Email = @Email,
                                        Address = @Address,
                                        IdCity = @IdCity,
                                        IdDistrict = @IdDistrict,
                                        IdWard = @IdWard,
                                        TaxNumber = @TaxNumber,
                                        AccountNumber = @AccountNumber,
                                        Note = @Note,
                                        Avatar = @Avatar,
                                        TypeOfEmployee = @TypeOfEmployee,
                                        SalaryBase = @SalaryBase,
                                        SocialInsurancePercent = @SocialInsurancePercent,
                                        TaxPercent = @TaxPercent,
                                        IdUnit = @IdUnit,
                                        JobGrade = @JobGrade
                                        WHERE Id = @Id";

    public const string QueryDeleteEmployee = "DELETE FROM [dbo].[Employee] WHERE Id IN @Ids";
    public const string QueryGetByIdEmployee = "select * from [dbo].[Employee] where Id = @Id";
    public const string QueryEmployeeByTypeOfEmployee = "select * from [dbo].[Employee] where TypeOfEmployee IN @TypeOfEmployee";
    public const string QueryEmployeeByIds = "select * from [dbo].[Employee] where Id IN @Ids";
    public const string QueryGetAllEmployee = "select *from [dbo].[Employee] order by CreatedDate desc";

    public const string QueryGetByPhone = "select * from [dbo].[Employee] where Phone = @Phone";
    public const string QueryGetByEmail = "select * from [dbo].[Employee] where Email = @Email";


    public const string QueryInsertEmployeeDeleted = @"INSERT INTO [dbo].[Deleted_Employee]
                               ([Id]
                               ,[Code]
                               ,[Name]
                               ,[Sex]
                               ,[Birthday]
                               ,[Phone]
                               ,[Email]
                               ,[Address]
                               ,[IdCity]
                               ,[IdDistrict]
                               ,[IdWard]
                               ,[TaxNumber]
                               ,[AccountNumber]
                               ,[Note]
                               ,[Avatar]
                               ,[IdUnit]
                               ,[Status]
                               ,[CreatedDate]
                               ,[SalaryBase]
                               ,[SocialInsurancePercent]
                               ,[TaxPercent]
                               ,[JobGrade]
                               ,[TypeOfEmployee])
           VALUES (@Id, @Code, @Name, @Sex, @Birthday, @Phone, @Email, @Address, @IdCity, @IdDistrict, @IdWard, @TaxNumber,
                      @AccountNumber, @Note, @Avatar, @IdUnit, @Status, @CreatedDate, @SalaryBase, @SocialInsurancePercent,
                      @TaxPercent, @JobGrade, @TypeOfEmployee)";
}