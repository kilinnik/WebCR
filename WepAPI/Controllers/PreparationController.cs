using Microsoft.AspNetCore.Mvc;
using WepAPI.Models;
using WepAPI.Repository;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreparationController : ControllerBase
    {
        private readonly IDataService<Preparation> _preparation;

        public PreparationController(IDataService<Preparation> preparation)
        {
            _preparation = preparation ?? throw new ArgumentNullException(nameof(preparation));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_preparation.GetAll());
        }

        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_preparation.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] Preparation preparation)
        {
            _preparation.Create(preparation);
            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Put(int id, Preparation preparation)
        {
            _preparation.Update(id, preparation);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public JsonResult Delete(int id)
        {
            _preparation.Delete(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
