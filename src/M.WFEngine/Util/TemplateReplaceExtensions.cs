using M.WorkFlow.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Linq;
namespace M.WFEngine.Util
{
    static class TemplateReplaceExtensions
    {
        const string regStr = "{@[^}]*}";
        /// <summary>
        /// 替换模板中的 {@.*}信息
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <param name="jsonData"></param>
        public static void ReplaceTemplateInfo(this WFTaskEntity taskEntity, string jsonData)
        {
            if (!string.IsNullOrWhiteSpace(taskEntity.Setting) && !string.IsNullOrWhiteSpace(jsonData))
            {
                var reg = new Regex(regStr);
                // 匹配模板： "Exchange:{@Exchange},ServerName:{@sserverName}";
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(taskEntity.Setting);
                foreach (var item in reg.Matches(taskEntity.Setting))
                {
                    var tmp = item.ToString();
                    if (obj.TryGetValue(tmp, StringComparison.CurrentCultureIgnoreCase, out JToken token))
                    {
                        taskEntity.Setting = taskEntity.Setting.Replace(tmp, token.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 判断是否匹配  {@.*}
        /// </summary>
        /// <param name="wFTaskEntity"></param>
        /// <returns></returns>
        public static bool HasTemplateInfo(this WFTaskEntity wFTaskEntity)
        {
            if (string.IsNullOrWhiteSpace(wFTaskEntity.Setting))
            {
                return false;
            }
            var reg = new Regex(regStr);
            return reg.IsMatch(wFTaskEntity.Setting);
        }
        /// <summary>
        /// 获取变量  {@.*}，并且用逗号隔开拼接到一起
        /// </summary>
        /// <param name="wFTaskEntity"></param>
        /// <returns></returns>
        public static string GetTemplateInfo(this WFTaskEntity wFTaskEntity)
        {
            if (string.IsNullOrWhiteSpace(wFTaskEntity.Setting))
            {
                return string.Empty;
            }
            var reg = new Regex(regStr);
            string result = string.Empty;
            foreach (var item in reg.Matches(wFTaskEntity.Setting))
            {
                result += item.ToString() + ",";
            }
            return result.TrimEnd(',');
        }
    }
}
