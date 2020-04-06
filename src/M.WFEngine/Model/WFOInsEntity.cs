using System;
using System.Collections.Generic;
using System.Text;

namespace M.WFEngine.Model
{
    public class WFOInsEntity
    {
        public string Id { get; set; }

        public string FlowId { get; set; }

        public string FinsId { get; set; }

        public string TaskId { get; set; }

        public string TinsId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime CDate { get; set; }

        public int Status { get; set; }

        public DateTime ProcessDate { get; set; }

    }
}
