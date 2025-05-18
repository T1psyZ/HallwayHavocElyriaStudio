using UnityEngine;

public class PlayerWalkOnly : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rgbd2d;
    public VirtualJoystick joystick;
    public Animator animator;

    public Vector2 movement;

    void Update()
    {
        movement.x = joystick.HorizontalRaw();
        movement.y = joystick.VerticalRaw();

        // Set animation parameters
        if (movement != Vector2.zero)
        {
            animator.SetFloat("InputX", movement.x);
            animator.SetFloat("InputY", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void FixedUpdate()
    {
        rgbd2d.MovePosition(rgbd2d.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
