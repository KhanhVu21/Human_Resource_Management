using HR.Application.Interface;
using HR.Application.Interface.Interfaces;

namespace HR.Application.Implement;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IRoleRepository roleRepository, IUserRepository userRepository,
        IUserRoleRepository userRoleRepository, IUserTypeRepository userTypeRepository, IUnitRepository unitRepository,
        IAllowanceRepository allowanceRepository,
        ICategoryCityRepository categoryCityRepository,
        ICategoryDistrictRepository categoryDistrictRepository,
        ICategoryWardRepository categoryWardRepository,
        IEmployeeAllowanceRepository employeeAllowanceRepository,
        IEmployeeRepository employeeRepository,
        INavigationRepository navigationRepository,
        ICategoryCompensationBenefitsRepository categoryCompensationBenefitsRepository,
        IEmployeeBenefitsRepository employeeBenefitsRepository,
        IEmployeeDayOffRepository employeeDayOffRepository,
        ICategoryLaborEquipmentRepository categoryLaborEquipmentRepository,
        IRequestToHiredRepository requestToHiredRepository,
        ICategoryVacanciesRepository categoryVacanciesRepository,
        ITicketLaborEquipmentDetailRepository ticketLaborEquipmentDetailRepository,
        ITicketLaborEquipmentRepository ticketLaborEquipmentRepository,
        ICategoryPositionRepository categoryPositionRepository,
        ILaborEquipmentUnitRepository laborEquipmentUnitRepository,
        IPositionEmployeeRepository positionEmployeeRepository,
        IPromotionTranferRepository promotionTranferRepository,
        IInternRequestRepository internRequestRepository,
        IOnLeaveRepository onLeaveRepository,
        ITypeDayOffRepository typeDayOffRepository,
        IOvertimeRepository overtimeRepository,
        IResignRepository resignRepository,
        IBusinessTripEmployeeRepository businessTripEmployeeRepository,
        IBusinessTripRepository businessTripRepository,
        IWorkflowTemplateRepository workflowTemplateRepository,
        IWorkFlowRepository workFlowRepository,
        IWorkFlowHistoryRepository workFlowHistoryRepository,
        IWorkflowStepRepository workflowStepRepository,
        IEmployeeTypeRepository employeeTypeRepository)
    {
        Role = roleRepository;
        User = userRepository;
        UserRole = userRoleRepository;
        UserType = userTypeRepository;
        Unit = unitRepository;
        Allowance = allowanceRepository;
        CategoryCity = categoryCityRepository;
        CategoryDistrict = categoryDistrictRepository;
        CategoryWard = categoryWardRepository;
        EmployeeAllowance = employeeAllowanceRepository;
        Employee = employeeRepository;
        EmployeeType = employeeTypeRepository;
        Navigation = navigationRepository;
        EmployeeDayOff = employeeDayOffRepository;
        EmployeeBenefits = employeeBenefitsRepository;
        CategoryVacancies = categoryVacanciesRepository;
        CategoryLaborEquipment = categoryLaborEquipmentRepository;
        CategoryCompensationBenefits = categoryCompensationBenefitsRepository;
        RequestToHired = requestToHiredRepository;
        TicketLaborEquipmentDetail = ticketLaborEquipmentDetailRepository;
        TicketLaborEquipment = ticketLaborEquipmentRepository;
        CategoryPosition = categoryPositionRepository;
        LaborEquipmentUnit = laborEquipmentUnitRepository;
        PositionEmployee = positionEmployeeRepository;
        PromotionTranfer = promotionTranferRepository;
        InternRequest = internRequestRepository;
        Overtime = overtimeRepository;
        TypeDayOff = typeDayOffRepository;
        OnLeave = onLeaveRepository;
        WorkflowTemplate = workflowTemplateRepository;
        Resign = resignRepository;
        BusinessTripEmployee = businessTripEmployeeRepository;
        WorkflowStep = workflowStepRepository;
        WorkFlow = workFlowRepository;
        WorkFlowHistory = workFlowHistoryRepository;
        BusinessTrip = businessTripRepository;
    }

    public IRoleRepository Role { get; }
    public IUserRepository User { get; }
    public IUserRoleRepository UserRole { get; }
    public IUserTypeRepository UserType { get; }
    public IUnitRepository Unit { get; }
    public IAllowanceRepository Allowance { get; }
    public ICategoryCityRepository CategoryCity { get; }
    public ICategoryDistrictRepository CategoryDistrict { get; }
    public ICategoryWardRepository CategoryWard { get; }
    public IEmployeeAllowanceRepository EmployeeAllowance { get; }
    public IEmployeeRepository Employee { get; }
    public IEmployeeTypeRepository EmployeeType { get; }
    public INavigationRepository Navigation { get; }
    public ICategoryCompensationBenefitsRepository CategoryCompensationBenefits { get; }
    public IEmployeeBenefitsRepository EmployeeBenefits { get; }
    public IEmployeeDayOffRepository EmployeeDayOff { get; }
    public ICategoryVacanciesRepository CategoryVacancies { get; }
    public ICategoryLaborEquipmentRepository CategoryLaborEquipment { get; }
    public IRequestToHiredRepository RequestToHired { get; }
    public ITicketLaborEquipmentDetailRepository TicketLaborEquipmentDetail { get; }
    public ITicketLaborEquipmentRepository TicketLaborEquipment { get; }
    public ICategoryPositionRepository CategoryPosition { get; }
    public ILaborEquipmentUnitRepository LaborEquipmentUnit { get; }
    public IPositionEmployeeRepository PositionEmployee { get; }
    public IPromotionTranferRepository PromotionTranfer { get; }
    public IInternRequestRepository InternRequest { get; }
    public IOnLeaveRepository OnLeave { get; }
    public IOvertimeRepository Overtime { get; }
    public ITypeDayOffRepository TypeDayOff { get; }
    public IResignRepository Resign { get; }
    public IBusinessTripRepository BusinessTrip { get; }
    public IBusinessTripEmployeeRepository BusinessTripEmployee { get; }
    public IWorkflowTemplateRepository WorkflowTemplate { get; }
    public IWorkflowStepRepository WorkflowStep { get; }
    public IWorkFlowRepository WorkFlow { get; }
    public IWorkFlowHistoryRepository WorkFlowHistory { get; }
}