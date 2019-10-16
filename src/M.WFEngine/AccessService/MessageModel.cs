namespace M.WFEngine.AccessService
{
    public class MessageModel
    {
        public string ServiceId { get; set; }
        public string DataId { get; set; }
        public EAccessMessageType MsgType { get; set; }
        public string ObjectCode { get; set; }
    }
}
