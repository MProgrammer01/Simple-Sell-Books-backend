using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleSellBooks_BusinessLayer.sellers;
using SimpleSellBooks_DataLayer.sellers;
using System.Security.Claims;

namespace SimpleSellBooks_API.Controllers.sellers
{
    [Route("api/Sellers")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllSellers")]
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


        [HttpGet("FindBySellerID/{sellerID}", Name = "GetSellerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<clsSellerDTO> GetSellerByID(int sellerID)
        {
            if (sellerID < 1)
            {
                return BadRequest($"Not accepted sellerID {sellerID}");
            }

            clsSellerBusiness? seller = clsSellerBusiness.FindSeller(sellerID);

            if (seller == null)
            {
                return NotFound($"Seller with sellerID {sellerID} not found.");
            }

            return Ok(seller.sellerDTO);
        }

        
        [HttpPost(Name = "AddNewSeller")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<clsSellerDTO> AddNewSeller(clsSellerDTO newSellerDTO)
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
                return CreatedAtRoute("GetSellerByID",
                    new { sellerID = seller.sellerID },
                    seller.sellerDTO);
            }

            return BadRequest("Failed to add seller.");
        }


        [HttpPut("{id}", Name = "UpdateSeller")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<clsSellerDTO> UpdateSeller(int id, clsSellerDTO updatedSellerDTO)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (updatedSellerDTO == null || string.IsNullOrEmpty(updatedSellerDTO.storeName))
            {
                return BadRequest("Invalid seller data.");
            }

            clsSellerBusiness? seller = clsSellerBusiness.FindSeller(id);

            if (seller == null)
            {
                return NotFound($"Seller with ID {id} not found.");
            }

            seller.storeName = updatedSellerDTO.storeName;
            seller.logoStore = !string.IsNullOrEmpty(updatedSellerDTO.logoStore) ? updatedSellerDTO.logoStore : null;

            if (seller.Save())
            {
                return Ok(seller.sellerDTO);
            }

            return BadRequest("Failed to update seller.");
        }


        [HttpDelete("{id}", Name = "DeleteSeller")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteSeller(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            clsSellerBusiness? seller = clsSellerBusiness.FindSeller(id);

            if (seller == null)
            {
                return NotFound($"Seller with ID {id} not found.");
            }

            if (clsSellerBusiness.DeleteSeller(id))
            {
                return Ok($"Seller with ID {id} has been deleted.");
            }

            return BadRequest("Failed to delete seller.");
        }
    }
}
