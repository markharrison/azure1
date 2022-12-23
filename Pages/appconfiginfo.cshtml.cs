using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Azure1.Pages
{
    public class AppConfigInfoModel : PageModel
    {
        IConfiguration _config;
        AppConfig _appconfig;
        public string strAppConfigInfoHtml;

        public AppConfigInfoModel(IConfiguration config, AppConfig appconfig)
        {
            _config = config;
            _appconfig = appconfig;
            strAppConfigInfoHtml = "";
        }

        public void OnGet()
        {
            string EchoData(string key, string value)
            {
                return key + ": <span style='color: blue'>" + value + "</span><br/>";
            }

            string pw = HttpContext.Request.Query["pw"].ToString();
            if (string.IsNullOrEmpty(pw) || pw != _appconfig.AdminPW)
                return;

            strAppConfigInfoHtml += EchoData("OS Description", System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            strAppConfigInfoHtml += EchoData("ASPNETCORE_ENVIRONMENT", _config.GetValue<string>("ASPNETCORE_ENVIRONMENT"));
            strAppConfigInfoHtml += EchoData("Framework Description", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
            strAppConfigInfoHtml += EchoData("Instrumentation Key", _config.GetValue<string>("ApplicationInsights:InstrumentationKey"));
            strAppConfigInfoHtml += EchoData("Build Identifier", _config.GetValue<string>("BuildIdentifier"));
            strAppConfigInfoHtml += EchoData("Google Id", _appconfig.GoogleId);
        }
    }
}

 
 
