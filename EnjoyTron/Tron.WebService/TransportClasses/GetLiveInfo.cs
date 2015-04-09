using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class GetLiveInfoReq : BaseReq
    {
        [DataMember]
        public int GameId;
    }

    [DataContract]
    public class GetLiveInfoResp : BaseResp
    {
        [DataMember]
        public EnGameLiveInfo GameLiveInfo;
    }
}