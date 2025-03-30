using UnityEngine;

namespace ProjectF.Farms
{
    public partial class FarmerAnimator : MonoBehaviour
    {
        private Animator animator = null;
        private int currentHash = 0;
        private bool nonHash = false;

        private void Awake()
        {
            currentHash = IS_IDLE_HASH;
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            nonHash = true;
        }

        private void ChangeState(int hash)
        {
            if(nonHash == false && currentHash == hash)
                return;

            nonHash = false;
            animator.SetBool(currentHash, false);
            currentHash = hash;
            animator.SetBool(currentHash, true);
        }
    }
}
