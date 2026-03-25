using Microsoft.AspNetCore.Authorization;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Support.Admin.PermissionsHandling.Interfaces;
using SeedStore.Support.Admin.PermissionsHandling.Requirements;
using System.Security.Claims;

namespace SeedStore.Support.Admin.PermissionsHandling.Handlers
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
        {
            var roleCode = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleCode))
            {
                context.Fail();
                return;
            }

            var priorityClaim = context.User.FindFirst("priority")?.Value;
            if (int.TryParse(priorityClaim, out int priority) && priority == 1)
            {
                context.Succeed(requirement);
                return;
            }

            var permissions = await _permissionRepository.GetRolePermissionCodesByRoleCodeAsync(roleCode);

            if (permissions.Contains(requirement.PermissionCode))
                context.Succeed(requirement);
            else
                context.Fail();
        }

        private async Task<EmployeeEntity?> GetEmployeeWithRoleAsync(int employeeId)
        {
            return null;
        }
    }
}