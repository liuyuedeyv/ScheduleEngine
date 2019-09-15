using System.Runtime.Serialization;

namespace FD.Simple.DB
{
    [DataContract]
    public enum EColType
    {
        NVARCHAR = 231,
        VARCHAR = 167,
        NUMERIC = 108,
        DATETIME = 61
    }
}