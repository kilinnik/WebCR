using Microsoft.AspNetCore.Mvc;
using WepAPI.Models;
using WepAPI.Repository;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private readonly IDataService<Diagnosis> _diagnosis;

        public DiagnosisController(IDataService<Diagnosis> diagnosis)
        {
            _diagnosis = diagnosis ?? throw new ArgumentNullException(nameof(diagnosis));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_diagnosis.GetAll());
        }

        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_diagnosis.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] Diagnosis diagnosis)
        {
            _diagnosis.Create(diagnosis);
            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Put(int id, Diagnosis diagnosis)
        {
            _diagnosis.Update(id, diagnosis);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public JsonResult Delete(int id)
        {
            _diagnosis.Delete(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
