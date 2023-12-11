using UnityEngine;

namespace Assets.Scripts
{
    public interface IControllableMovement
    {
        public void Move(Rigidbody rb2d, Vector2 velocity);
    }
}
