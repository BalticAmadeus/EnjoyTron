using System.Runtime.Serialization;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class PerformMoveReq : BaseReq
    {
        [DataMember]
        public int PlayerId;

        [DataMember(IsRequired=true)]
        public EnPoint Position;
    }

    [DataContract]
    public class PerformMoveResp : BaseResp
    {
        // default
    }
}
