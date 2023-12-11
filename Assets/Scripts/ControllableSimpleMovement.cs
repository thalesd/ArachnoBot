using UnityEngine;

namespace Assets.Scripts
{
    public class ControllableSimpleMovement : IControllableMovement
    {
        public virtual void Move(Rigidbody rb2d, Vector2 velocity)
        {
            rb2d.velocity = velocity;
        }
    }
}
