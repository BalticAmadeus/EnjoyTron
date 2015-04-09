using System.Runtime.Serialization;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class CreateGameReq : BaseReq
    {
        // empty
    }

    [DataContract]
    public class CreateGameResp : BaseResp
    {
        [DataMember]
        public EnGameInfo GameInfo;
    }
}
