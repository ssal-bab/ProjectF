using ProjectF.Datas;
using System.Collections.Generic;

namespace ProjectF.Networks.Packets
{
    public class UpdateFarmRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "update_farm";
        public override string Post => POST;

        public Dictionary<int, Dictionary<int, FieldData>> dirtiedFields = null;

        public UpdateFarmRequest(Dictionary<int, Dictionary<int, FieldData>> dirtiedFields)
        {
            this.dirtiedFields = dirtiedFields;
        }
    }

    public class UpdateFarmResponse : ResponsePacket
    {

    }
}