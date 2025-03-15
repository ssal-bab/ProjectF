using System;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class LoginRequest : RequestPacket
    {
        public override string Route => NetworkDefine.STANDARD_ROUTE;

        public const string POST = "login";
        public override string Post => POST;

        public LoginRequest()
        {
        }
    }

    public class LoginResponse : ResponsePacket
    {
        public UserData userData = null;
        public DateTime serverTime = DateTime.MinValue;
    }
}