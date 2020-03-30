using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFOinsEntity : DBEntity
    {
        public const string TableCode = "WFOINS";
        public override string _TableCode { get { return TableCode; } }

        private string _Ocmddis;
        /// <summary>
        /// 处理命令
        /// </summary>
        [DataMember]
        public string Ocmddis
        {
            get { return _Ocmddis; }
            set { _Ocmddis = value; OnPropertyChanged("Ocmddis"); }
        }

        private string _Tag;
        /// <summary>
        /// 标签
        /// </summary>
        [DataMember]
        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; OnPropertyChanged("Tag"); }
        }

        private DateTime _Sdate;
        /// <summary>
        /// 到岗时间
        /// </summary>
        [DataMember]
        public DateTime Sdate
        {
            get { return _Sdate; }
            set { _Sdate = value; OnPropertyChanged("Sdate"); }
        }

        private DateTime _Edate;
        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        public DateTime Edate
        {
            get { return _Edate; }
            set { _Edate = value; OnPropertyChanged("Edate"); }
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

        private DateTime _Lastalert;
        /// <summary>
        /// 上次提醒时间
        /// </summary>
        [DataMember]
        public DateTime Lastalert
        {
            get { return _Lastalert; }
            set { _Lastalert = value; OnPropertyChanged("Lastalert"); }
        }

        private decimal _Alarmnums;
        /// <summary>
        /// 提醒次数
        /// </summary>
        [DataMember]
        public decimal Alarmnums
        {
            get { return _Alarmnums; }
            set { _Alarmnums = value; OnPropertyChanged("Alarmnums"); }
        }

        private string _Userid;
        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember]
        public string Userid
        {
            get { return _Userid; }
            set { _Userid = value; OnPropertyChanged("Userid"); }
        }

        private string _Omemo;
        /// <summary>
        /// 处理意见
        /// </summary>
        [DataMember]
        public string Omemo
        {
            get { return _Omemo; }
            set { _Omemo = value; OnPropertyChanged("Omemo"); }
        }

        private string _Type;
        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged("Type"); }
        }

        private string _Fuserid;
        /// <summary>
        /// 来自于
        /// </summary>
        [DataMember]
        public string Fuserid
        {
            get { return _Fuserid; }
            set { _Fuserid = value; OnPropertyChanged("Fuserid"); }
        }

        private int _Ishasten;
        /// <summary>
        /// 催办标记
        /// </summary>
        [DataMember]
        public int Ishasten
        {
            get { return _Ishasten; }
            set { _Ishasten = value; OnPropertyChanged("Ishasten"); }
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

        private string _Tinsid;
        /// <summary>
        /// 任务节点实例Id
        /// </summary>
        [DataMember]
        public string Tinsid
        {
            get { return _Tinsid; }
            set { _Tinsid = value; OnPropertyChanged("Tinsid"); }
        }

        private string _Ocmd;
        /// <summary>
        /// 处理命令
        /// </summary>
        [DataMember]
        public string Ocmd
        {
            get { return _Ocmd; }
            set { _Ocmd = value; OnPropertyChanged("Ocmd"); }
        }

        private string _Omemotype;
        /// <summary>
        /// 意见类型
        /// </summary>
        [DataMember]
        public string Omemotype
        {
            get { return _Omemotype; }
            set { _Omemotype = value; OnPropertyChanged("Omemotype"); }
        }

    }
}//end

