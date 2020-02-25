using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFFinsEntity : DBEntity
    {
        public const string TableCode = "WFFINS";
        public override string _TableCode { get { return TableCode; } }

        private string _Name;
        /// <summary>
        /// 申请内容
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        private int _Status;
        /// <summary>
        /// 流程状态
        /// </summary>
        [DataMember]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged("Status"); }
        }

        private string _Ctaskid;
        /// <summary>
        /// 当前任务节点
        /// </summary>
        [DataMember]
        public string Ctaskid
        {
            get { return _Ctaskid; }
            set { _Ctaskid = value; OnPropertyChanged("Ctaskid"); }
        }

        private string _Ctaskname;
        /// <summary>
        /// 当前任务节点实例
        /// </summary>
        [DataMember]
        public string Ctaskname
        {
            get { return _Ctaskname; }
            set { _Ctaskname = value; OnPropertyChanged("Ctaskname"); }
        }

        private string _Flowid;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public string Flowid
        {
            get { return _Flowid; }
            set { _Flowid = value; OnPropertyChanged("Flowid"); }
        }

        private string _Dataid;
        /// <summary>
        /// 业务表的主键字段值
        /// </summary>
        [DataMember]
        public string Dataid
        {
            get { return _Dataid; }
            set { _Dataid = value; OnPropertyChanged("Dataid"); }
        }

        private string _Ctaskmemo;
        /// <summary>
        /// 任务节点
        /// </summary>
        [DataMember]
        public string Ctaskmemo
        {
            get { return _Ctaskmemo; }
            set { _Ctaskmemo = value; OnPropertyChanged("Ctaskmemo"); }
        }

        private string _Cmuser;
        /// <summary>
        /// 当前处理人
        /// </summary>
        [DataMember]
        public string Cmuser
        {
            get { return _Cmuser; }
            set { _Cmuser = value; OnPropertyChanged("Cmuser"); }
        }

        private string _ServiceId;
        /// <summary>
        /// 业务模板ID
        /// </summary>
        [DataMember]
        public string ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; OnPropertyChanged("ServiceId"); }
        }

        private string _Monitor;
        /// <summary>
        /// 监控人
        /// </summary>
        [DataMember]
        public string Monitor
        {
            get { return _Monitor; }
            set { _Monitor = value; OnPropertyChanged("Monitor"); }
        }

        private string _Workname;
        /// <summary>
        /// 流程名称
        /// </summary>
        [DataMember]
        public string Workname
        {
            get { return _Workname; }
            set { _Workname = value; OnPropertyChanged("Workname"); }
        }

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

        private DateTime _Officaldate;
        /// <summary>
        /// 流程最终期限
        /// </summary>
        [DataMember]
        public DateTime Officaldate
        {
            get { return _Officaldate; }
            set { _Officaldate = value; OnPropertyChanged("Officaldate"); }
        }

        private string _Cuser;
        /// <summary>
        /// 启动人
        /// </summary>
        [DataMember]
        public string Cuser
        {
            get { return _Cuser; }
            set { _Cuser = value; OnPropertyChanged("Cuser"); }
        }

        private DateTime _Cdate;
        /// <summary>
        /// 申请时间
        /// </summary>
        [DataMember]
        public DateTime Cdate
        {
            get { return _Cdate; }
            set { _Cdate = value; OnPropertyChanged("Cdate"); }
        }

        private DateTime _Edate;
        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime Edate
        {
            get { return _Edate; }
            set { _Edate = value; OnPropertyChanged("Edate"); }
        }

        private int _Lockflag;
        /// <summary>
        /// 锁定标记
        /// </summary>
        [DataMember]
        public int Lockflag
        {
            get { return _Lockflag; }
            set { _Lockflag = value; OnPropertyChanged("Lockflag"); }
        }

        private int _Topflag;
        /// <summary>
        /// 置顶标记
        /// </summary>
        [DataMember]
        public int Topflag
        {
            get { return _Topflag; }
            set { _Topflag = value; OnPropertyChanged("Topflag"); }
        }

    }
}//end

