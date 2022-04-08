using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NRSoft.SwaggerToExcel.SwaggerFiles;

/// <summary>
/// Swagger生成Excel文件的应用服务接口
/// </summary>
public interface ISwaggerFileAppService : IApplicationService
{
    /// <summary>
    /// 获取指定分组名的接口列表的文件流字节
    /// </summary>
    /// <param name="input">请求参数</param>
    /// <returns></returns>
    Task<byte[]> GetGroupAsync(GetGroupInput input);

    /// <summary>
    /// 获取指定UrL的接口信息
    /// </summary>
    /// <param name="input">请求参数</param>
    /// <param name="tplPath">Excel模板路径</param>
    /// <returns></returns>
    Task<byte[]> GetApiAsync(GetApiInput input, string tplPath);
}
