using Microsoft.AspNetCore.Authorization;

namespace SeedStore.Support.Admin.PermissionsHandling.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionCode { get; }

        public PermissionRequirement(string permissionCode)
        {
            PermissionCode = permissionCode;
        }
    }
}