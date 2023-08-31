using HR.Application.Implement.Repositories;
using HR.Application.Interface;
using HR.Application.Interface.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HR.Application.Implement;

public static class ServiceCollectionExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitRepository, UnitRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        services.AddTransient<IUserTypeRepository, UserTypeRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IAllowanceRepository, AllowanceRepository>();
        services.AddTransient<ICategoryCityRepository, CategoryCityRepository>();
        services.AddTransient<ICategoryDistrictRepository, CategoryDistrictRepository>();
        services.AddTransient<ICategoryWardRepository, CategoryWardRepository>();
        services.AddTransient<IEmployeeAllowanceRepository, EmployeeAllowanceRepository>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IEmployeeTypeRepository, EmployeeTypeRepository>();
        services.AddTransient<INavigationRepository, NavigationRepository>();
        services.AddTransient<ICategoryCompensationBenefitsRepository, CategoryCompensationBenefitsRepository>();
        services.AddTransient<IEmployeeBenefitsRepository, EmployeeBenefitsRepository>();
        services.AddTransient<IEmployeeDayOffRepository, EmployeeDayOffRepository>();
        services.AddTransient<ICategoryVacanciesRepository, CategoryVacanciesRepository>();
        services.AddTransient<ICategoryLaborEquipmentRepository, CategoryLaborEquipmentRepository>();
        services.AddTransient<IRequestToHiredRepository, RequestToHiredRepository>();
        services.AddTransient<ITicketLaborEquipmentDetailRepository, TicketLaborEquipmentDetailRepository>();
        services.AddTransient<ITicketLaborEquipmentRepository, TicketLaborEquipmentRepository>();

        services.AddTransient<ICategoryPositionRepository, CategoryPositionRepository>();
        services.AddTransient<ILaborEquipmentUnitRepository, LaborEquipmentUnitRepository>();
        services.AddTransient<IPositionEmployeeRepository, PositionEmployeeRepository>();
        services.AddTransient<IPromotionTranferRepository, PromotionTranferRepository>();
        services.AddTransient<IInternRequestRepository, InternRequestRepository>();
        services.AddTransient<IOnLeaveRepository, OnLeaveRepository>();
        services.AddTransient<IOvertimeRepository, OvertimeRepository>();
        services.AddTransient<ITypeDayOffRepository, TypeDayOffRepository>();
        services.AddTransient<IResignRepository, ResignRepository>();
        services.AddTransient<IBusinessTripRepository, BusinessTripRepository>();
        services.AddTransient<IBusinessTripEmployeeRepository, BusinessTripEmployeeRepository>();
        services.AddTransient<IWorkflowTemplateRepository, WorkflowTemplateRepository>();
        services.AddTransient<IWorkflowStepRepository, WorkflowStepRepository>();
        services.AddTransient<IWorkFlowRepository, WorkFlowRepository>();
        services.AddTransient<IWorkFlowHistoryRepository, WorkFlowHistoryRepository>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}