using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using SimpleSellBooks_API.helper_methods;
using SimpleSellBooks_API.Services;
using SimpleSellBooks_BusinessLayer.sellers;
using SimpleSellBooks_DataLayer.sellers;
using System.Security.Claims;

namespace SimpleSellBooks_API.Controllers.sellers
{
    [Authorize]
    [Route("api/Sellers")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly ISecurityAuditService _auditService;

        public SellersController(ISecurityAuditService auditService)
        {
            _auditService = auditService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllSellers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsSellerDTO>> GetAllSellers()
        {
            IEnumerable<clsSellerDTO> sellerList = clsSellerBusiness.GetAllSellers();

            if (!sellerList.Any())
            {
                return NotFound("No Sellers Found!");
            }

            return Ok(sellerList);
        }


        //[Authorize(Roles = "Admin, Seller")]
        [HttpGet("FindSellerByID", Name = "GetSellerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<clsSellerDTO>> GetSellerByID(int sellerID,
                [FromServices] IAuthorizationService authorizationService)
        {
            if (sellerID < 1)
            {
                return BadRequest($"Not accepted sellerID {sellerID}");
            }

            clsSellerBusiness? seller = clsSellerBusiness.FindSellerByID(sellerID);

            if (seller == null)
            {
                return NotFound($"Seller with sellerID {sellerID} not found.");
            }

            var authResult = await authorizationService.AuthorizeAsync(
                        User,
                        sellerID,
                        "OwnerOrAdmin");
            
            if (!authResult.Succeeded){

                await _auditService.LogAsync(
                    SecurityEventTypeAndAction.AuthorizationDenied.ToString(),
                    HttpContext,
                    statusCode: StatusCodes.Status403Forbidden,
                    details: "User is not authorized to access this seller.",
                    action: SecurityAction.AccessDenied,
                    targetId: sellerID.ToString(),
                    targetType: "Seller",
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext)
                );
                return Forbid();
            } // 403

            return Ok(seller.sellerDTO);
        }

        [HttpGet("FindSellerByPersonID", Name = "GetSellerByPersonID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<clsSellerDTO>> GetSellerByPersonID([FromServices] IAuthorizationService authorizationService)
        {
            int personID = clsHelperMethods.GetCurrentUserId(HttpContext) ?? 0;

            if (personID < 1)
            {
                return BadRequest($"Not accepted personID {personID}");
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
                    details: "User is not authorized to access this seller.",
                    action: SecurityAction.AccessDenied,
                    targetId: personID.ToString(),
                    targetType: "Seller",
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext)
                );
                return Forbid();
            } // 403


            clsSellerBusiness? seller = clsSellerBusiness.FindSellerByPersonID(personID);

            if (seller == null)
            {
                return NotFound($"Seller with personID {personID} not found.");
            }

            
            return Ok(seller.sellerByPersonIDDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewSeller")]
        [EnableRateLimiting("CreatePolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<clsSellerDTO>> AddNewSeller(clsSellerDTO newSellerDTO)
        {
            if (newSellerDTO == null || newSellerDTO.personID < 0 || 
                string.IsNullOrEmpty(newSellerDTO.storeName))
            {
                return BadRequest("Invalid seller data.");
            }

            clsSellerBusiness seller = new clsSellerBusiness();
            seller.personID = newSellerDTO.personID;
            seller.storeName = newSellerDTO.storeName;
            seller.logoStore = !string.IsNullOrEmpty(newSellerDTO.logoStore) ? newSellerDTO.logoStore : null ;

            if (seller.Save())
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status201Created,
                    targetType: "Seller",
                    targetId: seller.sellerID.ToString(),
                    details: "Admin Added New Seller."
                );

                return CreatedAtRoute("GetSellerByID",
                    new { sellerID = seller.sellerID },
                    seller.sellerDTO);
            }

            return BadRequest("Failed to add seller.");
        }


        //[Authorize(Roles = "Admin, Seller")]
        [HttpPut("UpdateSellerByID")]
        [EnableRateLimiting("UpdatePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<clsSellerDTO>> UpdateSeller(
            [FromServices] IAuthorizationService authorizationService, 
            clsSellerDTO updatedSellerDTO)
        {
            if (updatedSellerDTO.personID < 1)
            {
                return BadRequest($"Not accepted ID {updatedSellerDTO.personID}");
            }

            

            var authResult = await authorizationService.AuthorizeAsync(
                        User,
                        updatedSellerDTO.personID,
                        "OwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                await _auditService.LogAsync(
                    SecurityEventTypeAndAction.AuthorizationDenied.ToString(),
                    HttpContext,
                    statusCode: StatusCodes.Status403Forbidden,
                    details: "Seller is not authorized to access this seller for updating.",
                    action: SecurityAction.AccessDenied,
                    targetId: updatedSellerDTO.personID.ToString(),
                    targetType: "Book",
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext)
                );
                return Forbid();
            } // 403

            if (updatedSellerDTO == null || 
                string.IsNullOrEmpty(updatedSellerDTO.fullName) ||
                string.IsNullOrEmpty(updatedSellerDTO.email) ||
                string.IsNullOrEmpty(updatedSellerDTO.storeName))
            {
                return BadRequest("Invalid seller data.");
            }

            clsSellerBusiness? seller = clsSellerBusiness.FindSellerByPersonID(updatedSellerDTO.personID);

            if (seller == null)
            {
                return NotFound($"Seller with ID {updatedSellerDTO.personID} not found.");
            }
            seller.fullName = updatedSellerDTO.fullName;
            seller.email = updatedSellerDTO.email;
            seller.phone = !string.IsNullOrEmpty(updatedSellerDTO.phone) ? updatedSellerDTO.phone : null;
            seller.addressPerson = !string.IsNullOrEmpty(updatedSellerDTO.addressPerson) ? updatedSellerDTO.addressPerson : null;

            seller.storeName = updatedSellerDTO.storeName;
            seller.logoStore = !string.IsNullOrEmpty(updatedSellerDTO.logoStore) ? updatedSellerDTO.logoStore : null;

            if (seller.Save())
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status200OK,
                    targetType: "Seller",
                    targetId: seller.sellerID.ToString(),
                    details: $"Seller Was Updated By{clsHelperMethods.GetCurrentRole(HttpContext)}."
                );
                return Ok(seller.sellerDTO);
            }

            return BadRequest("Failed to update seller.");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteSellerByID")]
        [EnableRateLimiting("DeletePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSeller(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            clsSellerBusiness? seller = clsSellerBusiness.FindSellerByID(id);

            if (seller == null)
            {
                return NotFound($"Seller with ID {id} not found.");
            }

            if (clsSellerBusiness.DeleteSeller(id))
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status200OK,
                    targetType: "Seller",
                    targetId: seller.sellerID.ToString(),
                    details: "Admin Deleted Seller."
                );
                return Ok($"Seller with ID {id} has been deleted.");
            }

            return BadRequest("Failed to delete seller.");
        }
    }
}
