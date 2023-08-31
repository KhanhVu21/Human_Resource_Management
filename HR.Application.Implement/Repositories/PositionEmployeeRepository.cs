using System.Data;
using Dapper;
using HR.Application.Dto.Dto;
using HR.Application.Interface.Interfaces;
using HR.Application.Queries.Queries;
using HR.Application.Utilities.Utils;
using HR.Domain.Core.Entities;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HR.Application.Implement.Repositories;

public class PositionEmployeeRepository : IPositionEmployeeRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PositionEmployeeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ PositionEmployeeRepository Methods ]==================================================

    public async Task<TemplateApi<PositionEmployeeDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<PositionEmployee>(PositionEmployeeSqlQueries
                .QueryGetAllPositionEmployee))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<PositionEmployeeDto>()),
            units.Count);
    }

    public async Task<TemplateApi<PositionEmployeeDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<PositionEmployee>(
            PositionEmployeeSqlQueries.QueryGetByIdPositionEmployee, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<PositionEmployeeDto>());
    }

    public Task<TemplateApi<PositionEmployeeDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<PositionEmployeeDto>> Update(PositionEmployeeDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var PositionEmployee = model.Adapt<PositionEmployee>();
            await connection.ExecuteAsync(PositionEmployeeSqlQueries.QueryUpdatePositionEmployee, PositionEmployee,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng PositionEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "PositionEmployee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<PositionEmployeeDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PositionEmployeeDto>> Insert(PositionEmployeeDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(PositionEmployeeSqlQueries.QueryInsertPositionEmployee, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng PositionEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "PositionEmployee",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<PositionEmployeeDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PositionEmployeeDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<PositionEmployee>(PositionEmployeeSqlQueries.QueryGetPositionEmployeeByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(PositionEmployeeSqlQueries.QueryInsertPositionEmployeeDeleted, categoryCities,
                tran);

            await connection.ExecuteAsync(PositionEmployeeSqlQueries.QueryDeletePositionEmployee, new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng PositionEmployee",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "PositionEmployee",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<PositionEmployeeDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<PositionEmployeeDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}