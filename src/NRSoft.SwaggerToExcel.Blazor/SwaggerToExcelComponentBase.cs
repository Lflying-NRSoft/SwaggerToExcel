using NRSoft.SwaggerToExcel.Localization;
using Volo.Abp.AspNetCore.Components;

namespace NRSoft.SwaggerToExcel.Blazor;

public abstract class SwaggerToExcelComponentBase : AbpComponentBase
{
    protected SwaggerToExcelComponentBase()
    {
        LocalizationResource = typeof(SwaggerToExcelResource);
    }
}
