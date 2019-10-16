using FD.Simple.Utils;
using System.Collections.Generic;

namespace M.WorkFlow.Model
{
    public class WFJsonDataEntity : Dictionary<string, object>
    {
        public string GetStringProperty(string key)
        {
            if (this.ContainsKey(key))
            {
                return this[key].ConvertTostring();
            }
            else
            {
                return string.Empty;
            }
        }

        public int GetIntProperty(string key)
        {
            if (this.ContainsKey(key))
            {
                return this[key].ConvertToint();
            }
            else
            {
                return 0;
            }
        }

        public double GetDoubleProperty(string key)
        {
            if (this.ContainsKey(key))
            {
                return this[key].ConvertTodouble();
            }
            else
            {
                return 0;
            }
        }

        public object GetObjectProperty(string key)
        {
            if (this.ContainsKey(key))
            {
                return this[key];
            }
            else
            {
                return null;
            }
        }
    }
}
