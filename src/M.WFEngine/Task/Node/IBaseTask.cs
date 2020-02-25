
using M.WFEngine.AccessService;
using M.WorkFlow.Model;

namespace M.WFEngine.Task
{
    public interface IBaseTask
    {
        string GetBisData(WFTaskEntity taskEntity, string dataId, string serviceId, EAccessMessageType messageType);
    }
}
