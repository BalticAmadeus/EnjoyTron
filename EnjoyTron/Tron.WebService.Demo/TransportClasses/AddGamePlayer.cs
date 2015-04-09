using System.Runtime.Serialization;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class AddGamePlayerReq : BaseReq
    {
        [DataMember]
        public int GameId;

        [DataMember]
        public int PlayerId;
    }

    [DataContract]
    public class AddGamePlayerResp : BaseResp
    {
        // default
    }
}
