using System;
using System.Runtime.Serialization;
using FD.Simple.DB;
namespace M.WorkFlow.Model
{
    [Serializable]
    public partial class WFTaskSettingEntity : DBEntity
    {
        public const string TableCode = "WFTASKSETTING";
        public override string _TableCode { get { return TableCode; } }

        private string _DataTemplate;
        /// <summary>
        /// 业务数据模板
        /// </summary>
        [DataMember]
        public string DataTemplate
        {
            get { return _DataTemplate; }
            set { _DataTemplate = value; OnPropertyChanged("DataTemplate"); }
        }

        private string _Jobs;
        /// <summary>
        /// 节点名称
        /// </summary>
        [DataMember]
        public string Jobs
        {
            get { return _Jobs; }
            set { _Jobs = value; OnPropertyChanged("Jobs"); }
        }
    }
}

