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
    public class CustomerController : ApiController
    {
        public List<Customer> Get()
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return CustomerDA.GetCustomers(cp.Claims);
        }

        [AllowAnonymous]
        public Customer Get(string value)
        {
            int id = 0;

            if (int.TryParse(value, out id))
            {
                return CustomerDA.GetCustomerById(id);
            }

            return CustomerDA.GetCustomerByName(value);
        }

        [AllowAnonymous]
        public HttpResponseMessage Post(Customer c)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = CustomerDA.InsertCustomer(c, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());

            return message;
        }

        [AllowAnonymous]
        public HttpResponseMessage Put(Customer c)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            CustomerDA.UpdateCustomer(c, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
