using Microsoft.AspNetCore.Mvc;
using WepAPI.Models;
using WepAPI.Repository;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDataService<DataLogin> _dataLogin;

        public LoginController(IDataService<DataLogin> dataLogin)
        {
            _dataLogin = dataLogin ?? throw new ArgumentNullException(nameof(dataLogin));
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_dataLogin.GetAll());
        }

        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_dataLogin.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] DataLogin dataLogin)
        {
            _dataLogin.Create(dataLogin);
            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Put(int id, DataLogin dataLogin)
        {
            _dataLogin.Update(id, dataLogin);
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public JsonResult Delete(int id)
        {
            _dataLogin.Delete(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
