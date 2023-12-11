using UnityEngine;

namespace Assets.Scripts
{
    public class BoxCarrierControllableMovement : ControllableSimpleMovement
    {
        public void Jump(Rigidbody rb2d, Vector2 jumpVelocity)
        {
            rb2d.AddForce(jumpVelocity * Vector2.up);
        }

        public override void Move(Rigidbody rb2d, Vector2 velocity)
        {
            rb2d.velocity = new Vector2(velocity.x, rb2d.velocity.y);
        }
    }
}
