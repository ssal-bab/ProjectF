// using System;
// using System.Collections;
// using System.Collections.Generic;
// using H00N.DataTables;
// using H00N.Resources.Pools;
// using ProjectF.Datas;
// using ProjectF.DataTables;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// namespace ProjectF.UI.Adventure
// {
//     public class ExploreFarmerInfoPopupUI : PoolableBehaviourUI
//     {
//         [SerializeField] private TextMeshProUGUI _farmerNameText;
//         [SerializeField] private Image _farmerProfile;
//         [SerializeField] private TextMeshProUGUI _healthInfoText;
//         [SerializeField] private TextMeshProUGUI _adventureSkillInfoText;

//         private Action<string> _onRegisterExploreFarmer;

//         private const EFarmerStatType ADVENTURE_HEALTH = EFarmerStatType.AdventureHealth;
//         private const EFarmerStatType ADVENTURE_SKILL = EFarmerStatType.FarmingSkill;

//         private FarmerData _farmerData;

//         public void Initialize(FarmerData data, Action<string> registerExploreFarmer)
//         {
//             _farmerData = data;

//             _farmerNameText.text = data.nickname;
//             new SetSprite(_farmerProfile, ResourceUtility.GetFarmerIconKey(data.farmerID));

//             var statTable = DataTableManager.GetTable<FarmerStatTable>();
//             var tableRow = statTable.GetRow(data.farmerID);

//             var statDictionary = new GetFarmerStat(tableRow, data.level).statDictionary;

//             var healthValue = new CalculateFarmerProductivity(ADVENTURE_HEALTH, statDictionary[ADVENTURE_HEALTH]).value;
//             var adventureSkillValue = new CalculateFarmerProductivity(ADVENTURE_SKILL, statDictionary[ADVENTURE_SKILL]).value;

//             _healthInfoText.text = $"{ResourceUtility.GetStatDescriptionLocakKey(ADVENTURE_HEALTH)} {healthValue}";
//             _healthInfoText.text = $"{ResourceUtility.GetStatDescriptionLocakKey(ADVENTURE_SKILL)} {adventureSkillValue}";

//             _onRegisterExploreFarmer += registerExploreFarmer;
//         }

//         public void OnTouchRegisterButton()
//         {
//             _onRegisterExploreFarmer?.Invoke(_farmerData.farmerUUID);
//         }

//         public void OnTouchCloseButton()
//         {
//             Release();
//             PoolManager.Despawn(this);
//         }

//         protected override void Release()
//         {
//             base.Release();
            
//             _farmerData = null;
//             _onRegisterExploreFarmer = null;
//         }
//     }
// }
