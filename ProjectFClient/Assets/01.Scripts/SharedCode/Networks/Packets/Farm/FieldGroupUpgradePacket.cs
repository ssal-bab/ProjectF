
namespace ProjectF.Networks.Packets
{
    public class FieldGroupUpgradeRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "FieldGroupUpgrade";
        public override string Post => POST;

        public int fieldGroupID;

        public FieldGroupUpgradeRequest(int fieldGroupID) 
        { 
            this.fieldGroupID = fieldGroupID;
        }
    }

    public class FieldGroupUpgradeResponse : ResponsePacket
    {
        public int upgradedFieldGroupID;
        public int currentLevel;
    }
}