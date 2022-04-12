using Magicodes.ExporterAndImporter.Core;
using System.Collections.Generic;

namespace NRSoft.SwaggerToExcel.SwaggerFiles;

public class ApiInfo
{
    [ExporterHeader(DisplayName = "接口地址", AutoCenterColumn = true)]
    public string Url { get; set; }

    [ExporterHeader(DisplayName = "请求方式", AutoCenterColumn = true)]
    public string HttpMethod { get; set; }

    [ExporterHeader(DisplayName = "说明")]
    public string Summary { get; set; }

    [ExporterHeader(DisplayName = "返回值类型")]
    public string ContentType { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public List<ApiParameter> RequestParams { get; set; } = new List<ApiParameter>();

    /// <summary>
    /// 响应结果
    /// </summary>
    public List<ApiParameter> ResponseParams { get; set; } = new List<ApiParameter>();

    /// <summary>
    /// 响应结果的关联对象
    /// </summary>
    public Dictionary<string, List<ApiParameter>> RefDicResponseParams { get; set; } = new Dictionary<string, List<ApiParameter>>();
}

public class ApiParameter
{
    [ExporterHeader(DisplayName = "参数名")]
    public string Name { get; set; }

    [ExporterHeader(DisplayName = "参数类型")]
    public string Type { get; set; }

    /// <summary>
    /// 参数类型:是Body还是Query
    /// </summary>
    [ExporterHeader(DisplayName = "参数类型")]
    public string Kind { get; set; }

    [ExporterHeader(DisplayName = "是否必填")]
    [ValueMapping("是", true)]
    [ValueMapping("否", false)]
    public bool IsRequired { get; set; }
    public string IsRequiredString { get { return IsRequired ? "是" : "否"; } }

    [ExporterHeader(DisplayName = "说明")]
    public string Summary { get; set; }
}
