using FD.Component.DynamicDao;
using FD.Simple.Utils.Agent;
using M.WFEngine.Model;
using System;
using System.Collections.Generic;


namespace M.WFEngine.DAL
{
    [Table("wfcmd")]
    public interface IWFCmdDao
    {
        [Select]
        WFCmdEntity SelectById(string id);
    }

    [Autowired(typeof(DynamicDaoInterceptor))]
    public class WFCmdDao : IWFCmdDao
    {
        public WFCmdEntity SelectById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
