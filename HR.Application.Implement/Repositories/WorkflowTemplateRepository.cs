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

public class WorkflowTemplateRepository : IWorkflowTemplateRepository
{
    #region ===[ Private Members ]=============================================================

    private readonly IConfiguration _configuration;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkflowTemplateRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ===[ WorkflowTemplateRepository Methods ]==================================================

    public async Task<TemplateApi<WorkflowTemplateDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var units = (await connection.QueryAsync<WorkflowTemplate>(WorkflowTemplateSqlQueries
                .QueryGetAllWorkflowTemplate))
            .ToList();

        return new Pagination().HandleGetAllRespond(pageNumber, pageSize,
            units.Select(u => u.Adapt<WorkflowTemplateDto>()),
            units.Count);
    }

    public async Task<TemplateApi<WorkflowTemplateDto>> GetById(Guid id)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();

        var workflowTemplate = await connection.QueryFirstOrDefaultAsync<WorkflowTemplate>(
            WorkflowTemplateSqlQueries.QueryGetByIdWorkflowTemplate, new { Id = id });

        // var workflowStep = await connection.QueryAsync<WorkflowStep>(
        //     WorkflowStepSqlQueries.QueryGetWorkflowStepByTemplateId, new { TemplateId = id });
        //
        // var workflowInstances = await connection.QueryAsync<WorkflowInstances>(
        //     WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByTemplateId, new { TemplateId = id });
        //
        // var workflowHistories = (await connection.QueryAsync<WorkflowHistory>(
        //     WorkflowHistorySqlQueries.QueryGetAllWorkflowHistory)).ToList().Adapt<List<WorkflowHistoryDto>>();
        //
        // var listWorkFlowInstance = new List<WorkflowInstancesDto>();
        // foreach (var item in workflowInstances)
        // {
        //     var workflowInstancesDto = item.Adapt<WorkflowInstancesDto>();
        //     if (workflowHistories.Any())
        //     {
        //         workflowInstancesDto.WorkflowHistories =
        //             workflowHistories.Where(e => e.IdWorkFlowInstance == item.Id).ToList();
        //     }
        //
        //     listWorkFlowInstance.Add(workflowInstancesDto);
        // }

        var result = workflowTemplate.Adapt<WorkflowTemplateDto>();
        // result.WorkflowSteps = workflowStep.Adapt<List<WorkflowStepDto>>();
        // result.WorkflowInstancesList = listWorkFlowInstance;

        return new Pagination().HandleGetByIdRespond(result);
    }

    public Task<TemplateApi<WorkflowTemplateDto>> GetAllAvailable(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TemplateApi<WorkflowTemplateDto>> Update(WorkflowTemplateDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var workflowTemplate = model.Adapt<WorkflowTemplate>();
            await connection.ExecuteAsync(WorkflowTemplateSqlQueries.QueryUpdateWorkflowTemplate, workflowTemplate,
                tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã cập nhật bảng WorkflowTemplate",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Cập nhật CSDL",
                Operation = "Update",
                Table = "WorkflowTemplate",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();

            return new TemplateApi<WorkflowTemplateDto>(null, null, "Cập nhật thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<WorkflowTemplateDto>> Insert(WorkflowTemplateDto model, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(WorkflowTemplateSqlQueries.QueryInsertWorkflowTemplate, model, tran);

            var diary = new Diary()
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã thêm mới bảng WorkflowTemplate",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Thêm mới CSDL",
                Operation = "Create",
                Table = "WorkflowTemplate",
                IsSuccess = true,
                WithId = model.Id
            };

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diary, tran);

            tran.Commit();
            return new TemplateApi<WorkflowTemplateDto>(null, null, "Thêm mới thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public async Task<TemplateApi<WorkflowTemplateDto>> RemoveByList(List<Guid> ids, Guid idUserCurrent,
        string fullName)
    {
        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        connection.Open();
        using var tran = connection.BeginTransaction();

        try
        {
            var workflowStep = await connection.QueryFirstOrDefaultAsync<WorkflowStep>(
                WorkflowStepSqlQueries.QueryGetWorkflowStepByTemplateId, new { TemplateId = ids }, tran);

            var workflowInstances = await connection.QueryFirstOrDefaultAsync<WorkflowInstances>(
                WorkflowInstancesSqlQueries.QueryGetWorkflowInstancesByTemplateId, new { TemplateId = ids }, tran);

            if (workflowStep != null || workflowInstances != null)
            {
                return new TemplateApi<WorkflowTemplateDto>(null, null, "Đã có dữ liệu không thể xóa !", false, 0, 0, 0,
                    0);
            }

            var workflowTemplates =
                (await connection.QueryAsync<WorkflowTemplate>(WorkflowTemplateSqlQueries.QueryGetWorkflowTemplateByIds,
                    new { Ids = ids },
                    tran))
                .ToList();

            await connection.ExecuteAsync(WorkflowTemplateSqlQueries.QueryInsertWorkflowTemplateDeleted,
                workflowTemplates,
                tran);

            await connection.ExecuteAsync(WorkflowTemplateSqlQueries.QueryDeleteWorkflowTemplate, new { Ids = ids },
                tran);

            var diaries = ids.Select(id => new Diary
            {
                Id = Guid.NewGuid(),
                Content = $"{fullName} đã xóa từ bảng WorkflowTemplate",
                UserId = idUserCurrent,
                UserName = fullName,
                DateCreate = DateTime.Now,
                Title = "Xóa từ CSDL",
                Operation = "Delete",
                Table = "WorkflowTemplate",
                IsSuccess = true,
                WithId = id
            }).ToList();

            await connection.ExecuteAsync(DiarySqlQueries.QuerySaveDiary, diaries, tran);

            tran.Commit();
            return new TemplateApi<WorkflowTemplateDto>(null, null, "Xóa thành công !", true, 0, 0, 0, 0);
        }
        catch (Exception)
        {
            // roll the transaction back
            tran.Rollback();
            throw;
        }
    }

    public Task<TemplateApi<WorkflowTemplateDto>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent,
        string fullName)
    {
        throw new NotImplementedException();
    }

    #endregion
}