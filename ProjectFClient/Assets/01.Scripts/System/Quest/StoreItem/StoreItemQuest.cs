using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectF.Farms;

namespace ProjectF.Quests
{
    public class StoreItemQuest<T>  : Quest where T : ItemSO
    {
        private int targetQuantity;
        private int currentQuantity;

        public int TargetQuantity => targetQuantity;
        public int CurrentQuantity => currentQuantity;

        private ItemStorage targetStorage;

        public StoreItemQuest(ItemStorage targetStorage, int targetQuantity)
        {
            this.targetQuantity = targetQuantity;
            this.targetStorage = targetStorage;
        }

        public override void OnMakeQuest()
        {
            base.OnMakeQuest();

            targetStorage.OnStoreItem += OnStoreItem;
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

            targetStorage.OnStoreItem -= OnStoreItem;
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