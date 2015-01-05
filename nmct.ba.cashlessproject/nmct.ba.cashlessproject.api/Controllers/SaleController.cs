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
    public class SaleController : ApiController
    {
        public List<Sale> Get()
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return SaleDA.GetSales(cp.Claims);
        }

        [AllowAnonymous]
        public HttpResponseMessage Post(Sale s)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = SaleDA.InsertSale(s, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());

            return message;
        }
    }
}
