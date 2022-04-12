using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;

namespace NRSoft.SwaggerToExcel.SwaggerFiles;

[UnitOfWork(IsDisabled = true)]
public class SwaggerFileAppService : ApplicationService, ISwaggerFileAppService
{
    private readonly IExcelExporter _excelExporter;
    private readonly IExportFileByTemplate _exportFileByTemplate;
    private readonly IHttpClientFactory _httpClientFactory;
    public SwaggerFileAppService(
        IExcelExporter excelExporter,
        IExportFileByTemplate exportFileByTemplate,
        IHttpClientFactory httpClientFactory)
    {
        _excelExporter = excelExporter;
        _exportFileByTemplate = exportFileByTemplate;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<byte[]> GetGroupAsync(GetGroupInput input)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, input.SwaggerUrl);

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var contentJson = await httpResponseMessage.Content.ReadAsStringAsync();
            // 有点坑，特殊字符处理，否则序列化不了
            contentJson = contentJson.Replace("[安全任务]", "");

            var list = await ParseSwaggerGroupAsync(contentJson, input.ModuleName);

            return await _excelExporter.ExportAsByteArray(list);
        }

        return null;
    }

    public async Task<byte[]> GetApiAsync(GetApiInput input, string tplPath)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, input.SwaggerUrl);

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var contentJson = await httpResponseMessage.Content.ReadAsStringAsync();
            // 有点坑，特殊字符处理，否则序列化不了
            contentJson = contentJson.Replace("[安全任务]", "");

            var apiInfo = await ParseSwaggerApiAsync(contentJson, input.Url);

            return await _exportFileByTemplate.ExportBytesByTemplate(apiInfo, tplPath);
        }

        return null;
    }

    /// <summary>
    /// 获取分组里的所有接口列表
    /// </summary>
    /// <param name="jsonData"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    private async Task<List<ApiItem>> ParseSwaggerGroupAsync(string jsonData, string tag)
    {
        var document = await OpenApiDocument.FromJsonAsync(jsonData);
        var list = new List<ApiItem>();

        Logger.LogInformation("总共有{0}个接口", document.Paths.Count);

        var i = 1;
        foreach (var pair in document.Paths)
        {
            foreach (var p in pair.Value.ActualPathItem)
            {
                if (p.Value.Tags.FirstOrDefault() == tag && !p.Value.Summary.Contains("测试"))
                {
                    list.Add(new ApiItem
                    {
                        No = i++,
                        HttpMethod = p.Key.ToUpper(),
                        Path = pair.Key.TrimStart('/'),
                        Summary = p.Value.Summary.Replace("(已完成)", "").Replace("（已完成）", "")
                    });
                }
            }
        }

        return list;
    }

    private async Task<ApiInfo> ParseSwaggerApiAsync(string jsonData, string apiName)
    {
        var document = await OpenApiDocument.FromJsonAsync(jsonData);
        var apiInfo = new ApiInfo { Url = apiName };

        var api = document.Paths.FirstOrDefault(p => p.Key == apiName).Value;
        if (api != null)
        {
            foreach (var p in api.ActualPathItem)
            {
                apiInfo.HttpMethod = p.Key.ToUpper();
                apiInfo.Summary = p.Value.Summary.Replace("(已完成)", "").Replace("（已完成）", "");
                apiInfo.ContentType = p.Value.Produces.FirstOrDefault();

                var method = p.Value;
                var Summary = method.Summary;
                //var ResultDescription = method.ResultDescription;

                // 请求参数
                foreach (var parameter in method.Parameters)
                {
                    var kind = parameter.Kind.ToString();
                    if (parameter.IsAnyType && parameter.Schema is NJsonSchema.JsonSchema)
                    {
                        // 需要转到具体对象中去。
                        var dto = document.Definitions.FirstOrDefault(d => d.Key == parameter.Schema.Reference.Title);
                        var parameterName = dto.Key;
                        var jsonSchema = dto.Value;
                        foreach (var pair in jsonSchema.ActualProperties)
                        {
                            var name = pair.Key;
                            var value = pair.Value;
                            var description = value.Description;
                            var type = value.Type;
                            var format = value.Format;
                            var IsRequired = value.IsRequired;
                            apiInfo.RequestParams.Add(new ApiParameter
                            {
                                Name = name,
                                Summary = description,
                                Kind = kind,
                                IsRequired = IsRequired,
                                Type = type.ToString(),
                            });
                        }
                    }
                    else
                    {
                        var name = parameter.Name;
                        var schema = parameter.Schema; // Json是：NJsonSchema.JsonSchema
                        var description = parameter.Description;
                        var format = parameter.Format;
                        var type = parameter.Type;
                        var IsRequired = parameter.IsRequired;
                        apiInfo.RequestParams.Add(new ApiParameter
                        {
                            Name = name,
                            Summary = description,
                            Kind = kind,
                            IsRequired = IsRequired,
                            Type = type.ToString(),
                        });
                        //var description = parameter.HasDescription ? parameter.Description.Replace("\r", "\t").Replace("\n", "\t") : null;
                        //var VariableName = parameter.VariableName;
                    }
                }

                // 响应结果
                var response = method.Responses[((int)HttpStatusCode.OK).ToString()];
                var properties = response.ActualResponse.Schema.ActualSchema.ActualProperties;
                var dataType = properties.FirstOrDefault(p => p.Key == "data").Value;
                var actualProperties = dataType.ActualSchema.ActualProperties;
                if (!actualProperties.Any())
                {
                    apiInfo.ResponseParams.Add(new ApiParameter
                    {
                        Name = dataType.Name,
                        Summary = "返回值",
                        IsRequired = dataType.IsRequired,
                        Type = dataType.Type.ToString(),
                    });
                }
                // 如果是分页属性，则获取list的属性值
                if (dataType.ActualSchema.ActualProperties.Any(a => a.Key == "pageNum"))
                {
                    dataType = dataType.ActualSchema.ActualProperties.FirstOrDefault(p => p.Key == "list").Value;
                    if (dataType.IsArray)
                    {
                        actualProperties = dataType.Item.ActualSchema.ActualProperties;
                    }
                }

                foreach (var pair in actualProperties)
                {
                    var name = pair.Key;
                    var value = pair.Value;
                    var description = value.Description;
                    var type = value.Type;
                    var format = value.Format;
                    var IsRequired = value.IsRequired;
                    apiInfo.ResponseParams.Add(new ApiParameter
                    {
                        Name = name,
                        Summary = description,
                        IsRequired = IsRequired,
                        Type = type.ToString(),
                    });

                    if (value.Type == NJsonSchema.JsonObjectType.None)
                    {
                        ParseApiParameter(apiInfo.RefDicResponseParams, value.ActualSchema);
                    }
                }
            }
        }

        return apiInfo;

    }

    private void ParseApiParameter(Dictionary<string, List<ApiParameter>> dic, JsonSchema schema)
    {
        var listParams = new List<ApiParameter>();
        foreach (var pair in schema.ActualProperties)
        {
            var name = pair.Key;
            var value = pair.Value;
            var description = value.Description;
            var type = value.Type;
            var format = value.Format;
            var IsRequired = value.IsRequired;
            listParams.Add(new ApiParameter
            {
                Name = name,
                Summary = description,
                IsRequired = IsRequired,
                Type = type.ToString(),
            });

            if (value.Type == NJsonSchema.JsonObjectType.None)
            {
                // 递归往里找
                ParseApiParameter(dic, value.ActualSchema);
            }
        }
        dic.Add(schema.Title, listParams);
    }
}
