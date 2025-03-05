using System;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class SellCropRequest : RequestPacket
    {
        public override string Route => NetworkDefine.STANDARD_ROUTE;

        public const string POST = "SellCrop";
        public override string Post => POST;

        public int id;
        public ECropGrade grade;

        public SellCropRequest(int id, ECropGrade grade) 
        { 
            this.id = id;
            this.grade = grade;
        }
    }

    public class SellCropResponse : ResponsePacket
    {
        public int earnedGold;
        public int soldCropCount;
    }
}