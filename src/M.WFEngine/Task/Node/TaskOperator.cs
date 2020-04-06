using FD.Simple.Utils.Agent;
using M.WFEngine.DAL;
using M.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using FD.Simple.DB;
using M.WFEngine.Model;

namespace M.WFEngine.Task
{
    public class TaskOperator : BaseTask
    {
        public override ETaskType TaskType => ETaskType.Operator;

        public override bool NeedWaitCallbackWhenCreateJob => true;

        [Autowired]
        public IWFOperDao _WFOperDao { get; set; }

        [Autowired]
        public ILogger<TaskOperator> _Logging { get; set; }

        [Autowired]
        public IWFOInsDao _WFOinsDao { get; set; }

        public override bool RunTask(WFTaskEntity taskEntity, WFFinsEntity fins, WFTinsEntity tinsEntity, WFTEventEntity mqEntity)
        {
            _Logging.LogInformation($"创建处理人任务，等等处理……");
            var users = GetUserListByOperList(_WFOperDao.SelectByTaskId(taskEntity.ID));
            List<WFOInsEntity> listOIns = new List<WFOInsEntity>();
            foreach (var user in users)
            {
                var entity = new WFOInsEntity();
                entity.Id = DataKeyFactory.NewId();
                entity.FlowId = fins.Flowid;
                entity.FinsId = fins.ID;
                entity.TaskId = taskEntity.ID;
                entity.TinsId = tinsEntity.ID;
                entity.UserId = user.UserId;
                entity.UserName = user.UserName;
                entity.CDate = DateTime.Now;
                entity.Status = 0;
                listOIns.Add(entity);
            }
            var count = _WFOinsDao.Insert(listOIns);
            _Logging.LogInformation($"{tinsEntity.ID } 创建处理人实例成功，数量：{listOIns.Count }");
            return false;
        }

        List<OperUserModel> GetUserListByOperList(List<WFOperEntity> opers)
        {
            var list = new List<OperUserModel>();
            foreach (var oper in opers)
            {
                if (oper.OperType != EWFOperType.Person)
                {
                    continue;
                }
                list.Add(new OperUserModel()
                {
                    UserId = oper.Operator,
                    UserName = oper.OperDis
                });
            }
            return list;
        }
    }
}
