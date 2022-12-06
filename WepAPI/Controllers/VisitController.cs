using Microsoft.AspNetCore.Mvc;
using WepAPI.Models;
using WepAPI.Repository;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private readonly IDataService<Visit> _visit;

        public VisitController(IDataService<Visit> visit)
        {
            _visit = visit ?? throw new ArgumentNullException(nameof(visit));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_visit.GetAll());
        }

        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_visit.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] Visit visit)
        {
            _visit.Create(visit);
            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Put(int id, Visit visit)
        {
            _visit.Update(id, visit);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public JsonResult Delete(int id)
        {
            _visit.Delete(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
