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

namespace ProjectF.UI.Nests
{
    public class NestSlotListPanelUI : MonoBehaviourUI
    {
        [SerializeField] ScrollRect scrollView = null;
        [SerializeField] AddressableAsset<NestSlotElementUI> elementPrefab = null;
        private List<NestSlotElementUI> slotElementUIList = null;

        protected override void Awake()
        {
            slotElementUIList = new List<NestSlotElementUI>();
            scrollView.content.DespawnAllChildren();
        }

        public new async void Initialize()
        {
            base.Initialize();

            await elementPrefab.InitializeAsync();
            // GetFacilityTableRow<NestTable, NestTableRow> getFacilityTableRow = new GetFacilityTableRow<NestTable, NestTableRow>(nestData.level);
            // if (getFacilityTableRow.currentTableRow == null)
            //     return;


            RefreshUI();
        }

        private void RefreshUI()
        {
            UserNestData nestData = GameInstance.MainUser.nestData;
            NestLevelTableRow tableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(nestData.level);
            if(tableRow == null)
                return;

            scrollView.verticalNormalizedPosition = 1;
            SetUpSlotElementUI(tableRow);

            int index = 0;
            foreach(string eggUUID in nestData.hatchingEggDatas.Keys)
            {
                NestSlotElementUI ui = slotElementUIList[index];
                ui.Initialize(eggUUID);
                index++;
            }

            for(; index < slotElementUIList.Count; ++index)
            {
                NestSlotElementUI ui = slotElementUIList[index];
                ui.Initialize(null);
            }
        }

        private void SetUpSlotElementUI(NestLevelTableRow tableRow)
        {
            int createCount = tableRow.eggStoreLimit - slotElementUIList.Count;
            if (createCount <= 0)
                return;

            scrollView.gameObject.SetActive(false);

            for(int i = 0; i < createCount; ++i)
            {
                NestSlotElementUI ui = PoolManager.Spawn<NestSlotElementUI>(elementPrefab, scrollView.content);
                ui.InitializeTransform();
                slotElementUIList.Add(ui);
            }

            scrollView.gameObject.SetActive(true);
        }
    }
}