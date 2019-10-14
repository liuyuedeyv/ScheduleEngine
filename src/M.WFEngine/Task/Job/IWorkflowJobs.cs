using System.Collections.Generic;

namespace M.WFEngine.Task.Job
{
    public interface IWorkflowJobs
    {
        IEnumerable<IJob> Jobs { get; set; }

        IEnumerable<IJobModel> JobModels { get; set; }

        /// <summary>
        /// 流程获取业务数据的job
        /// </summary>
        IEnumerable<IGetBisDataForWorkflowJob> GetBisDataForWorkflowJobs { get; set; }
    }

}
