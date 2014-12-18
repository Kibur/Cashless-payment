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
    public class ProductController : ApiController
    {
        [AllowAnonymous]
        public List<Product> Get()
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            return ProductDA.GetProducts(cp.Claims);
        }

        public HttpResponseMessage Post(Product p)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            int id = ProductDA.InsertProduct(p, cp.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());

            return message;
        }

        public HttpResponseMessage Put(Product p)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            ProductDA.UpdateProduct(p, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            ProductDA.DeleteProduct(id, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
