// using System.Collections.Generic;
// using UnityEngine;
// using H00N.Resources;
// using H00N.Resources.Pools;
// using System.Linq;
// using System;
// using H00N.Extensions;

// namespace ProjectF.UI.Farms
// {
//     public class FarmerListUI : MonoBehaviourUI
//     {
//         [SerializeField] private AddressableAsset<FarmerInfoElementUI> farmerInfoUIPrefab = null;
//         [SerializeField] private Transform farmerInfoContent;

//         private EOrderType currentOrderType = EOrderType.Ascending;
//         private EFarmerClassificationType currentClasification = EFarmerClassificationType.Acquisition;
        

//         private List<FarmerInfoElementUI> infoElementList = new();

//         public async void Initialize(Action<string, int> farmerRegisterSalesAction, Action<string> farmerUnRegisterSalesAction)
//         {
//             base.Initialize();
//             await farmerInfoUIPrefab.InitializeAsync();
//             Debug.Log("Initialized");

//             currentOrderType = GameSetting.LastFarmerOrderType;
//             currentClasification = GameSetting.LastFarmerClassificationType;

//             RefreshUISelf(farmerRegisterSalesAction, farmerUnRegisterSalesAction);
//         }

//         public void RefreshUISelf(Action<string, int> farmerRegisterSalesAction, Action<string> farmerUnRegisterSalesAction)
//         {
//             RefreshUI(farmerRegisterSalesAction, farmerUnRegisterSalesAction);
//         }

//         public void RefreshUI(Action<string, int> farmerRegisterSalesAction, Action<string> farmerUnRegisterSalesAction)
//         {
//             var userFarmerData = GameInstance.MainUser.farmerData;
//             foreach(var farmerData in userFarmerData.farmerList.Values)
//             {
//                 Debug.Log(farmerInfoUIPrefab.Key);
//                 var farmerInfoElement = PoolManager.Spawn(farmerInfoUIPrefab, farmerInfoContent);
//                 farmerInfoElement.Initialize(farmerData);
//                 farmerInfoElement.RegisterFarmerSalesAction(farmerRegisterSalesAction, farmerUnRegisterSalesAction);
//                 farmerInfoElement.StretchRect();
//                 farmerInfoElement.RectTransform.sizeDelta = new Vector2(940, 175);

//                 infoElementList.Add(farmerInfoElement);
//             }
//         }

//         public void ActiveSalesFarmerMode(bool isActive, FarmerListPopupUI farmerListPopupUI)
//         {
//             foreach(var info in infoElementList)
//             {
//                 info.ActiveSalesFarmerMode(isActive);
//             }
//         }

//         public EOrderType ChangeOrder()
//         {
//             currentOrderType = currentOrderType == EOrderType.Ascending ? EOrderType.Descending : EOrderType.Ascending;
//             GameSetting.LastFarmerOrderType = currentOrderType;
//             SortingFarmerInfoElement(currentOrderType, currentClasification);
//             return currentOrderType;
//         }

//         public EFarmerClassificationType ChangeClassification(EFarmerClassificationType classificationType)
//         {
//             currentClasification = classificationType;
//             GameSetting.LastFarmerClassificationType = currentClasification;
//             SortingFarmerInfoElement(currentOrderType, currentClasification);
//             return currentClasification;
//         }

//         private void SortingFarmerInfoElement(EOrderType orderType, EFarmerClassificationType classificationType)
//         {
//             // 획득순은 데이터 없어서 보류
//             switch (classificationType)
//             {
//                 case EFarmerClassificationType.Acquisition:
//                     break;
//                 case EFarmerClassificationType.Rarity:
//                     infoElementList
//                     .OrderBy(e => e.Rarity)
//                     .ThenBy(e => e.Level)
//                     .ThenBy(e => e.NickName);
//                     break;
//                 case EFarmerClassificationType.Name:
//                     infoElementList
//                     .OrderBy(e => e.NickName)
//                     .ThenBy(e => e.Rarity)
//                     .ThenBy(e => e.Level);
//                     break;
//                 case EFarmerClassificationType.Level:
//                     infoElementList
//                     .OrderBy(e => e.Level)
//                     .ThenBy(e => e.Rarity)
//                     .ThenBy(e => e.NickName);
//                     break;
//             }

//             if(orderType == EOrderType.Ascending)
//             {
//                 for(int i = 0; i < infoElementList.Count; i++)
//                 {
//                     infoElementList[i].transform.SetSiblingIndex(i);
//                 }
//             }
//             else
//             {
//                 for(int i = 0; i < infoElementList.Count; i++)
//                 {
//                     infoElementList[infoElementList.Count - 1 - i].transform.SetSiblingIndex(i);
//                 }
//             }
//         }

//         public new void Release()
//         {
//             farmerInfoContent.DespawnAllChildren();
//             base.Release();
//         }
//     }
// }
