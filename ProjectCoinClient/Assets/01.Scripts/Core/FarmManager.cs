using UnityEngine;

namespace ProjectCoin
{
    public class FarmManager : MonoBehaviour
    {
        private static FarmManager instance = null;
        public static FarmManager Instance => instance;

        public void Initialize()
        {
            instance = this;
        }

        public void Release()
        {
            instance = null;
        }
    }
}
