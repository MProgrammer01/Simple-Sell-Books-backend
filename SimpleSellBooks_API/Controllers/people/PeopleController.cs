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

        [Authorize(Roles = "Admin")]
        [HttpGet("All", Name = "GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<clsPersonDTO>> GetAllPeople()
        {
            IEnumerable<clsPersonDTO> personList = clsPersonBusiness.GetAllPersons();

            if (!personList.Any())
            {
                return NotFound("No People Found!");
            }

            return Ok(personList);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("FindByPersonID/{personID}", Name = "GetPersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<clsPersonDTO> GetPersonByID(int personID)
        {
            if (personID < 1)
            {
                return BadRequest($"Not accepted personID {personID}");
            }

            clsPersonBusiness? person = clsPersonBusiness.FindPersonByID(personID);

            if (person == null)
            {
                return NotFound($"Person with personID {personID} not found.");
            }

            return Ok(person.personDTO);
        }


        //[HttpGet("FindByPersonEmail/{email}", Name = "GetPersonByEmail")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]

        //public ActionResult<clsPersonDTO> GetPersonByEmail(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return BadRequest($"Not accepted person email {email}");
        //    }

        //    clsPersonBusiness? person = clsPersonBusiness.FindPersonByEmail(email);

        //    if (person == null)
        //    {
        //        return NotFound($"Person with email {email} not found.");
        //    }

        //    return Ok(person.personDTO);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddNewPerson")]
        [EnableRateLimiting("CreatePolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<clsPersonDTO>> AddNewPerson(clsSignUpDTO newPersonDTO)
        {
            if (newPersonDTO == null || string.IsNullOrEmpty(newPersonDTO.fullName) || 
                string.IsNullOrEmpty(newPersonDTO.email) || 
                string.IsNullOrEmpty(newPersonDTO.password))
            {
                return BadRequest("Invalid person data.");
            }

            clsPersonBusiness person = new clsPersonBusiness();
            person.fullName = newPersonDTO.fullName;
            person.email = newPersonDTO.email;
            person.password = newPersonDTO.password;
            person.phone = !string.IsNullOrEmpty(newPersonDTO.phone) ? newPersonDTO.phone : null;
            person.addressPerson = !string.IsNullOrEmpty(newPersonDTO.addressPerson) ? newPersonDTO.addressPerson : null;
            //person.role = !string.IsNullOrEmpty(newPersonDTO.role) ? newPersonDTO.role : null;

            if (person.Save())
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status201Created,
                    targetType: "Person",
                    targetId: person.personID.ToString(),
                    details: "Admin Added New Person."
                );
                return CreatedAtRoute("GetPersonByID",
                    new { personID = person.personID },
                    person.personDTO);
            }

            return BadRequest("Failed to add person.");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdatePerson")]
        [EnableRateLimiting("UpdatePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<clsPersonDTO>> UpdatePerson(int id, clsUpdatePersonDTO updatedPersonDTO)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (updatedPersonDTO == null || 
                string.IsNullOrEmpty(updatedPersonDTO.fullName) ||
                string.IsNullOrEmpty(updatedPersonDTO.email))
            {
                return BadRequest("Invalid person data.");
            }

            clsPersonBusiness? person = clsPersonBusiness.FindPersonByID(id);

            if (person == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            person.fullName = updatedPersonDTO.fullName;
            person.email = updatedPersonDTO.email;
            person.phone = !string.IsNullOrEmpty(updatedPersonDTO.phone) ? updatedPersonDTO.phone : null;
            person.addressPerson = !string.IsNullOrEmpty(updatedPersonDTO.addressPerson) ? updatedPersonDTO.addressPerson : null;
            //person.role = !string.IsNullOrEmpty(updatedPersonDTO.role) ? updatedPersonDTO.role : null;

            if (person.Save())
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status200OK,
                    targetType: "Person",
                    targetId: person.personID.ToString(),
                    details: "Admin Updated Person."
                );
                return Ok(person.personDTO);
            }

            return BadRequest("Failed to update person.");
        }


        //[Authorize(Roles = "Admin")]
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

            clsPersonBusiness? person = clsPersonBusiness.FindPersonByID(personID);

            if (person == null)
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
