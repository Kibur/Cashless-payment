using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class ErrorlogController : ApiController
    {
        public HttpResponseMessage Post(Errorlog e)
        {
            ErrorlogDA.InsertErrorlog(e);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
