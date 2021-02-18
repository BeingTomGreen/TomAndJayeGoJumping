using Assets.Scripts.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Generic
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Camera Tracking:")]
        public Transform Target;
        public float MovementSmoothing;

        [Header("Constraints:")]
        public bool ConstrainCameraToMap;
        public Tilemap Tilemap;

        private Camera currentCamera;
        private float leftBound;
        private float rightBound;
        private float bottomBound;
        private float topBound;

        private void Start()
        {
            currentCamera = GetComponent<Camera>();

            float camVertExtent = currentCamera.orthographicSize;
            float camHorzExtent = currentCamera.aspect * camVertExtent;
            BoundsInt bounds = Tilemap.cellBounds;

            leftBound = bounds.min.x + camHorzExtent;
            rightBound = bounds.max.x - camHorzExtent;
            bottomBound = bounds.min.y + camVertExtent;
            topBound = bounds.max.y - camVertExtent;
        }

        private void FixedUpdate()
        {
            if (transform.position != Target.position)
            {
                // Build a vector for the target location of the camera, use the camera's current Z position so I don't fall through the world
                Vector3 targetPosition = new Vector3(Target.position.x, Target.position.y, transform.position.z);

                if (ConstrainCameraToMap == true && Tilemap != null)
                {
                    targetPosition.x = Mathf.Clamp(Target.transform.position.x, leftBound, rightBound);
                    targetPosition.y = Mathf.Clamp(Target.transform.position.y, bottomBound, topBound);
                }

                // Move (smoothly) towards target using Lerp
                transform.position = Vector3.Lerp(transform.position, targetPosition, MovementSmoothing);
            }
        }
    }
}