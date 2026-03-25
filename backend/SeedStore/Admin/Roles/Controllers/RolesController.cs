using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Roles.Interfaces;
using SeedStore.Admin.Roles.Models;
using SeedStore.Support.Admin.Reorder.Models;
using System.Security.Claims;

namespace SeedStore.Admin.Roles.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _rolesService.GetRolesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> GetRole(int id)
        {
            var result = await _rolesService.GetRoleAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> AddRole([FromBody] ChangeRoleModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.AddRoleAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] ChangeRoleModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.UpdateRoleAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.DeleteRoleAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/role")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] int roleId)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.AssignRoleAsync(id, roleId, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "role_not_found" => NotFound("role_not_found"),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }

        [HttpGet("{id}/permissions")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> GetRolePermissions(int id)
        {
            var result = await _rolesService.GetRolePermissionsAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}/permissions")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> SetRolePermissions(int id, [FromBody] List<int> permissionIds)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.SetRolePermissionsAsync(id, permissionIds, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("permissions")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> GetPermissions()
        {
            var result = await _rolesService.GetPermissionsAsync();
            return Ok(result);
        }

        [HttpPost("permissions")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.AddPermissionAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("permissions/{id}")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.UpdatePermissionAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("permissions/{id}")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _rolesService.DeletePermissionAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("reorder/roles")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> ReorderRoles([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _rolesService.ReorderRolesAsync(items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/permissions")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> ReorderPermissions([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _rolesService.ReorderPermissionsAsync(items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/role-permissions")]
        [Authorize(Policy = "Permission:roles.manage")]
        public async Task<IActionResult> ReorderRolePermissions([FromQuery] int roleId, [FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _rolesService.ReorderRolePermissionsAsync(roleId, items, initiatorId);
            return Ok();
        }
    }
}