using NRSoft.SwaggerToExcel.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NRSoft.SwaggerToExcel;

[DependsOn(
    typeof(SwaggerToExcelEntityFrameworkCoreTestModule)
    )]
public class SwaggerToExcelDomainTestModule : AbpModule
{

}
