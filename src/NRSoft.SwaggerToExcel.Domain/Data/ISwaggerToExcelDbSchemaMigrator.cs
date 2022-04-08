using System.Threading.Tasks;

namespace NRSoft.SwaggerToExcel.Data;

public interface ISwaggerToExcelDbSchemaMigrator
{
    Task MigrateAsync();
}
