// using System;
// using H00N.DataTables;
// using H00N.Resources;
// using H00N.Resources.Pools;
// using ProjectF.Datas;
// using ProjectF.DataTables;
// using ProjectF.Networks;
// using ProjectFServer.Networks.Packets;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using H00N.Extensions;

// namespace ProjectF.UI.Adventure
// {
    
//     public class AdventureProgressInfoPanel : PoolableBehaviourUI
//     {
//         [SerializeField] private TextMeshProUGUI _progressText;
//         [SerializeField] private Image _areaVisual;
//         [SerializeField] private TextMeshProUGUI _areaNameText;

//         [SerializeField] private AdventureLootItemScroll _lootItemScrollGroup;
//         [SerializeField] private AdventureProgressTimer _progressTimerGroup;

//         public async void Initialize(AdventureData adventureData)
//         {
//             Debug.Log($"{GameInstance.MainUser.adventureData.inAdventureAreaList[adventureData.adventureAreaID]}, {DateTime.Now}");

//             var req = new CheckAdventureProgressRequest(adventureData.adventureAreaID);
//             var response = await NetworkManager.Instance.SendWebRequestAsync<CheckAdventureProgressResponse>(req);

//             if (response.result != ENetworkResult.Success)
//             {
//                 Debug.LogError("CriticalError!");
//             }

//             var isCompleteExplore = response.isCompleteExplore;
            
//             _progressText.text = ResourceUtility.GetAdventureProgressStateKey(isCompleteExplore);
//             //_areaVisual.sprite = ResourceUtility.GetAdventureAreaImageKey(adventureData.adventureAreaID);
//             _areaNameText.text = ResourceUtility.GetAdventureAreaName(adventureData.adventureAreaID);

//             if (isCompleteExplore)
//             {
//                 _lootItemScrollGroup.Active();
//                 _progressTimerGroup.DeActive();

//                 _lootItemScrollGroup.Initialize(adventureData);
//             }
//             else
//             {
//                 _progressTimerGroup.Active();
//                 _lootItemScrollGroup.DeActive();

//                 _progressTimerGroup.Initialize(response.remainTime);
//             }
//         }

//         public void OnTouchCloseButton()
//         {
//             base.Release();
            
//             _lootItemScrollGroup.Release();
//             PoolManager.Despawn(this);
//         }
//     }
// }
