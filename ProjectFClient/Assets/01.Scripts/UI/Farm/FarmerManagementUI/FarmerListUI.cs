using System;
using System.Collections;
using System.Collections.Generic;
using ProjectF.UI;
using UnityEngine;
using H00N.Resources;
using H00N.Resources.Pools;
using System.Threading.Tasks;
using Codice.CM.Common;
using H00N.Extensions;
using System.Linq;

namespace ProjectF.UI.Farms
{
    public class FarmerListUI : MonoBehaviourUI
    {
        [SerializeField] private AddressableAsset<FarmerInfoElementUI> farmerInfoUIPrefab = null;
        [SerializeField] private Transform farmerInfoContent;

        private OrderType currentOrderType = OrderType.Ascending;
        private const string orderTypeKey = "SavedOrderType";
        private ClassificationType currentClasification = ClassificationType.Acquisition;
        private const string classificationTypeKey = "SavedClassificationType";

        private List<FarmerInfoElementUI> infoElementList = new();

        public new void Initialize()
        {
            base.Initialize();
            farmerInfoUIPrefab.Initialize();

            currentOrderType = (OrderType)PlayerPrefs.GetInt(orderTypeKey);
            currentClasification = (ClassificationType)PlayerPrefs.GetInt(classificationTypeKey);

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

        public OrderType ChangeOrder()
        {
            currentOrderType = currentOrderType == OrderType.Ascending ? OrderType.Descending : OrderType.Ascending;
            PlayerPrefs.SetInt(orderTypeKey, (int)currentOrderType);
            SortingFarmerInfoElement(currentOrderType, currentClasification);
            return currentOrderType;
        }

        public ClassificationType ChangeClassification(ClassificationType classificationType)
        {
            currentClasification = classificationType;
            PlayerPrefs.SetInt(classificationTypeKey, (int)classificationType);
            SortingFarmerInfoElement(currentOrderType, currentClasification);
            return currentClasification;
        }

        private void SortingFarmerInfoElement(OrderType orderType, ClassificationType classificationType)
        {
            // 획득순은 데이터 없어서 보류
            switch (classificationType)
            {
                case ClassificationType.Acquisition:
                    break;
                case ClassificationType.Rarity:
                    infoElementList
                    .OrderBy(e => e.Rarity)
                    .ThenBy(e => e.Level)
                    .ThenBy(e => e.NickName);
                    break;
                case ClassificationType.Name:
                    infoElementList
                    .OrderBy(e => e.NickName)
                    .ThenBy(e => e.Rarity)
                    .ThenBy(e => e.Level);
                    break;
                case ClassificationType.Level:
                    infoElementList
                    .OrderBy(e => e.Level)
                    .ThenBy(e => e.Rarity)
                    .ThenBy(e => e.NickName);
                    break;
            }

            if(orderType == OrderType.Ascending)
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
