using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.UI
{
    public abstract class UpgradePopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<CostOptionUI> costOptionUIPrefab = null;
        [SerializeField] Transform costOptionUIContainer = null;
        [SerializeField] UpgradeButtonUI upgradeButtonUI = null;
        private List<CostOptionUI> costOptionUIList = null;

        protected override void Initialize()
        {
            base.Initialize();
            costOptionUIList ??= new List<CostOptionUI>();
        }

        protected override void Release()
        {
            base.Release();

            costOptionUIList.ForEach(i => i.Release());
            costOptionUIList.Clear();
            costOptionUIContainer.DespawnAllChildren();

            upgradeButtonUI.Release();
        }

        protected async UniTask InitializeUpgradeUI()
        {
            await costOptionUIPrefab.InitializeAsync();
        }

        protected void RefreshUpgradeUI<TRow>(LevelTableRow levelTableRow, List<TRow> upgradeCostTableRowList) where TRow : UpgradeCostTableRow
        {
            costOptionUIContainer.DespawnAllChildren();
            costOptionUIList.Clear();
            if(upgradeCostTableRowList != null)
            {
                foreach(TRow upgradeCostTableRow in upgradeCostTableRowList)
                {
                    CostOptionUI costOptionUI = PoolManager.Spawn<CostOptionUI>(costOptionUIPrefab, costOptionUIContainer);
                    costOptionUI.InitializeTransform();
                    costOptionUI.Initialize(upgradeCostTableRow.costItemID, upgradeCostTableRow.costValue);
                    costOptionUIList.Add(costOptionUI);
                }
            }

            upgradeButtonUI.Initialize(levelTableRow.gold);
        }

        protected bool GetUpgradePossible()
        {
            foreach(var i in costOptionUIList)
            {
                if(i.OptionChecked == false)
                    return false;
            }

            if(upgradeButtonUI.UpgradePossible == false)
                return false;

            return true;
        }
    }
}