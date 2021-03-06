﻿using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using eg_03_csharp_auth_code_grant_core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eg_03_csharp_auth_code_grant_core.Controllers
{
    [Route("eg004")]
    public class Eg004EnvelopeInfoController : EgController
    {
        public Eg004EnvelopeInfoController(DSConfiguration config, IRequestItemsService requestItemsService) 
            : base(config, requestItemsService)
        {
            ViewBag.title = "Get envelope information";
        }

        public override string EgName => "eg004";

        private Envelope DoWork(string accessToken, string basePath, string accountId,
            string envelopeId)
        {
            // Data for this method
            // accessToken
            // basePath
            // accountId
            // envelopeId

            var config = new Configuration(new ApiClient(basePath));
            config.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(config);
            ViewBag.h1 = "Get envelope status results";
            ViewBag.message = "Results from the Envelopes::get method:";
            Envelope results = envelopesApi.GetEnvelope(accountId, envelopeId);
            return results;
        }


        [HttpPost]
        public IActionResult Create(string signerEmail, string signerName)
        {
            // Data for this method
            var accessToken = RequestItemsService.User.AccessToken;
            var basePath = RequestItemsService.Session.BasePath + "/restapi";
            var accountId = RequestItemsService.Session.AccountId;
            var envelopeId = RequestItemsService.EnvelopeId;

            // Check the token with minimal buffer time.
            bool tokenOk = CheckToken(3);
            if (!tokenOk)
            {
                // We could store the parameters of the requested operation 
                // so it could be restarted automatically.
                // But since it should be rare to have a token issue here,
                // we'll make the user re-enter the form data after 
                // authentication.
                RequestItemsService.EgName = EgName;
                return Redirect("/ds/mustAuthenticate");
            }

            Envelope results = DoWork(accessToken, basePath, accountId, envelopeId);
        
            ViewBag.h1 = "Get envelope status results";
            ViewBag.message  = "Results from the Envelopes::get method:";
            ViewBag.Locals.Json = JsonConvert.SerializeObject(results, Formatting.Indented);
            return View("example_done");
        }
    }
}