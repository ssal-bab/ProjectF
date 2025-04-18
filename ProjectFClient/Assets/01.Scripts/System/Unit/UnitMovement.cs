using UnityEngine;
using UnityEngine.AI;

namespace ProjectF.Units
{
    public class UnitMovement : MonoBehaviour
    {
        private NavMeshAgent navAgent = null;
        public Vector3 Velocity => navAgent.velocity;

        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.updateRotation = false;
            navAgent.updateUpAxis = false;
        }

        // private void FixedUpdate()
        // {
        //     Vector3 currentPosition = transform.position;
        //     Vector3 directionVector = currentDestination - currentPosition.PlaneVector();
        //     if(directionVector.sqrMagnitude <= 0.1f)
        //     {
        //         velocity = 0f;   
        //         return;
        //     }

        //     velocity += acceleration * Time.fixedDeltaTime;
        //     velocity = Mathf.Min(velocity, maxSpeed);
            
        //     Vector3 direction = directionVector.normalized;
        //     transform.position += direction * (velocity * Time.fixedDeltaTime);
        // }

        public void SetMaxSpeed(float maxSpeed)
        {
            navAgent.speed = maxSpeed;
        }

        public void SetAcceleration(float acceleration)
        {
            navAgent.acceleration = acceleration;
        }

        public void SetDestination(Vector2 destination)
        {
            navAgent.SetDestination(destination);
        }

        public Vector2 GetValidDestination(Vector2 destination)
        {
            NavMeshPath path = new NavMeshPath();
            if (navAgent.CalculatePath(destination, path) && path.corners.Length > 1)
                return path.corners[^1];

            return transform.position;
        }
    }
}
