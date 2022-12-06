using Microsoft.AspNetCore.Mvc;
using WepAPI.Models;
using WepAPI.Repository;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDataService<Doctor> _doctor;

        public DoctorController(IDataService<Doctor> doctor)
        {
            _doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_doctor.GetAll());
        }

        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_doctor.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] Doctor doctor)
        {
            _doctor.Create(doctor);
            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Put(int id, Doctor doctor)
        {
            _doctor.Update(id, doctor);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public JsonResult Delete(int id)
        {
            _doctor.Delete(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
