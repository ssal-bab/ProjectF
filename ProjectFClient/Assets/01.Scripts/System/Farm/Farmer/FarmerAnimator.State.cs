using UnityEngine;

namespace ProjectCoin.Farms
{
    public partial class FarmerAnimator
    {
        private readonly int IS_IDLE_HASH = Animator.StringToHash("is_idle");
        private readonly int IS_MOVE_HASH = Animator.StringToHash("is_move");
        private readonly int IS_PLOW_HASH = Animator.StringToHash("is_plow");
        private readonly int IS_PLANT_HASH = Animator.StringToHash("is_plant");
        private readonly int IS_WATER_HASH = Animator.StringToHash("is_water");
        private readonly int IS_HARVEST_HASH = Animator.StringToHash("is_harvest");
        private readonly int IS_LIFT_HASH = Animator.StringToHash("is_lift");
        private readonly int IS_LOAD_HASH = Animator.StringToHash("is_load");

        public void SetIdle() => ChangeState(IS_IDLE_HASH);
        public void SetMove() => ChangeState(IS_MOVE_HASH);
        public void SetPlow() => ChangeState(IS_PLOW_HASH);
        public void SetPlant() => ChangeState(IS_PLANT_HASH);
        public void SetWater() => ChangeState(IS_WATER_HASH);
        public void SetHarvest() => ChangeState(IS_HARVEST_HASH);
        public void SetLift() => ChangeState(IS_LIFT_HASH);
        public void SetLoad() => ChangeState(IS_LOAD_HASH);
    }
}