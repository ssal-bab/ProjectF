using UnityEngine;

namespace ProjectF.Farms
{
    public partial class Storage : FarmerTargetableBehaviour
    {
        [SerializeField] Transform entranceTransform = null;
        public override Vector3 TargetPosition => entranceTransform.position;

        public override bool TargetEnable => Watcher != null;

        protected override void Awake()
        {
            base.Awake();
        }

        #if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            if(entranceTransform == null)
                return;

            base.DrawGizmos();
        }
        #endif
    }
}