using ProjectF.Datas;
using System.Collections.Generic;

namespace ProjectF.Networks.Packets
{
    public class UpdateFarmRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "UpdateFarm";
        public override string Post => POST;

        public Dictionary<int, FieldGroupData> fieldGroupDatas;

        public UpdateFarmRequest(Dictionary<int, FieldGroupData> fieldGroupDatas)
        {
            this.fieldGroupDatas = fieldGroupDatas;
        }
    }

    public class UpdateFarmResponse : ResponsePacket
    {

    }
}