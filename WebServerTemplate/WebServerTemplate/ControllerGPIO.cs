
using nanoFramework.Json;
using nanoFramework.WebServer;
using System.Net;

namespace WebServerTemplate
{
    public class ControllerGPIO
    {
        private readonly ControllerService _service = new();

        [Route("test")]
        public void Test(WebServerEventArgs e)
        {
            var context = e.Context;
            var rawUrl = context.Request.RawUrl;

            WebServer.OutPutStream(e.Context.Response, "Succeded!");
            //WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
        }

        [Route("GetDeviceConfig")]
        [Method("GET")]
        public void GetDeviceConfig(WebServerEventArgs e)
        {
            var config = new
            {
                Led  = new
                {
                    RGB = true,
                    Dimmable = true,
                },
                LedSettings = ""
            };

            var json = JsonSerializer.SerializeObject(config);
        }
    }
}
