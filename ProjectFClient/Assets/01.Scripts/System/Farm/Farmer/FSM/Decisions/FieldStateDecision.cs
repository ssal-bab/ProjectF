using ProjectCoin.Datas;
using ProjectCoin.Farms.AI;
using UnityEngine;

namespace ProjectCoin.Farms
{
    public class FieldStateDecision : FarmerFSMDecision
    {
        [SerializeField] EFieldState targetFeildState = EFieldState.None;

        public override bool MakeDecision()
        {
            Field targetField = aiData.CurrentTarget as Field;
            if (targetField == null)
                return false;

            return targetField.CurrentState == targetFeildState;
        }
    }
}
