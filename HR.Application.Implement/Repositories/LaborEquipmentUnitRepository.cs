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

public class LaborEquipmentUnitRepository : ILaborEquipmentUnitRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public LaborEquipmentUnitRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ LaborEquipmentUnitRepository Methods ]==================================================

    public async Task<TemplateApi<LaborEquipmentUnitDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<LaborEquipmentUnit>(LaborEquipmentUnitSqlQueries
                .QueryGetAllLaborEquipmentUnit))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<LaborEquipmentUnitDto>()),
            units.Count);
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<LaborEquipmentUnit>(
            LaborEquipmentUnitSqlQueries.QueryGetByIdLaborEquipmentUnit, new { Id = id });

        return new Pagination().HandleGetByIdRespond(unit.Adapt<LaborEquipmentUnitDto>());
    }

    public Task<TemplateApi<LaborEquipmentUnitDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> Update(LaborEquipmentUnitDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var laborEquipmentUnit = model.Adapt<LaborEquipmentUnit>();
            await connection.ExecuteAsync(LaborEquipmentUnitSqlQueries.QueryUpdateLaborEquipmentUnit,
                laborEquipmentUnit, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng LaborEquipmentUnit",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "LaborEquipmentUnit",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<LaborEquipmentUnitDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<LaborEquipmentUnitDto>> Insert(LaborEquipmentUnitDto model, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var categoryCities =
                (await connection.QueryAsync<LaborEquipmentUnit>(
                    LaborEquipmentUnitSqlQueries.QueryGetLaborEquipmentUnitByIds, new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(LaborEquipmentUnitSqlQueries.QueryInsertLaborEquipmentUnitDeleted,
                categoryCities, tran);

            await connection.ExecuteAsync(LaborEquipmentUnitSqlQueries.QueryDeleteLaborEquipmentUnit, new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng LaborEquipmentUnit",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "LaborEquipmentUnit",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<LaborEquipmentUnitDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<LaborEquipmentUnitDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByUnit(Guid idUnit, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<LaborEquipmentUnit>(LaborEquipmentUnitSqlQueries
                .QueryGetAllLaborEquipmentUnit))
            .ToList();

        units = units.Where(e => e.IdUnit == idUnit).ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<LaborEquipmentUnitDto>()),
            units.Count);
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByIdTicketLaborEquipment(
        Guid idTicketLaborEquipment, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<TicketLaborEquipmentDetail>(TicketLaborEquipmentDetailSqlQueries
                    .QueryTicketLaborEquipmentDetailByIdTicketLaborEquipment,
                new { IdTicketLaborEquipment = idTicketLaborEquipment }))
            .ToList();

        var laborEquipmentUnits = new List<LaborEquipmentUnit>();

        if (units.Any())
        {
            var listEquipmentCode = units.Select(e => e.EquipmentCode).ToList();

            laborEquipmentUnits = (await connection.QueryAsync<LaborEquipmentUnit>(LaborEquipmentUnitSqlQueries
                    .QueryGetAllLaborEquipmentUnit))
                .ToList();

            laborEquipmentUnits = laborEquipmentUnits.Where(e => listEquipmentCode.Contains(e.EquipmentCode)).ToList();
        }

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            laborEquipmentUnits.Select(u => u.Adapt<LaborEquipmentUnitDto>()),
            laborEquipmentUnits.Count);
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> InsertLaborEquipmentUnitTypeInsert(
        Guid idTicketLaborEquipment,
        Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            #region LaborEquipmentUnit

            var ticketLaborEquipments = await connection.QueryFirstOrDefaultAsync<TicketLaborEquipment>(
                TicketLaborEquipmentSqlQueries.QueryGetByIdTicketLaborEquipment, new { Id = idTicketLaborEquipment },
                tran);

            var ticketLaborEquipmentDetails = (await connection.QueryAsync<TicketLaborEquipmentDetail>(
                    TicketLaborEquipmentDetailSqlQueries
                        .QueryTicketLaborEquipmentDetailByIdTicketLaborEquipment,
                    new { IdTicketLaborEquipment = idTicketLaborEquipment }, tran))
                .ToList();

            var laborEquipmentUnits = new List<LaborEquipmentUnit>();

            foreach (var item in ticketLaborEquipmentDetails)
            {
                for (var i = 0; i < item.Quantity; i++)
                {
                    laborEquipmentUnits.Add(new LaborEquipmentUnit()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Status = 0,
                        EquipmentCode = i + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
                        IdTicketLaborEquipment = idTicketLaborEquipment,
                        IdUnit = ticketLaborEquipments.IdUnit,
                        IdEmployee = item.IdEmployee,
                        IdCategoryLaborEquipment = item.IdCategoryLaborEquipment
                    });
                }
            }

            await connection.ExecuteAsync(LaborEquipmentUnitSqlQueries.QueryInsertLaborEquipmentUnit,
                laborEquipmentUnits,
                tran);

            var diaries = laborEquipmentUnits.Select(item => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng LaborEquipmentUnit",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "LaborEquipmentUnit",
                IsSuccess = true,
                WithId = item.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<LaborEquipmentUnitDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> UpdateStatusLaborEquipmentUnit(Guid idLaborEquipmentUnit,
        int status, Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(LaborEquipmentUnitSqlQueries.QueryUpdateStatusLaborEquipmentUnit,
                new { Status = status, Id = idLaborEquipmentUnit }, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng LaborEquipmentUnit",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "LaborEquipmentUnit",
                IsSuccess = true,
                WithId = idLaborEquipmentUnit
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<LaborEquipmentUnitDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByUnitAndEmployee(Guid idUnit,
        Guid idEmployee, int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<LaborEquipmentUnit>(LaborEquipmentUnitSqlQueries
                .QueryGetAllLaborEquipmentUnitByIdUnitAndIdEmployee, new { IdEmployee = idEmployee, IdUnit = idUnit }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<LaborEquipmentUnitDto>()),
            units.Count);
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByStatus(int status, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<LaborEquipmentUnit>(LaborEquipmentUnitSqlQueries
                .QueryGetAllLaborEquipmentUnitByStatus, new { Status = status }))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<LaborEquipmentUnitDto>()),
            units.Count);
    }

    public async Task<TemplateApi<LaborEquipmentUnitDto>> UpdateLaborEquipmentUnitByListIdAndStatus(Guid id, int status,
        Guid idUserCurrent, string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(LaborEquipmentUnitSqlQueries.QueryUpdateStatusLaborEquipmentUnit,
                new { Status = status, Id = id }, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng LaborEquipmentUnit",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "LaborEquipmentUnit",
                IsSuccess = true,
                WithId = id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<LaborEquipmentUnitDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    #endregion
}