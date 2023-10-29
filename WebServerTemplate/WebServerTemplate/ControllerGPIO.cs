
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
    }
}
