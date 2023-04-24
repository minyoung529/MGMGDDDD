using DG.Tweening;
using DG.Tweening.Core.Easing;
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

        public bool reverseStartEnd = false;

        [HideInInspector]
        public Vector3 destination = Vector3.zero;
        private Destination destName = Destination.Clock;

        [SerializeField]
        private float duration = -1f;

        public UnityEvent<Destination> onArrive;
        public UnityEvent<Destination> onDepart;

        private bool isStart = false;
        private bool reachDestination = false;

        [SerializeField]
        private Ease moveEase = Ease.Unset;

        public Vector3 EndPoint => pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);
        public Vector3 StartPoint => pathCreator.path.GetPoint(0);

        public Vector3 offset = Vector3.zero;

        void Awake()
        {
            if (onArrive == null || onArrive.GetPersistentEventCount() == 0)
            {
                onArrive = new UnityEvent<Destination>();
            }

            if (duration > 0f)
            {
                speed = pathCreator.path.length / duration;
            }

            speedStorage = speed;

            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        public void ReasetData()
        {
            isStop = false;
            distanceTravelled = 0f;
            isStart = true;
            reachDestination = false;
        }

        public void StartFollowing()
        {
            isStart = true;
        }

        void Update()
        {
            if (pathCreator != null && isStart)
            {
                CalculateDestination();
                //float ease = DOVirtual.EasedValue(0f, 1f, distanceTravelled / pathCreator.path.length, moveEase);
                distanceTravelled += speed * Time.deltaTime /** ease*/;

                Vector3 nextPos;
                Quaternion rotation;

                if (reverseStartEnd)
                {
                    nextPos = pathCreator.path.GetRPointAtDistance(distanceTravelled, endOfPathInstruction) - offset;
                    rotation = /*Quaternion.LookRotation(pathCreator.path.GetRDirectionAtDistance(distanceTravelled), Vector3.up);*/
                        pathCreator.path.GetRRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }
                else
                {
                    nextPos = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) - offset;
                    rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }

                Debug.Log($"{name} DIST => {Vector3.Distance(transform.position, destination)}");
                if (endOfPathInstruction == EndOfPathInstruction.Stop && distanceTravelled > 1f && !isStop && Vector3.Distance(transform.position, nextPos) < 0.01f)
                {
                    onArrive.Invoke(destName);
                    isStop = true;
                    isStart = false;
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
            reachDestination = false;
            isStop = false;
            speed = 0f;
            DOTween.To(() => speed, (x) => speed = x, speedStorage, 2f);
            distanceTravelled = 0f;
            onDepart?.Invoke(destName);

            if (reverseStartEnd)
            {
                transform.position = pathCreator.path.GetRPointAtDistance(distanceTravelled, endOfPathInstruction);
            }
            else
            {
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        private void CalculateDestination()
        {
            if (destination == null) return;

            float dist = Vector3.Distance(transform.position, destination);

            if (!reachDestination && dist < 5f)
            {
                reachDestination = true;
                DOTween.To(() => speed, (x) => speed = x, 0f, 2f);
            }
        }

        public void SetDestination(Transform destTrn, Destination dest)
        {
            destination = destTrn.position;
            destName = dest;
        }
    }
}