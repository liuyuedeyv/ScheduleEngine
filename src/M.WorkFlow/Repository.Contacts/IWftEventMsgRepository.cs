using System;
using System.Collections.Generic;
using System.Text;
using FD.Simple.DB;
using M.WorkFlow.Model;

namespace M.WFDesigner.Repository.Contacts
{
    interface IWftEventMsgRepository
    {
        WFTEventMsgEntity GetById(string id);
         
        DBCollection<WFTEventMsgEntity> GetList(TableFilter tf);

    }
}
