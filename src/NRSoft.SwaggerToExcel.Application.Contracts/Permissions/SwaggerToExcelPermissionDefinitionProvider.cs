using NRSoft.SwaggerToExcel.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NRSoft.SwaggerToExcel.Permissions;

public class SwaggerToExcelPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SwaggerToExcelPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(SwaggerToExcelPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SwaggerToExcelResource>(name);
    }
}
