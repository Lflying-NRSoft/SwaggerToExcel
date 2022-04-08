using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace NRSoft.SwaggerToExcel.Blazor;

[Dependency(ReplaceServices = true)]
public class SwaggerToExcelBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SwaggerToExcel";
}
