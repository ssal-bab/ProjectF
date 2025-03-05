using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class NestSlotListPanelUI : MonoBehaviourUI
    {
        [SerializeField] AddressableAsset<NestDetailInfoPopupUI> detailInfoPopupUIPrefab = null;

        [Space(10f)]
        [SerializeField] ScrollRect scrollView = null;
        [SerializeField] AddressableAsset<NestSlotElementUI> elementPrefab = null;
        private List<NestSlotElementUI> slotElementUIList = null;

        private NestUICallbackContainer callbackContainer = null;
        private UserNestData userNestData = null;

        protected override void Awake()
        {
            slotElementUIList = new List<NestSlotElementUI>();
            scrollView.content.DespawnAllChildren();
        }

        public void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer)
        {
            base.Initialize();
            this.userNestData = userNestData;
            this.callbackContainer = callbackContainer;

            NestTableRow tableRow = DataTableManager.GetTable<NestTable>().GetRowByLevel(userNestData.level); ;
            if (tableRow == null)
                return;

            RefreshUIAsync(tableRow, userNestData.hatchingEggList);
        }

        private async void RefreshUIAsync(NestTableRow tableRow, List<EggHatchingData> hatchingEggList)
        {
            scrollView.verticalNormalizedPosition = 1;
            await SetUpSlotElementUI(tableRow);

            for(int i = 0; i < slotElementUIList.Count; ++i)
            {
                NestSlotElementUI ui = slotElementUIList[i];
                if(hatchingEggList.Count > i)
                {
                    int currentIndex = i;   
                    ui.Initialize(hatchingEggList[currentIndex], () => OpenDetailInfoPopup(currentIndex));
                }
                else
                    ui.Initialize(null, null);
            }
        }

        private async UniTask SetUpSlotElementUI(NestTableRow tableRow)
        {
            int createCount = tableRow.eggStoreLimit - slotElementUIList.Count;
            if (createCount <= 0)
                return;

            scrollView.gameObject.SetActive(false);

            for(int i = 0; i < createCount; ++i)
            {
                NestSlotElementUI ui = await PoolManager.SpawnAsync<NestSlotElementUI>(elementPrefab.Key, scrollView.content);
                slotElementUIList.Add(ui);
            }

            scrollView.gameObject.SetActive(true);
        }

        private async void OpenDetailInfoPopup(int index)
        {
            if(userNestData.hatchingEggList.Count <= 0)
                return;

            NestDetailInfoPopupUI ui = await PoolManager.SpawnAsync<NestDetailInfoPopupUI>(detailInfoPopupUIPrefab.Key, GameDefine.ContentsPopupFrame);
            ui.StretchRect();
            ui.Initialize(userNestData, index, callbackContainer.HatchCallback);
        }
    }
}