using H00N.DataTables;
using H00N.Extensions;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageInfoPanelUI : MonoBehaviourUI
    {
        private const float COUNT_UPDATE_DELAY = 10f;

        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text countText = null;
        [SerializeField] RectTransform sliderFillRect = null;

        public new void Initialize()
        {
            base.Initialize();
            RefreshUI();
        }

        public new void Release()
        {
            base.Release();
            StopAllCoroutines();
        }

        private void RefreshUI()
        {
            UserStorageData storageData = GameInstance.MainUser.storageData;
            StorageTable storageTable = DataTableManager.GetTable<StorageTable>();
            StorageTableRow tableRow = storageTable.GetRowByLevel(storageData.level); ;
            if (tableRow == null)
                return;

            storageIconImage.sprite = ResourceUtility.GetStorageIcon(tableRow.id);
            nameText.text = $"Lv.{tableRow.level} 적재소{tableRow.level}"; // 나중에 localizing 적용해야 함
            StartCoroutine(this.LoopRoutine(COUNT_UPDATE_DELAY, () => UpdateCountInfo(storageData, tableRow)));
        }

        private void UpdateCountInfo(UserStorageData storageData, StorageTableRow tableRow)
        {
            int usedCount = new GetStorageUsedCount(storageData).storageUsedCount;
            countText.text = $"{usedCount}/{tableRow.storeLimit}";
            
            Vector2 anchorMax = sliderFillRect.anchorMax;
            anchorMax.x = Mathf.Max(usedCount / (float)tableRow.storeLimit, 0);
            sliderFillRect.anchorMax = anchorMax;
        }

        public void OnTouchUpgradeButton()
        {
            StorageUpgradePopupUI popupUI = PoolManager.Spawn<StorageUpgradePopupUI>("StorageUpgradePopupUI", GameDefine.ContentsPopupFrame);
            popupUI.Initialize();
        }
    }
}
