using DocuSign_integration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DocuSign_integration.Controllers
{
    public class Digital_SignatureController : ApiController
    {

        [HttpGet]

     
        public async Task<string> sendenvelope()
        {
            var dc = new Docusign();
            return await dc.OnPostAsync();
        }
    }
}
