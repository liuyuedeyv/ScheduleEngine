using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFServiceEntity : DBEntity
    {
        public const string TableCode = "WFSERVICE";
        public override string _TableCode { get { return TableCode; } }

        private DateTime _Cdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime Cdate
        {
            get { return _Cdate; }
            set { _Cdate = value; OnPropertyChanged("Cdate"); }
        }

        private string _Cuser;
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public string Cuser
        {
            get { return _Cuser; }
            set { _Cuser = value; OnPropertyChanged("Cuser"); }
        }


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


        private int _Orderid;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Orderid
        {
            get { return _Orderid; }
            set { _Orderid = value; OnPropertyChanged("Orderid"); }
        }


        private string _Currentflowid;
        /// <summary>
        /// 当前id
        /// </summary>
        [DataMember]
        public string Currentflowid
        {
            get { return _Currentflowid; }
            set { _Currentflowid = value; OnPropertyChanged("Currentflowid"); }
        }

        private string _WfappId;
        /// <summary>
        /// _WfappId
        /// </summary>
        [DataMember]
        public string WfappId
        {
            get { return _WfappId; }
            set { _WfappId = value; OnPropertyChanged("WfappId"); }
        }
    }
}//end

