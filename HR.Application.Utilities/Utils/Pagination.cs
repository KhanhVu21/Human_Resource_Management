namespace HR.Application.Utilities.Utils;

public class Pagination
{
    public TemplateApi<T> HandleGetAllRespond<T>(int pageNumber, int pageSize, IEnumerable<T> lstObject,
        int countRecord)
    {
        var enumerable = lstObject as T[] ?? lstObject.ToArray();
        if (!enumerable.Any())
        {
            return new TemplateApi<T>(
                default,
                Array.Empty<T>(),
                "Không tìm thấy dữ liệu !",
                false,
                0,
                0,
                0,
                0);
        }

        if (pageNumber != 0 && pageSize != 0)
        {
            if (pageNumber < 0)
            {
                pageNumber = 1;
            }

            lstObject = enumerable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        var numPageSize = pageSize == 0 ? 1 : pageSize;

        return new TemplateApi<T>(
            default,
            lstObject?.ToArray(),
            "Lấy danh sách thành công !!!",
            true,
            pageNumber,
            pageSize,
            countRecord,
            countRecord / numPageSize);
    }

    public TemplateApi<T> HandleGetByIdRespond<T>(T lstObject)
    {
        if (lstObject is null)
        {
            return new TemplateApi<T>(
                lstObject,
                null,
                "Không tìm thấy dữ liệu !",
                false,
                0,
                0,
                0,
                0);
        }

        return new TemplateApi<T>(
            lstObject,
            null,
            "Lấy thông tin thành công !",
            true,
            0,
            0,
            1,
            0);
    }
}