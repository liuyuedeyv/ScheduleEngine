using FD.Component.DynamicDao;
using FD.Simple.Utils.Agent;
using M.WFEngine.Model;
using System;
using System.Collections.Generic;

namespace M.WFEngine.DAL
{
    [Table("wfoins")]
    public interface IWFOInsDao
    {
        [Insert("id,flowid,finsid,taskid,tinsid,userid,username,cdate,status")]
        int Insert(List<WFOInsEntity> entitys);

        [Update("processdate,status")]
        int ProcessTask(WFOInsEntity entity);

        [Select]
        WFOInsEntity SelectById(string id);
    }

    [Autowired(typeof(DynamicDaoInterceptor))]
    public class WFOInsDao : IWFOInsDao
    {
        public int Insert(List<WFOInsEntity> entitys)
        {
            throw new NotImplementedException();
        }

        public WFOInsEntity SelectById(string id)
        {
            throw new NotImplementedException();
        }

        public int ProcessTask(WFOInsEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
