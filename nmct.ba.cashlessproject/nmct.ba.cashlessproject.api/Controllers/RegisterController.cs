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
    public class RegisterController : ApiController
    {
        public List<Register> Get()
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return RegisterDA.GetRegisters(cp.Claims);
        }

        public HttpResponseMessage Post(Register r)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = RegisterDA.InsertRegister(r, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());

            return message;
        }

        public HttpResponseMessage Put(Register r)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            RegisterDA.UpdateRegister(r, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            RegisterDA.DeleteRegister(id, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
