using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    public partial class WFTEventMsgEntity : DBEntity
    {
        public const string TableCode = " WFTEventMsg";
        public override string _TableCode { get { return TableCode; } }

        private string _EventId;
        /// <summary>
        /// 异步节点EventID
        /// </summary>
        [DataMember]
        public string EventId
        {
            get { return _EventId; }
            set { _EventId = value; OnPropertyChanged("EventId"); }
        }

        private string _FinsId;

        /// <summary>
        /// 一级经销商
        /// </summary>
        [DataMember]
        public string FinsId
        {
            get { return _FinsId; }
            set
            {
                _FinsId = value; OnPropertyChanged("FinsId");
            }
        }
        private string _Remark;
        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; OnPropertyChanged("Remark"); }
        }

        private DateTime _CDate;
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CDate
        {
            get { return _CDate; }
            set { _CDate = value; OnPropertyChanged("CDate"); }
        }

        private DateTime _RepairDate;

        /// <summary>
        /// 修复时间
        /// </summary>
        [DataMember]
        public DateTime RepairDate
        {
            get { return _RepairDate; }
            set
            {
                _RepairDate = value;
                OnPropertyChanged("RepairDate");
            }
        }
         
    }
}//end

