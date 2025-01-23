using System;

namespace ProjectCoin.Networks
{
    public class Connection
    {
        private string connectionExpression = "";
        public string ConnectionExpression => connectionExpression;

        private bool isConnectionAlive = false;
        public bool IsConnectionAlive => isConnectionAlive;

        public event Action<bool> OnConnectionChanged = null;

        public Connection(string expression)
        {
            connectionExpression = expression;
        }

        public virtual void CheckConnection()
        {
            SetConnection(true);
        }

        protected void SetConnection(bool connection)
        {
            bool prevState = isConnectionAlive;

            isConnectionAlive = connection;
            if (prevState == isConnectionAlive)
                return;

            OnConnectionChanged?.Invoke(isConnectionAlive);
        }
    }
}