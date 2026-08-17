using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleSellBooks_BusinessLayer.statuses;
using SimpleSellBooks_DataLayer.statuses;

namespace SimpleSellBooks_API.Controllers.statuses
{
    [Route("api/Statuses")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<clsStatuseDTO>> GetAllStatuses()
        {
            IEnumerable<clsStatuseDTO> statuseList = clsStatuseBusiness.GetAllStatuses();

            if (!statuseList.Any())
            {
                return NotFound("No Statuses Found!");
            }

            return Ok(statuseList);
        }


        [HttpGet("FindByStatusID/{statusID}", Name = "GetStatuseByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<clsStatuseDTO> GetStatuseByID(int statusID)
        {
            if (statusID < 1)
            {
                return BadRequest($"Not accepted statusID {statusID}");
            }

            clsStatuseBusiness? statuse = clsStatuseBusiness.FindStatuse(statusID);

            if (statuse == null)
            {
                return NotFound($"Statuse with statusID {statusID} not found.");
            }

            return Ok(statuse.statuseDTO);
        }
    }
}
