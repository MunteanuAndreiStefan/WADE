using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FakeNewsDetectionCache.Interfaces
{
    public interface IDataToRdfService
    {
        Task<XmlDocument> GenerateDocument();
    }
}
