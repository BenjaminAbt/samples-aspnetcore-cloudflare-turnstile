// Copyright © BEN ABT - all rights reserved
// https://schwabencode.com

using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BENABT.Samples.AspNetCore.CloudflareTurnstile.Controllers;

public class HomeController(CloudflareTurnstileProvider cloudflareTurnstileProvider) : Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(PostSampleSubmitModel submitModel,
        [FromForm(Name = "cf-turnstile-response")] string turnstileToken)
    {
        // read users ip address 
        // proxy? => https://learn.microsoft.com/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5001507
        IPAddress? userIP = Request.HttpContext.Connection.RemoteIpAddress;

        // verify token
        CloudflareTurnstileVerifyResult cftResult = await cloudflareTurnstileProvider
            .Verify(turnstileToken, userIpAddress: userIP);

        // in a productive environment you can implement this as action filter

        // present result
        ViewBag.Result = cftResult;

        return View();
    }
}

public record class PostSampleSubmitModel(string SampleInput);