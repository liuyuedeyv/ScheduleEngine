using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFFlowEntity : DBEntity
    {
        public const string TableCode = "WFFLOW";
        public override string _TableCode { get { return TableCode; } }

        private int _Orderid;
        /// <summary>
        /// 排序id
        /// </summary>
        [DataMember]
        public int Orderid
        {
            get { return _Orderid; }
            set { _Orderid = value; OnPropertyChanged("Orderid"); }
        }

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        private string _Filter;
        /// <summary>
        /// filter
        /// </summary>
        [DataMember]
        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; OnPropertyChanged("Filter"); }
        }

        private string _Xml;
        /// <summary>
        /// 流程数据
        /// </summary>
        [DataMember]
        public string Xml
        {
            get { return _Xml; }
            set { _Xml = value; OnPropertyChanged("Xml"); }
        }

        private string _Workid;
        /// <summary>
        /// 工作流业务ID
        /// </summary>
        [DataMember]
        public string Workid
        {
            get { return _Workid; }
            set { _Workid = value; OnPropertyChanged("Workid"); }
        }

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

    }
}//end

