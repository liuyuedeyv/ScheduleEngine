using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFLinkEntity : DBEntity
    {
        public const string TableCode = "WFLINK";
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

        private string _Begintaskid;
        /// <summary>
        /// 开始节点ID
        /// </summary>
        [DataMember]
        public string Begintaskid
        {
            get { return _Begintaskid; }
            set { _Begintaskid = value; OnPropertyChanged("Begintaskid"); }
        }

        private string _Endtaskid;
        /// <summary>
        /// 结束节点ID
        /// </summary>
        [DataMember]
        public string Endtaskid
        {
            get { return _Endtaskid; }
            set { _Endtaskid = value; OnPropertyChanged("Endtaskid"); }
        }

        private int _Priority;
        /// <summary>
        /// 优先级
        /// </summary>
        [DataMember]
        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; OnPropertyChanged("Priority"); }
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

        private string _Linetype;
        /// <summary>
        /// 流转类型
        /// </summary>
        [DataMember]
        public string Linetype
        {
            get { return _Linetype; }
            set { _Linetype = value; OnPropertyChanged("Linetype"); }
        }

        private string _Xml;
        /// <summary>
        /// 流转XML
        /// </summary>
        [DataMember]
        public string Xml
        {
            get { return _Xml; }
            set { _Xml = value; OnPropertyChanged("Xml"); }
        }

        private string _Filter;
        /// <summary>
        /// 流转条件
        /// </summary>
        [DataMember]
        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; OnPropertyChanged("Filter"); }
        }

        private string _Updatefield;
        /// <summary>
        /// 更新字段
        /// </summary>
        [DataMember]
        public string Updatefield
        {
            get { return _Updatefield; }
            set { _Updatefield = value; OnPropertyChanged("Updatefield"); }
        }

    }
}//end

