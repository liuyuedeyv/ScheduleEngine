

using WebApiClient;
using WebApiClient.Attributes;

namespace M.RemoteService.Controllers
{
    public interface IWFCallback : IHttpApi
    {
        [HttpGet("wft/callback")]
        ITask<dynamic> Call(string mqId);
    }
}
