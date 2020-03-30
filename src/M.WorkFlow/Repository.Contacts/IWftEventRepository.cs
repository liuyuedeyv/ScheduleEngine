using FD.Simple.DB;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository.Contacts
{
    interface IWftEventRepository
    {
        WFTEventEntity GetById(string id);
         
        DBCollection<WFTEventEntity> GetList(TableFilter tf);

    }
}
