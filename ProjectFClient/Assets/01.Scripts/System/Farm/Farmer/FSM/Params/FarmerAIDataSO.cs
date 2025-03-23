using System.Collections.Generic;
using H00N.FSM;
using ProjectF.Units;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    [CreateAssetMenu(menuName = "SO/Farm/FarmerAIData")]
    public class FarmerAIDataSO : FSMParamSO
    {
        public UnitMovement movement = null;
        public Farmer farmer = null;

        public int targetCropID = -1;
        public bool isResting = false;

        private Stack<FarmerTargetableBehaviour> targetStack = null;
        public FarmerTargetableBehaviour CurrentTarget {
            get {
                if(targetStack.Count <= 0)
                    return null;

                return targetStack.Peek();
            }
        }
        // private FarmerTargetableBehaviour currenTarget = null;

        public void Initialize(Farmer farmer)
        {
            targetStack = new Stack<FarmerTargetableBehaviour>();

            // 2025.02.13 farmer �ڵ忡 FarmerStatSO��� FarmerStat �߰��ϸ鼭 �ּ�ó��
            //farmerStat = farmer.StatData;
            movement = farmer.GetComponent<UnitMovement>();

            this.farmer = farmer;
        }

        public void PushTarget(FarmerTargetableBehaviour target)
        {
            targetStack.Push(target);
            target.AddWatcher(farmer);
        }

        public void PopTarget()
        {
            FarmerTargetableBehaviour target = targetStack.Pop();
            if(target != null)
                target.RemoveWatcher(farmer.FarmerUUID);
        }

        public void ClearTarget()
        {
            while(targetStack.Count > 0)
                PopTarget();
        }

        // public void SetTarget(FarmerTargetableBehaviour target)
        // {
        //     currenTarget = target;
        //     currenTarget?.SetWatcher(farmer);
        // }

        // public void ResetTarget()
        // {
        //     currenTarget?.SetWatcher(null);
        //     currenTarget = null;
        // }
    }
}
