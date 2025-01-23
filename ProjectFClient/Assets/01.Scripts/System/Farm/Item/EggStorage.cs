using System.Collections.Generic;
using UnityEngine;

namespace ProjectCoin.Farms
{
    public class EggStorage : ItemStorage
    {
        private List<Egg> eggList = null;

        protected override void Awake()
        {
            base.Awake();
            eggList = new List<Egg>();
        }

        public bool Contains(Egg egg) => eggList.Contains(egg);

        public Egg GetEgg(ItemSO itemData)
        {
            Egg egg = eggList.Find(i => i.ItemData == itemData);
            return egg;
        }

        public override bool ConsumeItem(ItemSO itemData)
        {
            bool result = base.ConsumeItem(itemData);

            int index = eggList.FindIndex(i => i.ItemData == itemData);
            if(index != -1)
            {
                Egg egg = eggList[index];
                egg.transform.SetParent(null);
                egg.SetStorage(null);

                eggList.RemoveAt(index);

            }

            return result;
        }

        public override void StoreItem(Item item)
        {
            base.StoreItem(item);

            Egg egg = item as Egg;
            egg.transform.SetParent(transform);
            egg.transform.localPosition = Vector3.zero;
            egg.SetStorage(this);

            eggList.Add(egg);
        }
    }
}