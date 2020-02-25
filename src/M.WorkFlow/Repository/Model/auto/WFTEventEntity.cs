using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    public partial class WFTEventEntity : DBEntity
    {
        public const string TableCode = "WFTEVENT";
        public override string _TableCode { get { return TableCode; } }

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
        /// 工作流ID
        /// </summary>
        [DataMember]
        public string Finsid
        {
            get { return _Finsid; }
            set { _Finsid = value; OnPropertyChanged("Finsid"); }
        }
        private string _DataId;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public string Dataid
        {
            get { return _DataId; }
            set { _DataId = value; OnPropertyChanged("Dataid"); }
        }

        private string _Taskid;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public string Taskid
        {
            get { return _Taskid; }
            set { _Taskid = value; OnPropertyChanged("Taskid"); }
        }
        private string _Tinsid;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public string Tinsid
        {
            get { return _Tinsid; }
            set { _Tinsid = value; OnPropertyChanged("Tinsid"); }
        }

        private int _Status;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged("Status"); }
        }
        private int _Waitcallback;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public int Waitcallback
        {
            get { return _Waitcallback; }
            set { _Waitcallback = value; OnPropertyChanged("Waitcallback"); }
        }


        private DateTime _Cdate;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public DateTime Cdate
        {
            get { return _Cdate; }
            set { _Cdate = value; OnPropertyChanged("Cdate"); }
        }


        private DateTime _ProcessDate;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public DateTime ProcessDate
        {
            get { return _ProcessDate; }
            set { _ProcessDate = value; OnPropertyChanged("ProcessDate"); }
        }


        private DateTime _Callbackdate;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public DateTime Callbackdate
        {
            get { return _Callbackdate; }
            set { _Callbackdate = value; OnPropertyChanged("Callbackdate"); }
        }
    }
}//end

