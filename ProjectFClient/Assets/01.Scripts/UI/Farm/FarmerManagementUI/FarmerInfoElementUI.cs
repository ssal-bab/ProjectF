using System.Collections;
using System.Collections.Generic;
using ProjectF.Datas;
using ProjectF.UI;
using UnityEngine;
using ProjectF.Farms;
using TMPro;
using UnityEngine.UI;
using H00N.DataTables;
using ProjectF.DataTables;
using System;
using ProjectFServer.Networks.Packets;
using ProjectF.Networks;
using System.Threading.Tasks;
using H00N.Stats;
using H00N.Resources;
using H00N.Resources.Pools;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace ProjectF.UI.Farms
{
    [Serializable]
    public struct FarmerStatGroup
    {
        public EFarmerStatType statType;
        public Image statIcon;
        public TextMeshProUGUI valueText;
    }

    public class FarmerInfoElementUI : MonoBehaviourUI
    {
        [Header("Assignment")]
        [SerializeField] private AddressableAsset<FarmerInfoPopupUI> farmerInfoPopupUIPrefab = null;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI nickNameText;
        [SerializeField] private Image farmerIcon;
        private const int MINIMALIZE_STAT_COUNT = 4;
        [SerializeField] private FarmerStatGroup[] statGroup = new FarmerStatGroup[MINIMALIZE_STAT_COUNT];

        [Header("TouchFilter")]
        [SerializeField] private Image touchFilter;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectedColor;

        #region DataProperty
        private FarmerData farmerData;
        public ERarity Rarity => farmerData.rarity;
        public string NickName => farmerData.nickname;
        public int Level => farmerData.level;
        #endregion

        private FarmerInfoTouchEvent onTouchEvent = null;

        public void Initialize(FarmerData data)
        {
            base.Initialize();
            farmerInfoPopupUIPrefab.Initialize();

            rarityText.text = GetRarityWord(data.rarity);
            nickNameText.text = data.nickname;
            farmerIcon.sprite = ResourceUtility.GetFarmerIcon(data.farmerID);

            var statTable = DataTableManager.GetTable<FarmerStatTable>();
            var statDictionary = statTable.GetFarmerStat(data.farmerID, data.level);

            for(int i = 0 ; i < MINIMALIZE_STAT_COUNT; i++)
            {
                EFarmerStatType statType = statGroup[i].statType;
                statGroup[i].statIcon.sprite = ResourceUtility.GetFarmerStatIcon((int)statType);
                statGroup[i].valueText.text = statDictionary[statType].ToString();
            }
            
            farmerData = data;
            onTouchEvent += HandleActiveFarmerInfoPopupAsync;
        }

        public void TouchThisElement()
        {
            onTouchEvent?.Invoke();
        }

        private void ClearTouchEvent()
        {
            onTouchEvent = null;
        }

        public void ActiveSalesFarmerMode(bool isActive, FarmerListPopupUI farmerListPopupUI)
        {
            ClearTouchEvent();

            if(isActive)
            {
                onTouchEvent += () => HandleRegisterSlaesFarmer(farmerListPopupUI);
            }
            else
            {
                farmerListPopupUI.UnRegisterSalesFarmer(farmerData.farmerUUID);
                onTouchEvent += HandleActiveFarmerInfoPopupAsync;
            }
        }

        private async void HandleActiveFarmerInfoPopupAsync()
        {
            await PoolManager.SpawnAsync<FarmerInfoPopupUI>(farmerInfoPopupUIPrefab.Key, GameDefine.ContentsPopupFrame);
        }

        private void HandleRegisterSlaesFarmer(FarmerListPopupUI farmerList)
        {
            ClearTouchEvent();
            onTouchEvent += () => HandleUnRegisterSlaesFarmer(farmerList);

            farmerList.RegisterSalesFarmer(farmerData.farmerUUID, Level);
            touchFilter.color = selectedColor;
        }

        private void HandleUnRegisterSlaesFarmer(FarmerListPopupUI farmerList)
        {
            ClearTouchEvent();
            onTouchEvent += () => HandleRegisterSlaesFarmer(farmerList);

            farmerList.UnRegisterSalesFarmer(farmerData.farmerUUID);
            touchFilter.color = normalColor;
        }

        private string GetRarityWord(ERarity rarity)
        {
            switch (rarity)
            {
                case ERarity.Common:
                    return "커먼";
                case ERarity.Uncommon:
                    return "언커먼";
                case ERarity.Rare:
                    return "레어";
                case ERarity.Epic:
                    return "에픽";
                case ERarity.Legendary:
                    return "레전더리";
                case ERarity.Mythic:
                    return "미스틱";
                default:
                    return "UnknownValue";
            }
        }
    }

    public delegate void FarmerInfoTouchEvent();
}
