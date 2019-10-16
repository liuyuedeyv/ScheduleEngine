using M.ACSA.ResponseService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace M.ACSA.AccessService
{
    public static class WfServiceMessageExtensions
    {
        public static BaseWfServiceMessage GetServiceMessageInstance(this IEnumerable<IWfServiceMessage> serviceMessages, MessageModel info)
        {
            //如果是流程获取业务数据消息
            if (info.MsgType == EResponseMessageType.GetWorkflowServiceBisdata)
            {
                foreach (BaseWfServiceMessage service in serviceMessages)
                {
                    if (service.WFServcieId == info.ServiceId && service.AccessMessageType == EResponseMessageType.GetWorkflowServiceBisdata)
                    {
                        return service;
                    }
                }
            }
            else if (info.MsgType == EResponseMessageType.GetModelTaskBisdata)
            {
                foreach (BaseWfServiceMessage service in serviceMessages)
                {
                    if (service.AccessMessageType == EResponseMessageType.GetModelTaskBisdata && service.ObjectCode == info.ObjectCode)
                    {
                        return service;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 检查自定义对象是否有编码重复
        /// </summary>
        /// <param name="serviceMessages"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool CheckServicesetting(this IEnumerable<IWfServiceMessage> serviceMessages, out string errorMsg)
        {
            errorMsg = string.Empty;
            bool isRepeat = false;

            List<string> listObjcode = new List<string>();
            foreach (BaseWfServiceMessage service in serviceMessages)
            {
                if (listObjcode.Contains(service.ObjectCode))
                {
                    isRepeat = true;
                    errorMsg = $"对象编码{service.ObjectCode}重复，请检查 ";
                    break;
                }
                listObjcode.Add(service.ObjectCode);
            }

            return !isRepeat;
        }
    }
}
