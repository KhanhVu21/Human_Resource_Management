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

public class RequestToHiredRepository : IRequestToHiredRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public RequestToHiredRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ RequestToHiredRepository Methods ]==================================================

    public async Task<TemplateApi<RequestToHiredDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var requestToHired =
            (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetAllRequestToHired))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            requestToHired.Select(u => u.Adapt<RequestToHiredDto>()),
            requestToHired.Count);
    }

    public async Task<RequestToHired> GetDataById(Guid idRequestToHire)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var requestToHired = await connection.QueryFirstOrDefaultAsync<RequestToHired>(
            RequestToHiredSqlQueries.QueryGetByIdRequestToHired, new { Id = idRequestToHire });

        return requestToHired;
    }

    public async Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlow(Guid idUserCurrent, int pageNumber,
        int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all data in requestToHired table
        var requestToHired =
            (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetAllRequestToHired))
            .ToList().Adapt<List<RequestToHiredDto>>();

        //get all data by id user in role table
        var roles = (await connection.QueryAsync<Role>(RoleSqlQueries.GetRoleByIdUser, new { IdUser = idUserCurrent }))
            .ToList();

        //checking for the records created by user is requesting if have any data then filtering it by id requesting user  
        //otherwise getting default data by UnitId and RoleId
        var checkHaveAnyByCreateBy =
            requestToHired.Exists(e => e.CreatedBy == idUserCurrent) && !roles.Exists(e => e.IsAdmin);
        if (checkHaveAnyByCreateBy)
        {
            requestToHired = requestToHired.Where(e => e.CreatedBy == idUserCurrent).ToList();
        }

        //getting all data if requesting user is admin
        bool isAdminOrUserRequest = roles.Exists(e => e.IsAdmin) || checkHaveAnyByCreateBy;
        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.RequestToHired });

        var filteredRequestToHired = await new WorkFlowRepository(_configuration).FilterAndSortItemData(connection,
            requestToHired,
            idUserCurrent, isAdminOrUserRequest, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredRequestToHired,
            filteredRequestToHired.Count);
    }

    public async Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlowByIdUserApproved(Guid idUserCurrent,
        int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        //get all requestToHired
        var requestToHired =
            (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetAllRequestToHired))
            .ToList().Adapt<List<RequestToHiredDto>>();

        //getting work flow template information by work flow code
        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryWorkflowTemplateByCode,
            new { WorkflowCode = AppSettings.RequestToHired });

        var filteredRequestToHired = await new WorkFlowRepository(_configuration).FilterAndSortItemDataHistories(
            connection,
            requestToHired,
            idUserCurrent, workflowTemplate.Id);

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize, filteredRequestToHired,
            filteredRequestToHired.Count);
    }

    public async Task<TemplateApi<RequestToHiredDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var requestToHired = await connection.QueryFirstOrDefaultAsync<RequestToHired>(
            RequestToHiredSqlQueries.QueryGetByIdRequestToHired, new { Id = id });

        var categoryVacancies =
            await connection.QueryFirstOrDefaultAsync<CategoryVacancies>(
                CategoryVacanciesSqlQueries.QueryCategoryVacanciesByIdRequestToHire, new { Id = id });

        var idsItemId = new List<Guid>() { id };
        var workflowInstances =
            (await connection.QueryAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByItemId, new { ItemId = idsItemId }))
            .ToList();

        var requestToHiredDto = requestToHired.Adapt<RequestToHiredDto>();
        requestToHiredDto.CategoryVacancies = categoryVacancies;
        requestToHiredDto.WorkflowInstances = workflowInstances;

        return new Pagination().HandleGetByIdRespond(requestToHiredDto);
    }

    public Task<TemplateApi<RequestToHiredDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<RequestToHiredDto>> Update(RequestToHiredDto model, Guid idUserCurrent,
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
                return new TemplateApi<RequestToHiredDto>(null, null, "Phiếu đã đi vào quy trình không thể chỉnh sửa !",
                    false, 0,
                    0, 0, 0);
            }

            var requestToHired = model.Adapt<RequestToHired>();

            var requestToHiredById = await connection.QueryFirstOrDefaultAsync<RequestToHired>(
                RequestToHiredSqlQueries.QueryGetByIdRequestToHired, new { model.Id }, tran);

            if (model.IdFile is not null)
            {
                requestToHired.FilePath = requestToHiredById.FilePath;
            }

            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryUpdateRequestToHired, requestToHired, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng RequestToHired",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "RequestToHired",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<RequestToHiredDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<RequestToHiredDto>> Insert(RequestToHiredDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var user = await connection.QueryFirstOrDefaultAsync<User>(UserSqlQueries.UserById,
                new { Id = idUserCurrent }, tran);
            model.IdUnit = user.UnitId;

            #region RequestToHired

            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryInsertRequestToHired, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng RequestToHired",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "RequestToHired",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<RequestToHiredDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<RequestToHiredDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName)
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
                return new TemplateApi<RequestToHiredDto>(null, null, "Phiếu đã đi vào qui trình không thể xóa !",
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

            #region RequestToHired

            var requestToHired =
                (await connection.QueryAsync<RequestToHired>(RequestToHiredSqlQueries.QueryGetRequestToHiredByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryInsertRequestToHiredDeleted, requestToHired,
                tran);

            await connection.ExecuteAsync(RequestToHiredSqlQueries.QueryDeleteRequestToHired, new { Ids = ids }, tran);

            diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng RequestToHired",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "RequestToHired",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            #endregion

            tran.Commit();
            return new TemplateApi<RequestToHiredDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<RequestToHiredDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}