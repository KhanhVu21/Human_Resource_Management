using System.Data;
using Dapper;
using HR.Application.Dto.Custom;
using HR.Application.Dto.Dto;
using HR.Application.Interface.Interfaces;
using HR.Application.Queries.Queries;
using HR.Application.Utilities.Utils;
using HR.Domain.Core.Entities;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HR.Application.Implement.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public EmployeeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ EmployeeRepository ]=================================================================

    public async Task<TemplateApi<EmployeeDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var employees = (await connection.QueryAsync<Employee>(EmployeeSqlQueries.QueryGetAllEmployee))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, employees.Select(u => u.Adapt<EmployeeDto>()),
            employees.Count);
    }

    public async Task<TemplateApi<EmployeeDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var employee = await connection.QueryFirstOrDefaultAsync<Employee>(
            EmployeeSqlQueries.QueryGetByIdEmployee, new { Id = id });

        return new Pagination().HandleGetByIdRespond(employee.Adapt<EmployeeDto>());
    }

    public Task<TemplateApi<EmployeeDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<EmployeeDto>> Update(EmployeeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var checkExistEmail =
                await connection.QueryFirstOrDefaultAsync<Employee>(EmployeeSqlQueries.QueryGetByEmail,
                    new { model.Email }, tran);
            if (checkExistEmail != null && checkExistEmail.Id != model.Id)
            {
                return new TemplateApi<EmployeeDto>(null, null, "Email này đã tồn tại !", false, 0, 0, 0, 0);
            }

            var checkExistPhone = await connection.QueryFirstOrDefaultAsync<Employee>(
                EmployeeSqlQueries.QueryGetByPhone,
                new { model.Phone }, tran);
            if (checkExistPhone != null && checkExistPhone.Id != model.Id)
            {
                return new TemplateApi<EmployeeDto>(null, null, "Số điện thoại này đã tồn tại !", false, 0, 0, 0, 0);
            }

            var employeeById = await connection.QueryFirstOrDefaultAsync<Employee>(
                EmployeeSqlQueries.QueryGetByIdEmployee, new { model.Id }, tran);

            var employee = model.Adapt<Employee>();
            if (model.IdFile is not null)
            {
                employee.Avatar = employeeById.Avatar;
            }

            await connection.ExecuteAsync(EmployeeSqlQueries.QueryUpdateEmployee, employee, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng Employee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "Employee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<EmployeeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<EmployeeDto>> Insert(EmployeeDto model, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var checkExistEmail =
                await connection.QueryFirstOrDefaultAsync<Employee>(EmployeeSqlQueries.QueryGetByEmail,
                    new { model.Email }, tran);
            if (checkExistEmail != null)
            {
                return new TemplateApi<EmployeeDto>(null, null, "Email này đã tồn tại !", false, 0, 0, 0, 0);
            }

            var checkExistPhone = await connection.QueryFirstOrDefaultAsync<Employee>(
                EmployeeSqlQueries.QueryGetByPhone,
                new { model.Phone }, tran);
            if (checkExistPhone != null)
            {
                return new TemplateApi<EmployeeDto>(null, null, "Số điện thoại này đã tồn tại !", false, 0, 0, 0, 0);
            }

            await connection.ExecuteAsync(EmployeeSqlQueries.QueryInsertEmployee, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng Employee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "Employee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<EmployeeDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<EmployeeDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var employeeDayOff = await connection.QueryFirstOrDefaultAsync<EmployeeDayOff>(
                EmployeeDayOffSqlQueries.QueryEmployeeDayOffByIdEmployee, new { IdEmployee = ids }, tran);

            if (employeeDayOff != null)
            {
                return new TemplateApi<EmployeeDto>(null, null, "Đã có dữ liệu không thể xóa !", false, 0, 0, 0, 0);
            }

            var employees =
                (await connection.QueryAsync<Employee>(EmployeeSqlQueries.QueryEmployeeByIds, new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(EmployeeSqlQueries.QueryInsertEmployeeDeleted, employees, tran);

            await connection.ExecuteAsync(EmployeeSqlQueries.QueryDeleteEmployee, new { Ids = ids }, tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng Employee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "Employee",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<EmployeeDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<EmployeeDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<EmployeeAndBenefits>> GetEmployeeAndBenefits(Guid idEmployee)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var employee = await connection.QueryFirstOrDefaultAsync<Employee>(
            EmployeeSqlQueries.QueryGetByIdEmployee, new { Id = idEmployee });

        var employeeBenefits =
            (await connection.QueryAsync<EmployeeBenefits>(
                EmployeeBenefitsSqlQueries.QueryEmployeeBenefitsByIdsEmployee,
                new { IdEmployee = idEmployee }))
            .ToList();

        var employeeAndBenefits = new EmployeeAndBenefits()
        {
            Employee = employee,
            EmployeeBenefits = employeeBenefits
        };

        return new Pagination().HandleGetByIdRespond(employeeAndBenefits.Adapt<EmployeeAndBenefits>());
    }

    public async Task<TemplateApi<EmployeeAndAllowance>> GetEmployeeAndAllowance(Guid idEmployee)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var employee = await connection.QueryFirstOrDefaultAsync<Employee>(
            EmployeeSqlQueries.QueryGetByIdEmployee, new { Id = idEmployee });

        var allowances =
            (await connection.QueryAsync<Allowance>(
                AllowanceSqlQueries.QueryGetAllAllowanceByIdEmployee,
                new { IdEmployee = idEmployee }))
            .ToList();

        var employeeAndBenefits = new EmployeeAndAllowance()
        {
            Employee = employee,
            Allowances = allowances
        };

        return new Pagination().HandleGetByIdRespond(employeeAndBenefits.Adapt<EmployeeAndAllowance>());
    }

    #endregion
}