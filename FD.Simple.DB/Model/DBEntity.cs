using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FD.Simple.DB
{
    [DataContract]
    public class DBEntity
    {
        public DBEntity()
        {
            var ss = "";
        }

        public bool InitReady = false;
        public void Add(string sql = null)
        {
            this.InitReady = true;
            this.State = EDBEntityState.Added;
        }
        public void Modify()
        {
            this.InitReady = true;
            this.State = EDBEntityState.Modified;
        }
        public virtual string _TableCode { get; private set; }
        public List<string> ChangeField
        {
            get { return changeField; }
        }
        private List<string> changeField = new List<string>();

        private EDBEntityState _state = EDBEntityState.UnChanged;
        [DataMember]
        public EDBEntityState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                if (value > EDBEntityState.UnChanged)
                {
                    this.InitReady = true;
                }
            }
        }

        public string Suffix { get; set; }

        private string id;

        [DataMember]
        public string ExField { get; set; }

        /// <summary>
        /// 用来存储关联的代码列数据
        /// </summary>
        [DataMember]
        public IDictionary<string, string> RefData { get; set; } = new Dictionary<string, string>();

        public void AddRef(string colName, string value)
        {
            if (this.RefData.ContainsKey(colName))
            {
                this.RefData[colName] = value;
            }
            else
            {
                this.RefData.Add(colName, value);
            }
        }

        [DataMember]
        public string ID
        {
            get { return id; }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (!InitReady)
            {
                return;
            }
            if (!ChangeField.Contains(propertyName))
            {
                ChangeField.Add(propertyName);
            }
            if (this.State == EDBEntityState.UnChanged)
            {
                this.State = EDBEntityState.Modified;
            }
        }
    }
}