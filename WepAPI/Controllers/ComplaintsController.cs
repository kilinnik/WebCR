using Microsoft.AspNetCore.Mvc;
using WepAPI.Models;
using WepAPI.Repository;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IDataService<Complaints> _complaints;

        public ComplaintsController(IDataService<Complaints> complaints)
        {
            _complaints = complaints ?? throw new ArgumentNullException(nameof(complaints));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_complaints.GetAll());
        }

        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_complaints.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] Complaints complaints)
        {
            _complaints.Create(complaints);
            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Put(int id, Complaints complaints)
        {
            _complaints.Update(id, complaints);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public JsonResult Delete(int id)
        {
            _complaints.Delete(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
