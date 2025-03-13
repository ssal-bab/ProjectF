using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Farms;
using ProjectF.UI.Farms;
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
            Farmer farmer = PoolManager.Spawn<Farmer>(farmerPrefab.Key);
            farmer?.InitializeAsync(0);
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.LeftControl) == false)
                return;

            if(Input.GetKeyDown(KeyCode.P))
            {
                StoragePopupUI storagePopupUI = PoolManager.Spawn<StoragePopupUI>("StoragePopupUI", GameDefine.MainPopupFrame);
                storagePopupUI.StretchRect();
                storagePopupUI.Initialize();
            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                NestPopupUI nestPopupUI = PoolManager.Spawn<NestPopupUI>("NestPopupUI", GameDefine.MainPopupFrame);
                nestPopupUI.StretchRect();
                nestPopupUI.Initialize();
            }
        }
    }
}
