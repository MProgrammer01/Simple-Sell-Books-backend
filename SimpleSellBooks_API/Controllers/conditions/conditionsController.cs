using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleSellBooks_BusinessLayer.conditions;
using SimpleSellBooks_DataLayer.conditions;

namespace SimpleSellBooks_API.Controllers.conditions
{
    [Authorize]
    [Route("api/Conditions")]
    [ApiController]
    public class ConditionsController : ControllerBase
    {
        [Authorize(Roles = "Admin, Seller")]
        [HttpGet("All", Name = "GetAllConditions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsConditionDTO>> GetAllConditions()
        {
            IEnumerable<clsConditionDTO> conditionList = clsConditionBusiness.GetAllConditions();

            if (!conditionList.Any())
            {
                return NotFound("No Conditions Found!");
            }

            return Ok(conditionList);
        }


        [Authorize(Roles = "Admin, Seller")]
        [HttpGet("FindByConditionID/{conditionID}", Name = "GetConditionByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<clsConditionDTO> GetConditionByID(int conditionID)
        {
            if (conditionID < 1)
            {
                return BadRequest($"Not accepted conditionID {conditionID}");
            }

            clsConditionBusiness? condition = clsConditionBusiness.FindCondition(conditionID);

            if (condition == null)
            {
                return NotFound($"Condition with conditionID {conditionID} not found.");
            }

            return Ok(condition.conditionDTO);
        }
    }
}
