using System.Collections;
using System.Collections.Generic;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.UI.Farms;
using UnityEngine;

namespace ProjectF
{
    public class FarmerManagement : MonoBehaviour
    {
        [SerializeField] private AddressableAsset<FarmerListPopupUI> farmerListPopupUIPrefab;

        private void Start()
        {
            farmerListPopupUIPrefab.Initialize();
        }

        public void OpenFarmerListPopupUI()
        {
            var farmerListPopupUI = PoolManager.Spawn<FarmerListPopupUI>(farmerListPopupUIPrefab.Key, GameDefine.MainPopupFrame);
            farmerListPopupUI.StretchRect();
            farmerListPopupUI.Initialize();
        }
    }
}
