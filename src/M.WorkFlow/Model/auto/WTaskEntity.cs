using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFTaskEntity : DBEntity
    {
        public const string TableCode = "WFTASK";
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

        private string _Name;
        /// <summary>
        /// 节点名称
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        //private string _Type;
        ///// <summary>
        ///// 节点类型
        ///// </summary>
        //[DataMember]
        //public string Type
        //{
        //    get { return _Type; }
        //    set { _Type = value; OnPropertyChanged("Type"); }
        //}

        private string _Setting;
        /// <summary>
        /// settingjson
        /// </summary>
        [DataMember]
        public string Setting
        {
            get { return _Setting; }
            set { _Setting = value; OnPropertyChanged("Setting"); }
        }

        private string _Datatemplate;
        /// <summary>
        /// Datatemplate
        /// </summary>
        [DataMember]
        public string Datatemplate
        {
            get { return _Datatemplate; }
            set { _Datatemplate = value; OnPropertyChanged("Datatemplate"); }
        }

        private string _Filter;
        /// <summary>
        /// 处理人FILTER
        /// </summary>
        [DataMember]
        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; OnPropertyChanged("Filter"); }
        }

        private string _Colex;
        /// <summary>
        /// 可处理字段
        /// </summary>
        [DataMember]
        public string Colex
        {
            get { return _Colex; }
            set { _Colex = value; OnPropertyChanged("Colex"); }
        }

        private string _Approve;
        /// <summary>
        /// 审批意见格式
        /// </summary>
        [DataMember]
        public string Approve
        {
            get { return _Approve; }
            set { _Approve = value; OnPropertyChanged("Approve"); }
        }

        private string _Opertype;
        /// <summary>
        /// 处理方式
        /// </summary>
        [DataMember]
        public string Opertype
        {
            get { return _Opertype; }
            set { _Opertype = value; OnPropertyChanged("Opertype"); }
        }

        private string _Memo;
        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public string Memo
        {
            get { return _Memo; }
            set { _Memo = value; OnPropertyChanged("Memo"); }
        }

        private string _Y;
        /// <summary>
        /// TOP
        /// </summary>
        [DataMember]
        public string Y
        {
            get { return _Y; }
            set { _Y = value; OnPropertyChanged("Y"); }
        }

        private string _X;
        /// <summary>
        /// LEFT
        /// </summary>
        [DataMember]
        public string X
        {
            get { return _X; }
            set { _X = value; OnPropertyChanged("X"); }
        }

        private string _Baseid;
        /// <summary>
        /// baseid
        /// </summary>
        [DataMember]
        public string Baseid
        {
            get { return _Baseid; }
            set { _Baseid = value; OnPropertyChanged("Baseid"); }
        }

    }
}//end

