using System.Runtime.Serialization;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class StartGameReq : BaseReq
    {
        [DataMember]
        public int GameId;
    }

    [DataContract]
    public class StartGameResp : BaseResp
    {
        // default
    }
}
