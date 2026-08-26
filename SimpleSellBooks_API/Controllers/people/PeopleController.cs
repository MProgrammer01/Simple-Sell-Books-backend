using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SimpleSellBooks_BusinessLayer.people;
using SimpleSellBooks_DataLayer.people;
using System.Security.Claims;

namespace SimpleSellBooks_API.Controllers.people
{
    [Authorize]
    [Route("api/People")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
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
        public ActionResult<clsPersonDTO> AddNewPerson(clsSignUpDTO newPersonDTO)
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
        public ActionResult<clsPersonDTO> UpdatePerson(int id, clsUpdatePersonDTO updatedPersonDTO)
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
                return Ok(person.personDTO);
            }

            return BadRequest("Failed to update person.");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeletePerson")]
        [EnableRateLimiting("DeletePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePerson(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            clsPersonBusiness? person = clsPersonBusiness.FindPersonByID(id);

            if (person == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            if (clsPersonBusiness.DeletePerson(id))
            {
                return Ok($"Person with ID {id} has been deleted.");
            }

            return BadRequest("Failed to delete person.");
        }
    }
}
