using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace NRSoft.SwaggerToExcel;

[Dependency(ReplaceServices = true)]
public class SwaggerToExcelBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SwaggerToExcel";
}
