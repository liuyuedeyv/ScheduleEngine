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

        private int _Version;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Version
        {
            get { return _Version; }
            set { _Version = value; OnPropertyChanged("Version"); }
        }


        private int _Released;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Released
        {
            get { return _Released; }
            set { _Released = value; OnPropertyChanged("Released"); }
        }

        private DateTime _ReleaseDate;
        /// <summary>
        /// 发布时间
        /// </summary>
        [DataMember]
        public DateTime ReleaseDate
        {
            get { return _ReleaseDate; }
            set { _ReleaseDate = value; OnPropertyChanged("ReleaseDate"); }
        }

        private string _ServiceId;
        /// <summary>
        /// 业务ID
        /// </summary>
        [DataMember]
        public string ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; OnPropertyChanged("ServiceId"); }
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

