using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NRSoft.SwaggerToExcel.SwaggerFiles;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace NRSoft.SwaggerToExcel.Controllers
{
    [AllowAnonymous]
    [RemoteService]
    [Route("api/swagger-file")]
    public class SwaggerFileController : AbpController
    {
        private readonly ISwaggerFileAppService _fileAppService;

        public SwaggerFileController(ISwaggerFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        /// <summary>
        /// 获取指定分组名的接口列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("group")]
        public async Task<ActionResult> GetGroupAsync(GetGroupInput input)
        {
            Logger.LogDebug("【临时文件】FlieService下载的文件名为：{@input}", input);
            var bytes = await _fileAppService.GetGroupAsync(input);
            if (bytes == null)
            {
                Logger.LogInformation("获取SwaggerUI的文档，请求参数为：{@input}", input);
                return NotFound($"获取SwaggerUI的文档（{input.SwaggerUrl})的文件");
            }
            else
            {
                var fileName = Path.GetFileName($"{input.ModuleName}.xlsx".Replace('\\', Path.DirectorySeparatorChar));
                var type = MimeTypes.GetByExtension(Path.GetExtension(fileName));
                Logger.LogDebug("下载文件，文件名为：{0}, contentType:{1}", fileName, type);

                if (type.StartsWith(@"image/"))
                {
                    return File(bytes, type);
                }
                else
                {
                    return File(bytes, type, fileName);
                }
            }
        }

        /// <summary>
        /// 获取指定UrL的接口信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("api")]
        public async Task<ActionResult> GetApiAsync(GetApiInput input)
        {
            //模板路径
            var tplPath = Path.Combine(Directory.GetCurrentDirectory(), "SwaggerApi.xlsx");

            Logger.LogDebug("【临时文件】FlieService下载的文件名为：{@input}", input);
            var bytes = await _fileAppService.GetApiAsync(input, tplPath);
            if (bytes == null)
            {
                Logger.LogInformation("获取SwaggerUI的文档，请求参数为：{@input}", input);
                return NotFound($"获取SwaggerUI的文档（{input.SwaggerUrl})的文件");
            }
            else
            {
                var fileName = input.Url.Trim('/').Replace("/", "_") + ".xlsx";
                var type = MimeTypes.GetByExtension(Path.GetExtension(fileName));
                Logger.LogDebug("下载文件，文件名为：{0}, contentType:{1}", fileName, type);

                if (type.StartsWith(@"image/"))
                {
                    return File(bytes, type);
                }
                else
                {
                    return File(bytes, type, fileName);
                }
            }
        }
    }
}
