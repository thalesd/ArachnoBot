using UnityEngine;

namespace Assets.Scripts
{
    public class PossessionCrossHairController : MonoBehaviour
    {
        private ControllableArachnoBot _arachnobot;
        private float _possessionRadius;

        public bool useMousePosition = true;

        private void OnEnable()
        {
            if(Player.Instance?.possessedObject != null)
            {
                _arachnobot = Player.Instance.possessedObject.GetComponent<ControllableArachnoBot>();

                if (_arachnobot != null) _possessionRadius = _arachnobot.GetPossessionRadius();

            }
            else
            {
                _possessionRadius = 0;
            }
            transform.position = transform.parent.position;
        }

        void Update()
        {
            if (_possessionRadius == 0) return;

            if (useMousePosition)
            {
                var mousePos = Input.mousePosition;
                var worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
                var radiusCenter = transform.parent.position;

                var mouseFromCenter = radiusCenter + worldMousePos;
                var clampedMouseToCenter = Vector2.ClampMagnitude(worldMousePos - radiusCenter, _possessionRadius);

                transform.position = (Vector2)radiusCenter + clampedMouseToCenter;
            }
            else
            {
                var inputAxisX = transform.position.x + Input.GetAxisRaw("Horizontal");
                var inputAxisY = transform.position.y + Input.GetAxisRaw("Vertical");

                transform.position = Vector2.ClampMagnitude(new Vector2(inputAxisX, inputAxisY), _possessionRadius);
            }
        }
    }
}
