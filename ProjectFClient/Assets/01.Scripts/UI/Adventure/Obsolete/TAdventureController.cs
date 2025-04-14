// using System.Collections;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using H00N.DataTables;
// using H00N.Resources;
// using H00N.Resources.Pools;
// using ProjectF.Datas;
// using ProjectF.DataTables;
// using ProjectF.UI.Adventure;
// using UnityEngine;

// namespace ProjectF.UI.Adventure
// {
//     public class TAdventureController : MonoBehaviour
//     {
//         [SerializeField] private AddressableAsset<AdventurePopupUI> adventurePopupUI;
//         [SerializeField] private AddressableAsset<AdventureProgressInfoPanel> adventureProgressPopupUI;
//         public async void OpenUI()
//         {
//             await adventurePopupUI.InitializeAsync();
//             await adventureProgressPopupUI.InitializeAsync();

//             AdventureLevelTable table = DataTableManager.GetTable<AdventureLevelTable>();
//             int id = table.GetRow(0).id;

//             // 임시 생성
//             AdventureData data = new AdventureData();

//             if (!GameInstance.MainUser.adventureData.inAdventureAreaList.ContainsKey(id))
//             {
//                 var ui = PoolManager.Spawn(adventurePopupUI, GameDefine.MainPopupFrame);
//                 ui.StretchRect();
//                 ui.Initialize(data);
//             }
//             else
//             {
//                 var ui = PoolManager.Spawn(adventureProgressPopupUI, GameDefine.MainPopupFrame);
//                 ui.StretchRect();
//                 ui.Initialize(data);
//             }

//         }
//     }
// }
