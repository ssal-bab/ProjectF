namespace ProjectCoin.Networks.Payloads
{
    public abstract class RequestPayload : Payload
    {
        public abstract string Route { get; }
        public abstract string Post { get; }

        public string userID = "";
    }
}