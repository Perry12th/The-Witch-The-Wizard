using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        [SerializeField]
        private PathCreator pathCreator;
        [SerializeField]
        private EndOfPathInstruction endOfPathInstruction;
        [SerializeField]
        private float speed = 5;
        private float distanceTravelled;
        [SerializeField]
        private bool isGoingLeft = true;
        [SerializeField]
        private bool usingPathRotation = false;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                Vector3 pathEulerAngle = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;
                if (usingPathRotation)
                {
                    transform.eulerAngles = pathEulerAngle;
                }
                enabled = false;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                Vector3 pathEulerAngle = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;
                if (usingPathRotation)
                {
                    transform.eulerAngles = pathEulerAngle;
                }
                //if (isGoingLeft)
                //{
                //    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                //}
                //else
                //{ 
                //    transform.localEulerAngles = new Vector3(-transform.localEulerAngles.x, -transform.localEulerAngles.y, -transform.localEulerAngles.z);
                //}
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        public void SetIsGoingLeft(bool goingLeft)
        {
            isGoingLeft = goingLeft;
        }

        public bool IsGoingLeft()
        {
            return isGoingLeft;
        }

        public void SetUsingPathRotation(bool usingFullPathRotation)
        {
            usingPathRotation = usingFullPathRotation;
        }

        public bool IsUsingPathRotation()
        {
            return usingPathRotation;
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public PathCreator GetPathCreator()
        {
            return pathCreator;
        }

        public void SetPathCreator(PathCreator pathCreator)
        {
            this.pathCreator = pathCreator;
        }
    }
}