using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFTinsEntity : DBEntity
    {
        public const string TableCode = "WFTINS";
        public override string _TableCode { get { return TableCode; } }

        private DateTime _Deadline;
        /// <summary>
        /// 办理期限
        /// </summary>
        [DataMember]
        public DateTime Deadline
        {
            get { return _Deadline; }
            set { _Deadline = value; OnPropertyChanged("Deadline"); }
        }

        private DateTime _Sdate;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime Sdate
        {
            get { return _Sdate; }
            set { _Sdate = value; OnPropertyChanged("Sdate"); }
        }

        private DateTime _Edate;
        /// <summary>
        /// 任务完成时间
        /// </summary>
        [DataMember]
        public DateTime Edate
        {
            get { return _Edate; }
            set { _Edate = value; OnPropertyChanged("Edate"); }
        }

        private string _Taskname;
        /// <summary>
        /// 任务节点名称
        /// </summary>
        [DataMember]
        public string Taskname
        {
            get { return _Taskname; }
            set { _Taskname = value; OnPropertyChanged("Taskname"); }
        }

        private string _Memo;
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Memo
        {
            get { return _Memo; }
            set { _Memo = value; OnPropertyChanged("Memo"); }
        }

        private string _Flowid;
        /// <summary>
        /// 工作流Id
        /// </summary>
        [DataMember]
        public string Flowid
        {
            get { return _Flowid; }
            set { _Flowid = value; OnPropertyChanged("Flowid"); }
        }

        private string _Finsid;
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        [DataMember]
        public string Finsid
        {
            get { return _Finsid; }
            set { _Finsid = value; OnPropertyChanged("Finsid"); }
        }

        private string _Taskid;
        /// <summary>
        /// 任务节点Id
        /// </summary>
        [DataMember]
        public string Taskid
        {
            get { return _Taskid; }
            set { _Taskid = value; OnPropertyChanged("Taskid"); }
        }

        private string _Pretaskid;
        /// <summary>
        /// 前任务节点Id
        /// </summary>
        [DataMember]
        public string Pretaskid
        {
            get { return _Pretaskid; }
            set { _Pretaskid = value; OnPropertyChanged("Pretaskid"); }
        }

    }
}//end

