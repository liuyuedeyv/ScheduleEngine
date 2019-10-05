using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    public partial class WFTDataEntity : DBEntity
    {
        public const string TableCode = "WFTDATA";
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


        private string _JsonData;
        /// <summary>
        /// 工作流ID
        /// </summary>
        [DataMember]
        public string JsonData
        {
            get { return _JsonData; }
            set { _JsonData = value; OnPropertyChanged("JsonData"); }
        }
    }
}//end

