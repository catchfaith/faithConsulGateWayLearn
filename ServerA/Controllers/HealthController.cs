using ConsulRegistHelper;
using ConsulRegistHelper.HttpHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Refit;
using ServerB;

namespace ServerA.Controllers;
[Route("[controller]/[action]")]
[ApiController]
public class HealthController:ControllerBase
{
    public readonly AbstractConsulDispatcher abstractConsulDispatcher;
    public readonly IHttpExtensionsService ExtensionsService;
    public HealthController(AbstractConsulDispatcher abstractConsulDispatcher,
                            IHttpExtensionsService ExtensionsService) { 
        this.abstractConsulDispatcher = abstractConsulDispatcher;
        this.ExtensionsService = ExtensionsService;
    }
    [HttpGet("/healthCheck")]
    public IActionResult check() => Ok("ok");
    [HttpGet("/getService")]
    public async Task<List<Person>> getServcieB() {
        //基于consul获取url地址
        string url = "http://ServerB/getPerson";
        string realUrl = abstractConsulDispatcher.GetAddress(url);
        var gitHubApi = RestService.For<IGitHubApi>(realUrl);
        return await gitHubApi.getPerson();
        // var res =await ExtensionsService.GetAsync(realUrl);
        //return JsonConvert.DeserializeObject<List<Person>>(res);
    }
}