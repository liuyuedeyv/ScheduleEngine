using FD.Simple.Utils.Agent;
using System.Collections.Generic;

namespace M.WFEngine.Task.Job
{
    [Autowired]
    public class WorkflowJobs : IWorkflowJobs
    {
        [Autowired]
        public IEnumerable<IJob> Jobs { get; set; }
        [Autowired]
        public IEnumerable<IJobModel> JobModels { get; set; }
        [Autowired]
        public IEnumerable<IGetBisDataForWorkflowJob> GetBisDataForWorkflowJobs { get; set; }
    }
}
