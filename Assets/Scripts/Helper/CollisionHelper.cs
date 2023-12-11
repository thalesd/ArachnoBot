using UnityEngine;

namespace Assets.Scripts.Helper
{
    public class CollisionHelper
    {
        public static bool CheckIfGrounded(Collider collider, LayerMask whatIsGroundLayer)
        {
            var collision = Physics.BoxCast(
                        collider.bounds.center,
                        collider.bounds.size / 4,
                        Vector3.down,
                        Quaternion.identity,
                        .7f,
                        whatIsGroundLayer
                    );
            //return collision.collider?.gameObject != null;
            return collision;
        }

        public static bool isLayerInLayerMask(int gamObjectLayer, LayerMask layerMask)
        {
            return ((1 << gamObjectLayer) & layerMask) > 0;
        }
    }
}
