using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jumping Properties:")]
    public float JumpForce;
    public Transform GroundCheckTransform;
    public LayerMask GroundLayers;

    private float groundCheckRadius = 0.5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public bool IsGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(GroundCheckTransform.position, groundCheckRadius, GroundLayers);

        if (groundCheck != null)
        {
            return true;
        }

        return false;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
}
