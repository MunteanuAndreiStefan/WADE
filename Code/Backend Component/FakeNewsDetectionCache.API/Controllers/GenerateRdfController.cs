using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml;
using FakeNewsDetectionCache.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeNewsDetectionCache.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateRdfController : ControllerBase
    {
        private IDataToRdfService dataToRdfService;
        public GenerateRdfController(IDataToRdfService dataToRdfService)
        {
            this.dataToRdfService = dataToRdfService;
        }

        [HttpGet]
        public async Task Get()
        {
            var item = await dataToRdfService.GenerateDocument();


            Response.Clear();
            Response.Headers.Add("Content-Disposition", "attachment;filename=MyXmlDocument.xml");
            Response.Headers.Add("Content-Length", item.OuterXml.Length.ToString());
            Response.ContentType = "application/octet-stream";
            await Response.WriteAsync(item.OuterXml);
            
            
        }
    }
}