// using H00N.Extensions;
// using H00N.Resources;
// using H00N.Resources.Pools;
// using ProjectF.Datas;
// using ProjectF.Networks;
// using ProjectFServer.Networks.Packets;
// using UnityEngine;

// namespace ProjectF.UI.Adventure
// {
//     public class AdventureLootItemScroll : AdventureProgressContentUI
//     {
//         [SerializeField] private GameObject lootItemScroll;
//         [SerializeField] private GameObject recieveButton;
//         [SerializeField] private RectTransform content;
//         [SerializeField] private AddressableAsset<AdventureLootItemIcon> lootItemIconUIPrefab;

//         private AdventureData adventureData;

//         public async void Initialize(AdventureData adventureData)
//         {
//             this.adventureData = adventureData;
//             await lootItemIconUIPrefab.InitializeAsync();

//             var req = new AdventureResultRequest(adventureData.adventureAreaID);
//             var response = await NetworkManager.Instance.SendWebRequestAsync<AdventureResultResponse>(req);

//             if (response.result != ENetworkResult.Success)
//             {
//                 Debug.LogError("Critical Error!");
//                 return;
//             }

//             foreach (var data in response.resultPack.materialLootInfo)
//             {
//                 var lootItemIcon = PoolManager.Spawn(lootItemIconUIPrefab, content);
//                 lootItemIcon.Initialize(ResourceUtility.GetMaterialIconKey(data.rewardItemID), data.rewardItemAmount);
//                 lootItemIcon.StretchRect();
//             }

//             foreach (var data in response.resultPack.seedLootInfo)
//             {
//                 var lootItemIcon = PoolManager.Spawn(lootItemIconUIPrefab, content);
//                 lootItemIcon.Initialize(ResourceUtility.GetMaterialIconKey(data.rewardItemID), data.rewardItemAmount);
//                 lootItemIcon.StretchRect();
//             }
//         }

//         public async void OnTouchRecieveButton()
//         {
//             var req = new ReceiveAdventureResultRequest(adventureData.adventureAreaID);
//             var response = await NetworkManager.Instance.SendWebRequestAsync<ReceiveAdventureResultResponse>(req);

//             if (response.result != ENetworkResult.Success)
//             {
//                 Debug.Log("Critical Error!");
//             }

//             var storageData = GameInstance.MainUser.storageData;
//             var seedPocketData = GameInstance.MainUser.seedPocketData;

//             var materialStorage = storageData.materialStorage;
//             var seedStorage = seedPocketData.seedStorage;

//             foreach (var data in response.resultPack.materialLootInfo)
//             {
//                 if (materialStorage.ContainsKey(data.rewardItemID))
//                 {
//                     materialStorage[data.rewardItemID] += data.rewardItemAmount;
//                 }
//                 else
//                 {
//                     materialStorage.Add(data.rewardItemID, data.rewardItemAmount);
//                 }
//             }

//             foreach (var data in response.resultPack.seedLootInfo)
//             {
//                 if (seedStorage.ContainsKey(data.rewardItemID))
//                 {
//                     seedStorage[data.rewardItemID] += data.rewardItemAmount;
//                 }
//                 else
//                 {
//                     seedStorage.Add(data.rewardItemID, data.rewardItemAmount);
//                 }
//             }

//             var userAdventureData = GameInstance.MainUser.adventureData;
//             userAdventureData.inAdventureAreaList.Remove(adventureData.adventureAreaID);
//             var list = userAdventureData.inExploreFarmerList[adventureData.adventureAreaID];

//             foreach (var farmerUUID in list)
//             {
//                 userAdventureData.allFarmerinExploreList.Remove(farmerUUID);
//             }

//             userAdventureData.inExploreFarmerList.Remove(adventureData.adventureAreaID);
//         }

//         public new void Release()
//         {
//             base.Release();
//             content.DespawnAllChildren();
//         }

//         public override void Active()
//         {
//             lootItemScroll.SetActive(true);
//             recieveButton.SetActive(true);
//         }

//         public override void DeActive()
//         {
//             lootItemScroll.SetActive(false);
//             recieveButton.SetActive(false);
//         }
//     }
// }
