// using Cysharp.Threading.Tasks;
// using H00N.Resources;
// using H00N.Resources.Pools;
// using ProjectF.UI.Farms;
// using UnityEngine;

// namespace ProjectF
// {
//     public class FarmerManagement : MonoBehaviour
//     {
//         [SerializeField] private AddressableAsset<FarmerListPopupUI> farmerListPopupUIPrefab;

//         private void Start()
//         {
//             farmerListPopupUIPrefab.InitializeAsync().Forget();
//         }

//         public async void OpenFarmerListPopupUI()
//         {
//             await UniTask.WaitUntil(() => farmerListPopupUIPrefab.Initialized);

//             var farmerListPopupUI = PoolManager.Spawn<FarmerListPopupUI>(farmerListPopupUIPrefab, GameDefine.MainPopupFrame);
//             farmerListPopupUI.StretchRect();
//             farmerListPopupUI.Initialize();
//         }
//     }
// }
