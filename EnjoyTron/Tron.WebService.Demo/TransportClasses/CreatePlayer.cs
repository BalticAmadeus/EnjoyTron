using System.Runtime.Serialization;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class CreatePlayerReq : BaseReq
    {
        // empty
    }

    [DataContract]
    public class CreatePlayerResp : BaseResp
    {
        [DataMember]
        public int PlayerId;
    }
}
