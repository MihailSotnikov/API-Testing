using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace PracticaAPI.Steps
{
    [Binding]
    public class APISteps
    {
        RestClient client;
        Dictionary<string, string> userdata;
        IRestResponse response;
        string userName;
        string email;
        string pass = "Qwe1234";
        Dictionary<string, object> companydata;

        [Given(@"creat new rest client")]
        public void GivenCreatNewRestClient()
        {
            client = new RestClient("http://users.bugred.ru");
        }

        [Given(@"data for registration is ready")]
        public void GivenDataForRegistrationIsReady()
        {
            Random random = new Random();
            string time = DateTime.Now.ToString();
            string temp;


            
            temp = time.Replace(":", ".").Replace(" ", "").Replace("/", "");
            userName = temp;
            email = "user" + temp + "@gmail.com";
           

            userdata = new Dictionary<string, string>
            {
                {"name", userName },
                {"email", email },
                {"password", pass }
            };
        }

        [When(@"I send post registration request")]
        public void WhenISendPostRegistrationRequest()
        {
            RestRequest request = new RestRequest("tasks/rest/doregister", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userdata);
            response = client.Execute(request);
        }

        [Then(@"status code request ok")]
        public void ThenStatusCodeRequestOk()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [Then(@"name from responce equal name from request")]
        public void ThenNameFromResponceEqualNameFromRequest()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(userName, json["name"]?.ToString());
        }

        [Then(@"email from responce equal email from request")]
        public void ThenEmailFromResponceEqualEmailFromRequest()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(email, json["email"]?.ToString());
        }

        //////////////////////////////////////////////////////////////////

        [Given(@"data for registration with invalid email is ready")]
        public void GivenDataForRegistrationWithInvalidEmailIsReady()
        {
            userName = "Santiago";
            email = "zx@z.x";
            userdata = new Dictionary<string, string>
            {
                {"name",userName},
                {"email", email},
                {"password", pass}
            };
        }

        [Then(@"the request response contains an error message")]
        public void ThenTheRequestResponseContainsAnErrorMessage()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(" email zx@z.x уже есть в базе", json["message"]?.ToString());
        }

        ////////////////////////////////////////////////////////////////////////////////////

        [Given(@"data about the new company is ready")]
        public void GivenDataAboutTheNewCompanyIsReady()
        {
            companydata = new Dictionary<string,object>
            {
                {"company_name", "Focal"},
                {"company_type", "ООО" },
                {"company_users", new List<string>{ "blacksTest@i.ua" } },
                {"email_owner", "r1@mail.ru" }
            };
           
        }
        [When(@"I send post company registration request")]
        public void WhenISendPostCompanyRegistrationRequest()
        {
            RestRequest request = new RestRequest("tasks/rest/createcompany", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(companydata);
            response = client.Execute(request);
        }


        [Then(@"the request response contains an type -success")]
        public void ThenTheRequestResponseContainsAnType_Success()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("success", json["type"]?.ToString());
        }





    }
}
