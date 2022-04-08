using NRSoft.SwaggerToExcel.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace NRSoft.SwaggerToExcel.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SwaggerToExcelEntityFrameworkCoreModule),
    typeof(SwaggerToExcelApplicationContractsModule)
    )]
public class SwaggerToExcelDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
