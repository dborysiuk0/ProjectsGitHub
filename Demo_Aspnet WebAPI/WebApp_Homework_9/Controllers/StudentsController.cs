using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdoNetProvider;
using Microsoft.AspNetCore.Mvc;



namespace WebApp_Homework_9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        public StudentRepository repository = new StudentRepository();


        // GET: api/<StudentsController>
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return repository.GetStudents();
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public IEnumerable<Student> Get(int id)
        {
            return repository.GetStudentID(id);
        } 


        // POST api/<StudentsController>
        [HttpPost]
        public async Task Post([FromBody] Student newStudent)
        {
            if (newStudent == null)
            {
                throw new ArgumentNullException(nameof(newStudent));
            }
            if (newStudent.Name == null || newStudent.LastName == null)
            {
                throw new ArgumentNullException("Name or Lastname are ampty");
            }
            await repository.CreateStudent(newStudent);
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Student newStudent)
        {
            await repository.PutID(id, newStudent);
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await repository.DeleteStudent(id);
        }
    }
}
