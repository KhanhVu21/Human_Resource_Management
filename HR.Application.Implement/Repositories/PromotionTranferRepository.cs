using System.Data;
using Dapper;
using HR.Application.Dto.Dto;
using HR.Application.Interface.Interfaces;
using HR.Application.Queries.Queries;
using HR.Application.Utilities.Utils;
using HR.Domain.Core.Entities;
using HR.Domain.Kernel.Utils;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HR.Application.Implement.Repositories;

public class PromotionTranferRepository : IPromotionTranferRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PromotionTranferRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ PromotionTranferRepository Methods ]==================================

    public async Task<TemplateApi<PromotionTranferDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<PromotionTranfer>(PromotionTranferSqlQueries
                .QueryGetAllPromotionTranfer))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<PromotionTranferDto>()),
            units.Count);
    }

    public async Task<TemplateApi<PromotionTranferDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var unit = await connection.QueryFirstOrDefaultAsync<PromotionTranfer>(
            PromotionTranferSqlQueries.QueryGetByIdPromotionTranfer, new { Id = id });

        var result = unit.Adapt<PromotionTranferDto>();

        return new Pagination().HandleGetByIdRespond(result);
    }

    public Task<TemplateApi<PromotionTranferDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<PromotionTranferDto>> Update(PromotionTranferDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var idsItemId = new List<Guid>() { model.Id };
            var workflowInstances =
                await connection.QueryFirstOrDefaultAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = idsItemId }, tran);

            if (workflowInstances != null && workflowInstances.CurrentStep != 0)
            {
                return new TemplateApi<PromotionTranferDto>(null, null, "Phiếu đã đi vào quy trình không thể chỉnh sửa !",
                    false, 0,
                    0, 0, 0);
            }

            var promotionTranfer = model.Adapt<PromotionTranfer>();
            await connection.ExecuteAsync(PromotionTranferSqlQueries.QueryUpdatePromotionTranfer, promotionTranfer,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng PromotionTranfer",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "PromotionTranfer",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<PromotionTranferDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PromotionTranferDto>> Insert(PromotionTranferDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(PromotionTranferSqlQueries.QueryInsertPromotionTranfer, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng PromotionTranfer",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "PromotionTranfer",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<PromotionTranferDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<PromotionTranferDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var workflowInstances =
                (await connection.QueryAsync<WorkflowInstances>(
                    WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = ids }, tran))
                .ToList();

            var workflowHistories =
                (await connection.QueryAsync<WorkflowHistory>(
                    WorkflowHistorySqlQueries.QueryGetWorkflowHistoriesByIdWorkFlowInstance,
                    new { IdWorkFlowInstance = workflowInstances.Select(e => e.Id) },
                    tran))
                .ToList();

            if (workflowHistories.Count > 1)
            {
                return new TemplateApi<PromotionTranferDto>(null, null, "Phiếu đã đi vào qui trình không thể xóa !",
                    false, 0,
                    0, 0, 0);
            }

            #region WorkflowHistory

            await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryInsertWorkflowHistoryDeleted,
                workflowHistories,
                tran);

            await connection.ExecuteAsync(WorkflowHistorySqlQueries.QueryDeleteWorkflowHistory,
                new { Ids = workflowHistories.Select(e => e.Id) }, tran);

            var diaries = workflowHistories.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng WorkflowHistory",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "WorkflowHistory",
                IsSuccess = true,
                WithId = id.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            #region WorkflowInstances

            await connection.ExecuteAsync(WorkflowInstancesSqlQueries.QueryInsertWorkflowInstancesDeleted,
                workflowInstances,
                tran);

            await connection.ExecuteAsync(WorkflowInstancesSqlQueries.QueryDeleteWorkflowInstances,
                new { Ids = workflowInstances.Select(e => e.Id) }, tran);

            diaries = workflowHistories.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng WorkflowInstances",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "WorkflowInstances",
                IsSuccess = true,
                WithId = id.Id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            #region PromotionTranfer

            var promotionTranfers =
                (await connection.QueryAsync<PromotionTranfer>(PromotionTranferSqlQueries.QueryGetPromotionTranferByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(PromotionTranferSqlQueries.QueryInsertPromotionTranferDeleted,
                promotionTranfers,
                tran);

            await connection.ExecuteAsync(PromotionTranferSqlQueries.QueryDeletePromotionTranfer, new { Ids = ids },
                tran);

            diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng PromotionTranfer",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "PromotionTranfer",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<PromotionTranferDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<PromotionTranferDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<PromotionTranferDto>> GetPromotionTranferAndWorkFlow(Guid idUserCurrent,
        int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in PromotionTranfer table
        var promotionTranfer =
            (await connection.QueryAsync<PromotionTranfer>(PromotionTranferSqlQueries.QueryGetAllPromotionTranfer))
            .ToList().Adapt<List<PromotionTranferDto>>();

        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //checking for the records created by user is requesting if have any data then filtering it by id requesting user  
        //otherwise getting default data by UnitId and RoleId
        var checkHaveAnyByCreateBy =
            promotionTranfer.Exists(e => e.IdUserRequest == idUserCurrent) && !roles.Exists(e => e.IsAdmin);
        if (checkHaveAnyByCreateBy)
        {
            promotionTranfer = promotionTranfer.Where(e => e.IdUserRequest == idUserCurrent).ToList();
        }

        //getting all data if requesting user is admin
        bool isAdminOrUserRequest = roles.Exists(e => e.IsAdmin) || checkHaveAnyByCreateBy;
        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.PromotionTranfer });

        var filteredPromotionTranfer = await new WorkFlowRepository(_configuration).FilterAndSortItemData(connection,
            promotionTranfer,
            idUserCurrent, isAdminOrUserRequest, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredPromotionTranfer,
            filteredPromotionTranfer.Count);
    }

    public async Task<TemplateApi<PromotionTranferDto>> GetPromotionTranferAndWorkFlowByIdUserApproved(
        Guid idUserCurrent,
        int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in PromotionTranfer table
        var promotionTranfer =
            (await connection.QueryAsync<PromotionTranfer>(PromotionTranferSqlQueries.QueryGetAllPromotionTranfer))
            .ToList().Adapt<List<PromotionTranferDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.PromotionTranfer });

        var filteredRequestToHired = await new WorkFlowRepository(_configuration).FilterAndSortItemDataHistories(
            connection,
            promotionTranfer,
            idUserCurrent, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredRequestToHired,
            filteredRequestToHired.Count);
    }

    #endregion
}