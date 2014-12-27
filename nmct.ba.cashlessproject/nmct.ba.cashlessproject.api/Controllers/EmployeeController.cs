using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    [Authorize]
    public class EmployeeController : ApiController
    {
        public List<Employee> Get()
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployees(cp.Claims);
        }

        [AllowAnonymous]
        public Employee Get(int id)
        {
            return EmployeeDA.GetEmployeeById(id);
        }

        public List<RegisterEmployee> Get(string rID)
        {
            int id = Convert.ToInt32(rID.Substring(1, rID.Length - 1));

            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployeesByRegister(id, cp.Claims);
        }

        public HttpResponseMessage Post(Employee e)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = EmployeeDA.InsertEmployee(e, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());

            return message;
        }

        public HttpResponseMessage Put(Employee e)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.UpdateEmployee(e, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.DeleteEmployee(id, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
