using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SimpleSellBooks_API.helper_methods;
using SimpleSellBooks_API.Services;
using SimpleSellBooks_BusinessLayer.people;
using SimpleSellBooks_BusinessLayer.sellers;
using SimpleSellBooks_DataLayer.people;
using SimpleSellBooks_DataLayer.sellers;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SimpleSellBooks_API.Controllers.people
{
    [Authorize]
    [Route("api/People")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ISecurityAuditService _auditService;

        public PeopleController(ISecurityAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpDelete("DeletePerson")]
        [EnableRateLimiting("DeletePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePerson(
            [FromServices] IAuthorizationService authorizationService,
            int personID)
        {
            if (personID < 1)
            {
                return BadRequest($"Not accepted ID {personID}");
            }

            var authResult = await authorizationService.AuthorizeAsync(
                        User,
                        personID,
                        "OwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                await _auditService.LogAsync(
                    SecurityEventTypeAndAction.AuthorizationDenied.ToString(),
                    HttpContext,
                    statusCode: StatusCodes.Status403Forbidden,
                    details: "Person is not authorized to access this person for updating.",
                    action: SecurityAction.AccessDenied,
                    targetId: personID.ToString(),
                    targetType: "Person",
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext)
                );
                return Forbid();
            } // 403

            if (!clsPersonBusiness.IsPersonExists(personID))
            {
                return NotFound($"Person with ID {personID} not found.");
            }

            if (clsPersonBusiness.DeletePerson(personID))
            {
                return Ok($"Person with ID {personID} has been deleted.");
            }

            return BadRequest("Failed to delete person.");
        }

        [HttpPut("UpdatePasswordByPersonID")]
        [EnableRateLimiting("UpdatePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdatePasswordByPersonID(
            [FromServices] IAuthorizationService authorizationService,
            clsChangePasswordDTO updatedPasswordDTO)
        {
            if (updatedPasswordDTO.personID < 1)
            {
                return BadRequest($"Not accepted ID {updatedPasswordDTO.personID}");
            }

            var authResult = await authorizationService.AuthorizeAsync(
                        User,
                        updatedPasswordDTO.personID,
                        "OwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                await _auditService.LogAsync(
                    SecurityEventTypeAndAction.AuthorizationDenied.ToString(),
                    HttpContext,
                    statusCode: StatusCodes.Status403Forbidden,
                    details: "Person is not authorized to access this Person for updating.",
                    action: SecurityAction.AccessDenied,
                    targetId: updatedPasswordDTO.personID.ToString(),
                    targetType: "Person",
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext)
                );
                return Forbid();
            } // 403

            if (updatedPasswordDTO == null ||
                string.IsNullOrEmpty(updatedPasswordDTO.newPassword) ||
                string.IsNullOrEmpty(updatedPasswordDTO.currentPassword)
                )
            {
                return BadRequest("Invalid password data.");
            }

            string personPasswordHash = clsPersonBusiness.getPasswordHashByPersonID(updatedPasswordDTO.personID);

            if (string.IsNullOrEmpty(personPasswordHash))
            {
                return NotFound($"Person with Password {updatedPasswordDTO.personID} not found.");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                updatedPasswordDTO.currentPassword, 
                personPasswordHash);

            if (!isValidPassword)
            {
                await _auditService.LogAsync(
                    SecurityEventTypeAndAction.AuthorizationDenied.ToString(),
                    HttpContext,
                    statusCode: StatusCodes.Status400BadRequest,
                    details: "Current password is incorrect.",
                    action: SecurityAction.AccessDenied,
                    targetId: updatedPasswordDTO.personID.ToString(),
                    targetType: "Person",
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext)
                );
                return BadRequest("Current password is incorrect.");
            }

            if (clsPersonBusiness.UpdatePasswordHash(updatedPasswordDTO.personID, updatedPasswordDTO.newPassword))
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status200OK,
                    targetType: "Person",
                    targetId: updatedPasswordDTO.personID.ToString(),
                    details: $"Person Password Was Updated By {clsHelperMethods.GetCurrentRole(HttpContext)}."
                );
                return Ok("Person Password Was Updated");
            }

            return BadRequest("Failed to update person password.");
        }


    }
}
