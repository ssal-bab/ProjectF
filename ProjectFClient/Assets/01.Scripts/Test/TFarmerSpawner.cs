using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Farms;
using UnityEngine;

namespace ProjectF.Tests
{
    public class TFarmerSpawner : MonoBehaviour
    {
        [SerializeField] AddressableAsset<Farmer> farmerPrefab = null;

        private void Awake()
        {
            farmerPrefab.Initialize();
        }

        private void Start()
        {
            Farmer farmer = PoolManager.Spawn(farmerPrefab.Key) as Farmer;
            farmer?.InitializeAsync(0);
        }
    }
}
