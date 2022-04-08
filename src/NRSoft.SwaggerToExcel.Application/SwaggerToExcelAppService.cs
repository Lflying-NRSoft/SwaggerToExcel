using System;
using System.Collections.Generic;
using System.Text;
using NRSoft.SwaggerToExcel.Localization;
using Volo.Abp.Application.Services;

namespace NRSoft.SwaggerToExcel;

/* Inherit your application services from this class.
 */
public abstract class SwaggerToExcelAppService : ApplicationService
{
    protected SwaggerToExcelAppService()
    {
        LocalizationResource = typeof(SwaggerToExcelResource);
    }
}
