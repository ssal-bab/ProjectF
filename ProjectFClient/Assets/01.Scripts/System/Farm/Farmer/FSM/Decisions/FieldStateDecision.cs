using ProjectF.Datas;
using ProjectF.Farms.AI;
using UnityEngine;

namespace ProjectF.Farms
{
    public class FieldStateDecision : FarmerFSMDecision
    {
        [SerializeField] EFieldState targetFeildState = EFieldState.None;

        public override bool MakeDecision()
        {
            Field targetField = aiData.CurrentTarget as Field;
            if (targetField == null)
                return false;

            return targetField.FieldState == targetFeildState;
        }
    }
}
