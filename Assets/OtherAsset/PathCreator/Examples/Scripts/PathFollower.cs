using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        private float speedStorage = 0f;
        public float speed = 5;
        private bool isStop = false;
        float distanceTravelled;

        [SerializeField]
        private bool reverseStartEnd = false;

        [HideInInspector]
        public Transform destination = null;
        private Destination destName = Destination.Clock;

        public UnityEvent<Destination> onArrive;
        public UnityEvent<Destination> onDepart;

        private bool isStart = false;
        private bool reacnDestination = false;

        void Start()
        {
            speedStorage = speed;
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null && isStart)
            {
                CalculateDestination();
                distanceTravelled += speed * Time.deltaTime;

                Vector3 nextPos;
                Quaternion rotation;

                if (reverseStartEnd)
                {
                    nextPos = pathCreator.path.GetRPointAtDistance(distanceTravelled, endOfPathInstruction);
                    rotation = Quaternion.LookRotation(-pathCreator.path.GetRDirectionAtDistance(distanceTravelled), Vector3.up);
                }
                else
                {
                    nextPos = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }

                if (endOfPathInstruction == EndOfPathInstruction.Stop && distanceTravelled > 1f && !isStop && Vector3.Distance(transform.position, nextPos) < 0.01f)
                {
                    onArrive?.Invoke(destName);
                    isStop = true;
                }

                transform.position = nextPos;
                transform.rotation = rotation;
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        public void Depart()
        {
            isStart = true;
            reacnDestination = false;
            speed = 0f;
            DOTween.To(() => speed, (x) => speed = x, speedStorage, 2f);

            onDepart?.Invoke(destName);
        }

        private void CalculateDestination()
        {
            if (destination == null) return;

            float dist = Vector3.Distance(transform.position, destination.position);

            if (!reacnDestination && dist < 5f)
            {
                reacnDestination = true;
                DOTween.To(() => speed, (x) => speed = x, 0f, 2f).OnComplete(() => onArrive.Invoke(destName));
            }
        }

        public void SetDestination(Transform destTrn, Destination dest)
        {
            destination = destTrn;
            destName = dest;
        }
    }
}