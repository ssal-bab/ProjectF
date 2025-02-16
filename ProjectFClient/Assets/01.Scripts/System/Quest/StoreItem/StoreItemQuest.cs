using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectF.Farms;
using ProjectF.Datas;

namespace ProjectF.Quests
{
    public class StoreItemQuest : Quest
    {
        private int targetQuantity;
        private int currentQuantity;

        public int TargetQuantity => targetQuantity;
        public int CurrentQuantity => currentQuantity;

        private EItemType itemType;

        public StoreItemQuest(EItemType itemType, int targetQuantity)
        {
            this.targetQuantity = targetQuantity;
            this.itemType = itemType;
        }

        public override void OnMakeQuest()
        {
            base.OnMakeQuest();

            //아이템 획득시 작업
        }

        protected override bool CheckQuestClear()
        {
            return currentQuantity >= targetQuantity;
        }

        protected override void UpdateQuest()
        {
            message = $"acquire items : {currentQuantity} / {targetQuantity}";

            base.UpdateQuest();
        }

        public override void OnClearQuest()
        {
            base.OnClearQuest();
        }

        protected override void MakeReward()
        {

        }

        protected virtual void OnStoreItem(ItemSO itemData, int quantity)
        {
            currentQuantity = quantity;

            UpdateQuest();
        }
    }
}