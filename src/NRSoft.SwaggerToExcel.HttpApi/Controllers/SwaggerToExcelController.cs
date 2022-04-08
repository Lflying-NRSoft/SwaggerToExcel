using NRSoft.SwaggerToExcel.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NRSoft.SwaggerToExcel.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SwaggerToExcelController : AbpControllerBase
{
    protected SwaggerToExcelController()
    {
        LocalizationResource = typeof(SwaggerToExcelResource);
    }
}
