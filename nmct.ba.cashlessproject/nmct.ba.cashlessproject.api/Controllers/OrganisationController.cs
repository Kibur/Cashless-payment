using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model.IT_bedrijf;
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
    public class OrganisationController : ApiController
    {
        public bool Get(string username, string password)
        {
            return OrganisationDA.CheckAccount(username, password);
        }

        public HttpResponseMessage Put(Organisation o)
        {
            ClaimsPrincipal cp = RequestContext.Principal as ClaimsPrincipal;
            OrganisationDA.UpdateOrganisation(o, cp.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
