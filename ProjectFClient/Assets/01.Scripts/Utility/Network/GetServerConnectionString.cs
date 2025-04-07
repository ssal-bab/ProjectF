using ProjectF.Networks;

namespace ProjectF
{
    public struct GetServerConnectionString
    {
        public string serverConnection;

        public GetServerConnectionString(EServerConnectionType connectionType)
        {
            serverConnection = connectionType switch {
                EServerConnectionType.Local => "http://localhost:5192",
                EServerConnectionType.Development => "http://seh00n.iptime.org:5959",
                _ => "http://seh00n.iptime.org:5959"
            };
        }
    }
}