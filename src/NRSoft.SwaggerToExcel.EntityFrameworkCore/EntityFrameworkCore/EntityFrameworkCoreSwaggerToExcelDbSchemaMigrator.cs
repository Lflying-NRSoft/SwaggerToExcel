using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NRSoft.SwaggerToExcel.Data;
using Volo.Abp.DependencyInjection;

namespace NRSoft.SwaggerToExcel.EntityFrameworkCore;

public class EntityFrameworkCoreSwaggerToExcelDbSchemaMigrator
    : ISwaggerToExcelDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSwaggerToExcelDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the SwaggerToExcelDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SwaggerToExcelDbContext>()
            .Database
            .MigrateAsync();
    }
}
