using System.ComponentModel.DataAnnotations;

namespace NRSoft.SwaggerToExcel.SwaggerFiles;

/// <summary>
/// 获取接口信息的请求参数
/// </summary>
public class GetApiInput
{
    /// <summary>
    /// Swagger文档的地址
    /// </summary>
    [Required]
    public string SwaggerUrl { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [Required]
    public string Url { get; set; }
}