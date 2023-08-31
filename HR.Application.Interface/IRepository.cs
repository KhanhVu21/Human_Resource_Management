using HR.Application.Utilities.Utils;

namespace HR.Application.Interface;

public interface IRepository<T> where T : class
{
    Task<TemplateApi<T>> GetAllAsync(int pageNumber, int pageSize);
    Task<TemplateApi<T>> GetById(Guid id);
    Task<TemplateApi<T>> GetAllAvailable(int pageNumber, int pageSize);
    Task<TemplateApi<T>> Update(T model, Guid idUserCurrent, string fullName);
    Task<TemplateApi<T>> Insert(T model, Guid idUserCurrent, string fullName);
    Task<TemplateApi<T>> RemoveByList(List<Guid> ids, Guid idUserCurrent, string fullName);
    Task<TemplateApi<T>> HideByList(List<Guid> ids, bool isLock, Guid idUserCurrent, string fullName);
}