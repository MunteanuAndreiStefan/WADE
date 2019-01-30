using FakeNewsDetectionCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FakeNewsDetectionCache.Service
{
    public class DataToRdfService : IDataToRdfService
    {
        private ITwitterUserService twitterUserService;
        private INewsArticleService newsArticleService;
        private IVoteService voteService;
        private IApplicationUserService applicationUserService;

        private const string BaseUrl = "https://fakenewsdetection.azurewebsites.net/";

        public DataToRdfService(
            ITwitterUserService twitterUserService,
            INewsArticleService newsArticleService,
            IVoteService voteService,
            IApplicationUserService applicationUserService)
        {
            this.twitterUserService = twitterUserService;
            this.newsArticleService = newsArticleService;
            this.voteService = voteService;
            this.applicationUserService = applicationUserService;
        }

        private XmlDocument CreateDocument()
        {
            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", null, null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            return doc;
        }

        private async Task<XmlDocument> CreateRdfElement(XmlDocument doc)
        {
            //(2) string.Empty makes cleaner code
            XmlElement rdfElement = doc.CreateElement("rdf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            doc.AppendChild(rdfElement);

            doc = await AddTwitterUsers(doc, rdfElement);
            doc = await AddNewsArticles(doc, rdfElement);
            doc = await AddApplicationUsers(doc, rdfElement);

            return doc;
        }

        private async Task<XmlDocument> AddTwitterUsers(XmlDocument doc,XmlElement rdfElement)
        {
            var users = await twitterUserService.GetAll();


            foreach (var user in users)
            {
                XmlElement element = doc.CreateElement("rdf:Description");
                var uri = BaseUrl+"api/twitterUsers/Id/" + user.Id;

                element.SetAttribute("about", uri);

                var property = doc.CreateElement("Username");
                property.InnerText = user.Username;

                element.AppendChild(property);

                property = doc.CreateElement("CredibilityScore");
                property.InnerText = string.Empty+user.CredibilityScore?? "0" ;

                element.AppendChild(property);

                var newsArticlles = (await newsArticleService.GetAsQueriable()).Where(n => n.UserId == user.Id);
                foreach (var newsArticle in newsArticlles)
                {
                    var applicationUserURI = BaseUrl + "api/newsArticles/Id/" + newsArticle.Id;
                    property = doc.CreateElement("Posted");
                    property.InnerText = applicationUserURI;
                    element.AppendChild(property);
                }


                rdfElement.AppendChild(element);
            }

            return doc;
        }

        private async Task<XmlDocument> AddNewsArticles(XmlDocument doc, XmlElement rdfElement)
        {
            var newsArticles = await newsArticleService.GetAll();

            

            foreach (var newsArticle in newsArticles)
            {
                XmlElement element = doc.CreateElement("rdf:Description");
                var uri = BaseUrl + "api/newsArticeles/Id" + newsArticle.Id;
                var twitterUserURI = BaseUrl + "api/twitterUsers/Id/" + newsArticle.UserId;

                element.SetAttribute("about", uri);

                var property = doc.CreateElement("Source");
                property.InnerText = newsArticle.Source;
                element.AppendChild(property);

                property = doc.CreateElement("CredibilityScore");
                property.InnerText = string.Empty + newsArticle.CredibilityScore ?? "0";
                element.AppendChild(property);

                property = doc.CreateElement("TwitterUser");
                property.InnerText = twitterUserURI;
                element.AppendChild(property);

                var newsArticlleVotes = (await voteService.GetAsQueriable()).Where(v => v.NewsArticleId == newsArticle.Id);
                foreach(var vote in newsArticlleVotes)
                {
                    var applicationUserURI= BaseUrl + "api/applicationUsers/Id/" + vote.ApplicationUserId;
                    property = doc.CreateElement("VotedBy");
                    property.InnerText = applicationUserURI;
                    element.AppendChild(property);
                }


                rdfElement.AppendChild(element);
            }

            return doc;
        }

        private async Task<XmlDocument> AddApplicationUsers(XmlDocument doc, XmlElement rdfElement)
        {
            var users = await applicationUserService.GetAll();


            foreach (var user in users)
            {
                XmlElement element = doc.CreateElement("rdf:Description");
                var uri = "https://localhost:44345/api/applicationUsers/id/?id=" + user.Id;

                element.SetAttribute("about", uri);

                var property = doc.CreateElement("Username");
                property.InnerText = user.Username;

                element.AppendChild(property);

                property = doc.CreateElement("UserId");
                property.InnerText = user.UserId;
                element.AppendChild(property);

                property = doc.CreateElement("CredibilityScore");
                property.InnerText = string.Empty + user.CredibilityScore ?? "0";

                element.AppendChild(property);

                var newsArticlleVotes = (await voteService.GetAsQueriable()).Where(v => v.ApplicationUserId == user.Id);
                foreach (var vote in newsArticlleVotes)
                {
                    var applicationUserURI = "https://localhost:44345/api/newsArticles/id/?id=" + vote.NewsArticleId;
                    property = doc.CreateElement("Voted");
                    property.InnerText = applicationUserURI;
                    element.AppendChild(property);
                }

                rdfElement.AppendChild(element);
            }

            return doc;
        }

        public async Task<XmlDocument> GenerateDocument()
        {
            var doc = CreateDocument();

            doc = await CreateRdfElement(doc);
            

            return doc;
        }
    }
}
