using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace ShortUrls.APITests
{
    public class ApiTests_ShortUrls
    {
        //private const string base_url = "https://shorturl.ivaylogodu.repl.co/api/urls";       
        private const string base_url = "https://shorturl.nakov.repl.co/api/urls";
        private RestClient client;
        

        [SetUp]
        public void Setup()
        {
           client = new RestClient();
        }
        [Test]
        public void List_Short_Urls()
        {
            // Arrange
            var request = new RestRequest(base_url);
            // Act
            var response = client.Execute(request, Method.Get);
            var urls = JsonSerializer.Deserialize<List<Url>>(response.Content);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(urls);
            Assert.That(urls.Count, Is.GreaterThan(0));

        }
        [Test]
        public void Find_Url_By_Vailid_Short_Code()
        {
            // Arrange
            var request = new RestRequest(base_url + "/{keyword}");
            request.AddUrlSegment("keyword", "seldev");
            // Act
            var response = client.Execute(request, Method.Get); 
            var short_url = JsonSerializer.Deserialize<Url>(response.Content);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(short_url);
            Assert.That(short_url.shortCode, Is.EqualTo("seldev"));
            Assert.That(short_url.url, Is.EqualTo("https://selenium.dev"));
            

        }
        [Test]
        public void Find_Url_By_Invailid_Short_Code()
        {
            // Arrange
            var request = new RestRequest(base_url + "/" + DateTime.Now.Ticks);
            // Act
            var response = client.Execute(request, Method.Get);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }
        
        [Test]
        public void Create_Url_Vailid_Data()
        {
            // Arrange
            var request = new RestRequest(base_url);
            var body = new
            {
                url = "https://www.example.com",
                shortCode = "new" + DateTime.Now.Ticks,
            };
            request.AddJsonBody(body);
            // Act
            var response = client.Execute(request, Method.Post);
            var allUrls = client.Execute(request, Method.Get);
            var urls = JsonSerializer.Deserialize<List<Url>>(allUrls.Content);
            var lastUrl = urls.Last();
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lastUrl, Is.Not.Null);
            Assert.That(lastUrl.url, Is.EqualTo(body.url));
            Assert.That(lastUrl.shortCode, Is.EqualTo(body.shortCode));

        }
        [Test]
        public void Create_Url_Invailid_Data()
        {
            // Arrange
            var request = new RestRequest(base_url);
            var body = new
            {

                shortCode = DateTime.Now.Ticks,
            };
            request.AddJsonBody(body);
            // Act
            var response = client.Execute(request, Method.Post);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"URL cannot be empty!\"}"));

        }
        [Test]
        public void Delete_Url_Vailid_Data()
        {
            // Arrange
            var request = new RestRequest(base_url);
            var body = new
            {
                url = "https://www.example.com",
                shortCode = "new" + DateTime.Now.Ticks,
            };
            request.AddJsonBody(body);
            // Act
            var response = client.Execute(request, Method.Post);
            var requestDelete = new RestRequest(base_url + "/" + body.shortCode);
            var responseDelete = client.Execute(requestDelete, Method.Delete);
            // Arrange
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseDelete.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
        }
        [Test]
        public void Delete_Url_Invailid_Data()
        {
            // Arrange
            var requestDelete = new RestRequest(base_url + "/" + DateTime.Now.Ticks);
            // Act
            var responseDelete = client.Execute(requestDelete, Method.Delete);
            // Arrange
            Assert.That(responseDelete.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }
        
    }
}