using H00N.Resources.Pools;

namespace ProjectF.UI
{
    public class PoolableBehaviourUI : MonoBehaviourUI, IPoolableBehaviour
    {
        private PoolReference poolReference = null;
        public PoolReference PoolReference => poolReference;

        protected override void Awake()
        {
            base.Awake();
            poolReference = GetComponent<PoolReference>();
        }

        public virtual void OnSpawned()
        {
        }

        public virtual void OnDespawn()
        {
        }
    }
}
