// using System;
// using System.Collections;
// using System.Collections.Generic;
// using H00N.DataTables;
// using H00N.Resources;
// using H00N.Resources.Pools;
// using ProjectF.Datas;
// using ProjectF.DataTables;
// using ProjectF.Farms;
// using ProjectF.UI.Farms;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// namespace ProjectF.UI.Adventure
// {
//     public class ExploreFarmerInfoElementUI : PoolableBehaviourUI
//     {
//         private FarmerData _farmerData;
//         private Action<string> _onRegisterExploreFarmer;
//         private Action<string> _onUnRegisterExploreFarmer;

//         private bool _isRegister;

//         [SerializeField] private Image _farmerProfile;
//         [SerializeField] private TextMeshProUGUI _farmerNameText;
//         [SerializeField] private FarmerStatGroup[] _statGroupArr = new FarmerStatGroup[2];
//         [SerializeField] private GameObject _farmerRegisteredVisualize;
//         [SerializeField] private GameObject _farmerAlreadyInExplore;
//         private AddressableAsset<ExploreFarmerInfoPopupUI> _exploreFarmerInfoPopupUIPrefab;

//         public void Initialize(FarmerData data, Action<string> registerExploreFarmer, Action<string> unRegisterExploreFarmer, 
//                                AddressableAsset<ExploreFarmerInfoPopupUI> popupUI)
//         {
//             new SetSprite(_farmerProfile, ResourceUtility.GetFarmerIconKey(data.farmerID));
//             _farmerNameText.text = data.nickname;

//             var statTable = DataTableManager.GetTable<FarmerStatTable>();
//             var tableRow = statTable.GetRow(data.farmerID);

//             var statDictionary = new GetFarmerStat(tableRow, data.level).statDictionary;

//             foreach(var group in _statGroupArr)
//             {
//                 EFarmerStatType type = group.statType;
//                 new SetSprite(group.statIcon, ResourceUtility.GetFarmerStatIconKey((int)type));
//                 group.valueText.text = statDictionary[type].ToString();
//             }

//             _farmerData = data;

//             var adventureData = GameInstance.MainUser.adventureData;
//             if(adventureData.allFarmerinExploreList.Contains(data.farmerUUID))
//             {
//                 _farmerAlreadyInExplore.SetActive(true);
//                 return;
//             }

//             _onRegisterExploreFarmer += registerExploreFarmer;
//             _onUnRegisterExploreFarmer += unRegisterExploreFarmer;

//             _exploreFarmerInfoPopupUIPrefab = popupUI;
//         }

//         public void OnTouchFarmerInfoButton()
//         {
//             var popup = PoolManager.Spawn(_exploreFarmerInfoPopupUIPrefab, GameDefine.MainPopupFrame);
//             popup.Initialize(_farmerData, _onRegisterExploreFarmer);
//             popup.StretchRect();
//         }

//         public void OnTouchThisElement()
//         {
//             if(_isRegister)
//             {
//                 _onUnRegisterExploreFarmer?.Invoke(_farmerData.farmerUUID);
//             }
//             else
//             {
//                 _onRegisterExploreFarmer?.Invoke(_farmerData.farmerUUID);
//             }

//             _isRegister = !_isRegister;
//             _farmerRegisteredVisualize.SetActive(_isRegister);
//         }
//     }
// }
