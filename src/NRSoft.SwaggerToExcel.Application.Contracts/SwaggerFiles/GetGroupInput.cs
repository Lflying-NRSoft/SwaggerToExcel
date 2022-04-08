using System.ComponentModel.DataAnnotations;

namespace NRSoft.SwaggerToExcel.SwaggerFiles;

/// <summary>
/// 请求参数
/// </summary>
public class GetGroupInput
{
    /// <summary>
    /// Swagger文档的地址
    /// </summary>
    [Required]
    public string SwaggerUrl { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    [Required]
    public string ModuleName { get; set; }
}
