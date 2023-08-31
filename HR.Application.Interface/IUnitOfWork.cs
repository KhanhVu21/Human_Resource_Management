using HR.Application.Interface.Interfaces;

namespace HR.Application.Interface;

public interface IUnitOfWork
{
    IRoleRepository Role { get; }
    IUserRepository User { get; }
    IUserRoleRepository UserRole { get; }
    IUserTypeRepository UserType { get; }
    IUnitRepository Unit { get; }
    IAllowanceRepository Allowance { get; }
    ICategoryCityRepository CategoryCity { get; }
    ICategoryDistrictRepository CategoryDistrict { get; }
    ICategoryWardRepository CategoryWard { get; }
    IEmployeeAllowanceRepository EmployeeAllowance { get; }
    IEmployeeRepository Employee { get; }
    IEmployeeTypeRepository EmployeeType { get; }
    INavigationRepository Navigation { get; }
    ICategoryCompensationBenefitsRepository CategoryCompensationBenefits { get; }
    IEmployeeBenefitsRepository EmployeeBenefits { get; }
    IEmployeeDayOffRepository EmployeeDayOff { get; }
    ICategoryVacanciesRepository CategoryVacancies { get; }
    ICategoryLaborEquipmentRepository CategoryLaborEquipment { get; }
    IRequestToHiredRepository RequestToHired { get; }
    ITicketLaborEquipmentDetailRepository TicketLaborEquipmentDetail { get; }
    ITicketLaborEquipmentRepository TicketLaborEquipment { get; }
    ICategoryPositionRepository CategoryPosition { get; }
    ILaborEquipmentUnitRepository LaborEquipmentUnit { get; }
    IPositionEmployeeRepository PositionEmployee { get; }
    IPromotionTranferRepository PromotionTranfer { get; }
    IInternRequestRepository InternRequest { get; }
    IOnLeaveRepository OnLeave { get; }
    IOvertimeRepository Overtime { get; }
    ITypeDayOffRepository TypeDayOff { get; }
    IResignRepository Resign { get; }
    IBusinessTripRepository BusinessTrip { get; }
    IBusinessTripEmployeeRepository BusinessTripEmployee { get; }
    IWorkflowTemplateRepository WorkflowTemplate { get; }
    IWorkflowStepRepository WorkflowStep { get; }
    IWorkFlowRepository WorkFlow { get; }
    IWorkFlowHistoryRepository WorkFlowHistory { get; }
}