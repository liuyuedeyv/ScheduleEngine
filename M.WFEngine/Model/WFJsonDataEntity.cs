using FD.Simple.Utils;
using System.Collections.Generic;

namespace M.WFEngine.Model
{
    public class WFJsonDataEntity : Dictionary<string, object>
    {
        public string GetStringProperty(string key)
        {
            return this[key].ConvertTostring();
        }

        public int GetIntProperty(string key)
        {
            return this[key].ConvertToint();
        }

        public double GetDoubleProperty(string key)
        {
            return this[key].ConvertTodouble();
        }

        public object GetObjectProperty(string key)
        {
            return this[key];
        }
    }
}
