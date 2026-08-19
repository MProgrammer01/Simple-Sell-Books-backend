using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleSellBooks_BusinessLayer.categories;
using SimpleSellBooks_DataLayer.categories;

namespace SimpleSellBooks_API.Controllers.categories
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsCategorieDTO>> GetAllCategories()
        {
            IEnumerable<clsCategorieDTO> categorieList = clsCategorieBusiness.GetAllCategories();

            if (!categorieList.Any())
            {
                return NotFound("No Categories Found!");
            }

            return Ok(categorieList);
        }


        [HttpGet("FindByCategoryID/{categoryID}", Name = "GetCategorieByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<clsCategorieDTO> GetCategorieByID(int categoryID)
        {
            if (categoryID < 1)
            {
                return BadRequest($"Not accepted categoryID {categoryID}");
            }

            clsCategorieBusiness? categorie = clsCategorieBusiness.FindCategorie(categoryID);

            if (categorie == null)
            {
                return NotFound($"Categorie with categoryID {categoryID} not found.");
            }

            return Ok(categorie.categorieDTO);
        }

    }
}
