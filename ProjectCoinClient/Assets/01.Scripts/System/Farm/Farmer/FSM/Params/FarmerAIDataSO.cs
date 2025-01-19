using System.Collections.Generic;
using H00N.FSM;
using ProjectCoin.Units;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    [CreateAssetMenu(menuName = "SO/Farm/FarmerAIData")]
    public class FarmerAIDataSO : FSMParamSO
    {
        public FarmerStatSO farmerStat = null;
        public UnitMovement movement = null;
        public Farmer farmer = null;

        public CropSO currentSeedData = null;

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

            farmerStat = farmer.StatData;
            movement = farmer.GetComponent<UnitMovement>();

            this.farmer = farmer;
        }

        public void PushTarget(FarmerTargetableBehaviour target)
        {
            targetStack.Push(target);
            target?.SetWatcher(farmer);
        }

        public void PopTarget()
        {
            FarmerTargetableBehaviour target = targetStack.Pop();
            target?.SetWatcher(null);
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
