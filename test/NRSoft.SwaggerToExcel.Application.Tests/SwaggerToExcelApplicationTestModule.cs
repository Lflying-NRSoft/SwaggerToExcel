using Volo.Abp.Modularity;

namespace NRSoft.SwaggerToExcel;

[DependsOn(
    typeof(SwaggerToExcelApplicationModule),
    typeof(SwaggerToExcelDomainTestModule)
    )]
public class SwaggerToExcelApplicationTestModule : AbpModule
{

}
