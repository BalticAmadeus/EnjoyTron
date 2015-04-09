using System.Runtime.Serialization;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class GetGameDetailsReq : BaseReq
    {
        [DataMember]
        public int GameId;
    }

    [DataContract]
    public class GetGameDetailsResp : BaseResp
    {
        [DataMember]
        public EnGameDetails GameDetails;
    }
}
