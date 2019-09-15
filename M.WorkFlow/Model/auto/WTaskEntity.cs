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

        private string _Xml;
        /// <summary>
        /// 处理人XML
        /// </summary>
        [DataMember]
        public string Xml
        {
            get { return _Xml; }
            set { _Xml = value; OnPropertyChanged("Xml"); }
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

        private string _Exscript;
        /// <summary>
        /// 脚本扩展
        /// </summary>
        [DataMember]
        public string Exscript
        {
            get { return _Exscript; }
            set { _Exscript = value; OnPropertyChanged("Exscript"); }
        }

        private int _Mandate;
        /// <summary>
        /// 任务期限（天）
        /// </summary>
        [DataMember]
        public int Mandate
        {
            get { return _Mandate; }
            set { _Mandate = value; OnPropertyChanged("Mandate"); }
        }

        private string _Remind;
        /// <summary>
        /// 提醒内容
        /// </summary>
        [DataMember]
        public string Remind
        {
            get { return _Remind; }
            set { _Remind = value; OnPropertyChanged("Remind"); }
        }

        private string _Overremind;
        /// <summary>
        /// 超期提醒
        /// </summary>
        [DataMember]
        public string Overremind
        {
            get { return _Overremind; }
            set { _Overremind = value; OnPropertyChanged("Overremind"); }
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

        private string _Ctgridinfo;
        /// <summary>
        /// 从表扩展
        /// </summary>
        [DataMember]
        public string Ctgridinfo
        {
            get { return _Ctgridinfo; }
            set { _Ctgridinfo = value; OnPropertyChanged("Ctgridinfo"); }
        }

        private string _Formid;
        /// <summary>
        /// 表单
        /// </summary>
        [DataMember]
        public string Formid
        {
            get { return _Formid; }
            set { _Formid = value; OnPropertyChanged("Formid"); }
        }

        private string _Acrequired;
        /// <summary>
        /// 审批意见必填
        /// </summary>
        [DataMember]
        public string Acrequired
        {
            get { return _Acrequired; }
            set { _Acrequired = value; OnPropertyChanged("Acrequired"); }
        }

        private int _Updateagent;
        /// <summary>
        /// 编写审批报告
        /// </summary>
        [DataMember]
        public int Updateagent
        {
            get { return _Updateagent; }
            set { _Updateagent = value; OnPropertyChanged("Updateagent"); }
        }

        private string _Moduleid;
        /// <summary>
        /// 评价模型
        /// </summary>
        [DataMember]
        public string Moduleid
        {
            get { return _Moduleid; }
            set { _Moduleid = value; OnPropertyChanged("Moduleid"); }
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

        private decimal _Orderid;
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public decimal Orderid
        {
            get { return _Orderid; }
            set { _Orderid = value; OnPropertyChanged("Orderid"); }
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

