namespace ProjectF.Networks.Packets
{
    public abstract class RequestPacket : PacketBase
    {
        public abstract string Route { get; }
        public abstract string Post { get; }

        public string userID = "";
    }
}