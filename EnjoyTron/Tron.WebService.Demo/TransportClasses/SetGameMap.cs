using System.Runtime.Serialization;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class SetGameMapReq : BaseReq
    {
        [DataMember]
        public int GameId;

        [DataMember]
        public EnMapData MapData;
    }

    [DataContract]
    public class SetGameMapResp : BaseResp
    {
        // default
    }
}
