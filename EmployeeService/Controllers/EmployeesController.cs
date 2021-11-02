using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;  
using System.Web.Http.Description;
using EmployeeService;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {
        private EmployeeDBContext db = new EmployeeDBContext();

        // GET: api/Employees
        public HttpResponseMessage GetEmployees()
        {
            return Request.CreateResponse(db.Employees.ToList());
        }

        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public HttpResponseMessage GetEmployeeByID(int id)
        {
            Employee employee = db.Employees.Find(id);
            return Request.CreateResponse(employee);
        }
        // GET: api/Employees
        [HttpGet]
        public HttpResponseMessage GetEmployeeByName(string name)
        {
            return Request.CreateResponse(db.Employees.Where(e => e.FirstName.Contains(name)).ToList());
        }

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.ID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public async Task<HttpResponseMessage> PostEmployee([FromBody] Employee employee)
        {
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
    
            return Request.CreateResponse(HttpStatusCode.Created,employee);
        }

        // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.ID == id) > 0;
        }
    }
}