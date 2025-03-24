using System.Collections.Generic;
using UnityEngine;
using H00N.Resources;
using H00N.Resources.Pools;
using System.Linq;

namespace ProjectF.UI.Farms
{
    public class FarmerListUI : MonoBehaviourUI
    {
        [SerializeField] private AddressableAsset<FarmerInfoElementUI> farmerInfoUIPrefab = null;
        [SerializeField] private Transform farmerInfoContent;

        private EOrderType currentOrderType = EOrderType.Ascending;
        private const string orderTypeKey = "SavedOrderType";
        private EClassificationType currentClasification = EClassificationType.Acquisition;
        private const string classificationTypeKey = "SavedClassificationType";

        private List<FarmerInfoElementUI> infoElementList = new();

        public new void Initialize()
        {
            base.Initialize();
            farmerInfoUIPrefab.Initialize();

            currentOrderType = (EOrderType)PlayerPrefs.GetInt(orderTypeKey);
            currentClasification = (EClassificationType)PlayerPrefs.GetInt(classificationTypeKey);

            RefreshUISelf();
        }

        public void RefreshUISelf()
        {
            RefreshUIAsync();
        }

        public async void RefreshUIAsync()
        {
            foreach(var farmerData in GameInstance.MainUser.farmerData.farmerList.Values)
            {
                var farmerInfoElement = await PoolManager.SpawnAsync<FarmerInfoElementUI>(farmerInfoUIPrefab.Key, farmerInfoContent);
                farmerInfoElement.Initialize(farmerData);

                infoElementList.Add(farmerInfoElement);
            }
        }

        public void ActiveSalesFarmerMode(bool isActive, FarmerListPopupUI farmerListPopupUI)
        {
            foreach(var info in infoElementList)
            {
                info.ActiveSalesFarmerMode(isActive, farmerListPopupUI);
            }
        }

        public EOrderType ChangeOrder()
        {
            currentOrderType = currentOrderType == EOrderType.Ascending ? EOrderType.Descending : EOrderType.Ascending;
            PlayerPrefs.SetInt(orderTypeKey, (int)currentOrderType);
            SortingFarmerInfoElement(currentOrderType, currentClasification);
            return currentOrderType;
        }

        public EClassificationType ChangeClassification(EClassificationType classificationType)
        {
            currentClasification = classificationType;
            PlayerPrefs.SetInt(classificationTypeKey, (int)classificationType);
            SortingFarmerInfoElement(currentOrderType, currentClasification);
            return currentClasification;
        }

        private void SortingFarmerInfoElement(EOrderType orderType, EClassificationType classificationType)
        {
            // 획득순은 데이터 없어서 보류
            switch (classificationType)
            {
                case EClassificationType.Acquisition:
                    break;
                case EClassificationType.Rarity:
                    infoElementList
                    .OrderBy(e => e.Rarity)
                    .ThenBy(e => e.Level)
                    .ThenBy(e => e.NickName);
                    break;
                case EClassificationType.Name:
                    infoElementList
                    .OrderBy(e => e.NickName)
                    .ThenBy(e => e.Rarity)
                    .ThenBy(e => e.Level);
                    break;
                case EClassificationType.Level:
                    infoElementList
                    .OrderBy(e => e.Level)
                    .ThenBy(e => e.Rarity)
                    .ThenBy(e => e.NickName);
                    break;
            }

            if(orderType == EOrderType.Ascending)
            {
                for(int i = 0; i < infoElementList.Count; i++)
                {
                    infoElementList[i].transform.SetSiblingIndex(i);
                }
            }
            else
            {
                for(int i = 0; i < infoElementList.Count; i++)
                {
                    infoElementList[infoElementList.Count - 1 - i].transform.SetSiblingIndex(i);
                }
            }
        }

        public new void Release()
        {
            base.Release();
        }
    }
}
