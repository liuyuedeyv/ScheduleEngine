using FD.Simple.DB;
using FD.Simple.Utils.Agent;
using System.Runtime.Serialization;

namespace M.WorkFlow.Model
{
    public partial class WFTaskEntity : DBEntity, IFooParameter
    {
        private ETaskType _Type;
        /// <summary>
        /// 节点类型
        /// </summary>
        [DataMember]
        public ETaskType Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged("Type"); }
        }

        public WFTaskSettingEntity SettingEntity { get; set; }
    }
}
