using ProjectF.Datas;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using H00N.DataTables;
using ProjectF.DataTables;
using System;
using H00N.Resources;
using H00N.Resources.Pools;

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

        private Action onTouchEvent = null;
        private Action<string, int> onFarmerRegisterSalesList = null;
        private Action<string> onFarmerUnRegisterSalesList = null;

        public void Initialize(FarmerData data)
        {
            base.Initialize();
            farmerInfoPopupUIPrefab.Initialize();

            rarityText.text = ResourceUtility.GetRarityNameLocalKey(data.rarity);
            nickNameText.text = data.nickname;
            farmerIcon.sprite = ResourceUtility.GetFarmerIcon(data.farmerID);

            var statTable = DataTableManager.GetTable<FarmerStatTable>();
            var tableRow = statTable.GetRow(data.farmerID);

            var statDictionary = new GetFarmerStat(tableRow, data.level).statDictionary;

            for(int i = 0 ; i < MINIMALIZE_STAT_COUNT; i++)
            {
                EFarmerStatType statType = statGroup[i].statType;
                statGroup[i].statIcon.sprite = ResourceUtility.GetFarmerStatIcon((int)statType);
                statGroup[i].valueText.text = statDictionary[statType].ToString();
            }
            
            farmerData = data;
            onTouchEvent += HandleActiveFarmerInfoPopupAsync;
        }

        public void RegisterFarmerSalesAction(Action<string, int> farmerRegisterSalesAction, Action<string> farmerUnRegisterSalesAction)
        {
            onFarmerRegisterSalesList += farmerRegisterSalesAction;
            onFarmerUnRegisterSalesList += farmerUnRegisterSalesAction;
        }

        public void TouchThisElement()
        {
            onTouchEvent?.Invoke();
        }

        private void ClearTouchEvent()
        {
            onTouchEvent = null;
        }

        public void ActiveSalesFarmerMode(bool isActive)
        {
            ClearTouchEvent();

            if(isActive)
            {
                onTouchEvent += () => HandleRegisterSalesFarmer();
            }
            else
            {
                onFarmerUnRegisterSalesList?.Invoke(farmerData.farmerUUID);
                onTouchEvent += HandleActiveFarmerInfoPopupAsync;
            }
        }

        private async void HandleActiveFarmerInfoPopupAsync()
        {
            var infoUI = await PoolManager.SpawnAsync<FarmerInfoPopupUI>(farmerInfoPopupUIPrefab.Key, GameDefine.ContentsPopupFrame);
            infoUI.Initialize(farmerData);
            infoUI.StretchRect();
        }

        private void HandleRegisterSalesFarmer()
        {
            ClearTouchEvent();
            onTouchEvent += () => HandleUnRegisterSalesFarmer();

            onFarmerRegisterSalesList?.Invoke(farmerData.farmerUUID, Level);
            touchFilter.color = selectedColor;
        }

        private void HandleUnRegisterSalesFarmer()
        {
            ClearTouchEvent();
            onTouchEvent += () => HandleRegisterSalesFarmer();

            onFarmerUnRegisterSalesList?.Invoke(farmerData.farmerUUID);
            touchFilter.color = normalColor;
        }
    }
}
