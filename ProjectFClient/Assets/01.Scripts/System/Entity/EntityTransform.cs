using UnityEngine;

namespace ProjectCoin.Entities
{
    public class EntityTransform : MonoBehaviour
    {
        private float lastUpdatedDelta = 0f;

        private void FixedUpdate()
        {
            if(lastUpdatedDelta == transform.position.y)
                return;

            lastUpdatedDelta = transform.position.y;
            UpdateDepth(lastUpdatedDelta);
        }

        private void UpdateDepth(float delta)
        {
            Vector3 position = transform.position;
            position.z = delta * 0.01f;
            transform.position = position;
        }
    }
}
