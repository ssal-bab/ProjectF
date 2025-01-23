using ProjectCoin.Farms.AI;
using UnityEngine;

namespace ProjectCoin.Tests
{
    public class TTarget : FarmerFSMAction
    {
        [SerializeField] Transform testTarget = null;

        public override void EnterState()
        {
            base.EnterState();
            // brain.GetFSMParam<FarmerAIDataSO>().currentTarget = testTarget;
        }
    }
}
