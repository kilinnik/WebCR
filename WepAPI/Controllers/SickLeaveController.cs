using Microsoft.AspNetCore.Mvc;
using WepAPI.Models;
using WepAPI.Repository;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SickLeaveController : ControllerBase
    {
        private readonly IDataService<SickLeave> _sickLeave;

        public SickLeaveController(IDataService<SickLeave> sickLeave)
        {
            _sickLeave = sickLeave ?? throw new ArgumentNullException(nameof(sickLeave));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_sickLeave.GetAll());
        }

        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_sickLeave.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] SickLeave sickLeave)
        {
            _sickLeave.Create(sickLeave);
            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Put(int id, SickLeave sickLeave)
        {
            _sickLeave.Update(id, sickLeave);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public JsonResult Delete(int id)
        {
            _sickLeave.Delete(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
