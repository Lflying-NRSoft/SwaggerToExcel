using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NRSoft.SwaggerToExcel.Data;

/* This is used if database provider does't define
 * ISwaggerToExcelDbSchemaMigrator implementation.
 */
public class NullSwaggerToExcelDbSchemaMigrator : ISwaggerToExcelDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
