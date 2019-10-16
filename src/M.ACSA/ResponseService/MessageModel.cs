using FD.Simple.Utils.Agent;

namespace M.ACSA.ResponseService
{
    public class MessageModel: IFooParameter
    {
        public string ServiceId { get; set; }
        public string DataId { get; set; }
        public EResponseMessageType MsgType { get; set; }
        public string ObjectCode { get; set; }
    }
}
