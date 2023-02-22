﻿using DG.Tweening;
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
        float distanceTravelled;

        [SerializeField]
        private bool reverseStartEnd = false;

        [HideInInspector]
        public Transform destination = null;
        private Destination destName = Destination.Clock;

        public UnityEvent<Destination> onArrive;

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

                if (reverseStartEnd)
                {
                    transform.position = pathCreator.path.GetRPointAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.rotation = Quaternion.LookRotation(-pathCreator.path.GetRDirectionAtDistance(distanceTravelled), Vector3.up);
                }
                else
                {
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }
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