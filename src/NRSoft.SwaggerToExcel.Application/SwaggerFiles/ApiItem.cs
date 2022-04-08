using Magicodes.ExporterAndImporter.Core;

namespace NRSoft.SwaggerToExcel.SwaggerFiles;

/// <summary>
/// 请求接口的项，是一个汇总信息用于添加到列表中
/// </summary>
public class ApiItem
{
    [ExporterHeader(DisplayName = "序号", AutoCenterColumn = true)]
    public int No { get; set; }

    [ExporterHeader(DisplayName = "接口名字")]
    public string Path { get; set; }

    [ExporterHeader(DisplayName = "方式", AutoCenterColumn = true)]
    public string HttpMethod { get; set; }

    [ExporterHeader(DisplayName = "说明")]
    public string Summary { get; set; }
}
