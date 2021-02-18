using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Generic
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Motor : MonoBehaviour
    {
        [Header("Movement Properties:")]
        public float Speed;

        protected Rigidbody2D rb;

        protected float movementX { get; private set; }

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            movementX = 0f;
        }

        public void SetMovementVector(Vector2 inputVector)
        {
            movementX = inputVector.normalized.x * Speed;
        }

        protected void Move()
        {
            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Move();
        }
    }
}