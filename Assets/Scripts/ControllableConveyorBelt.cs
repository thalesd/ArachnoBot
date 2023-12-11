using UnityEngine;
using Assets.Scripts.Helper;
using System.Linq;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ControllableConveyorBelt : MonoBehaviour, IControllableEntity
    {
        [SerializeField] private LayerMask WhatIsGroundLayer;

        private Rigidbody _rb;
        [SerializeField] private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private IControllableMovement _controllableMovement;
        private SurfaceEffector2D _surfaceEffector2D;
        private AudioSource audSourc;

        public bool conveyorBeltIsOn = false;
        public float conveyorBeltSpeed = 0.2f;

        private bool wasGroundedLastUpdate = false;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _surfaceEffector2D = GetComponent<SurfaceEffector2D>();

            _controllableMovement = new ControllableSimpleMovement();

            //_collider.usedByEffector = true;
            _surfaceEffector2D.speed = conveyorBeltIsOn ? conveyorBeltSpeed : 0f;
        }

        public bool CheckIfGrounded()
        {
            if (!_rb.isKinematic)
            {
                return CollisionHelper.CheckIfGrounded(_collider, WhatIsGroundLayer);
            }

            return false;
        }

        public void Move(Vector2 direction, float speed, float deltaTime)
        {
            if(CheckIfGrounded())
            {
                _controllableMovement.Move(_rb, direction * speed * deltaTime * new Vector2(1, 0));
                wasGroundedLastUpdate = true;
            }
            else if (wasGroundedLastUpdate)
            {
                _rb.velocity = Vector2.zero;

                wasGroundedLastUpdate = false;
            }

            if (direction.x > 0) _surfaceEffector2D.speed = conveyorBeltSpeed;
            else if(direction.x < 0) _surfaceEffector2D.speed = -conveyorBeltSpeed;
        }

        public void Action()
        {
            if (conveyorBeltIsOn) DeactivateConveyorBelt();
            else ActivateConveyorBelt();

        }

        public void Possess()
        {
            _spriteRenderer.color = DefaultValues.PossessedColor;
        }

        public void Highlight(Color highlightColor)
        {
            //Make code to highlight game object when mouseOver or interactable overlay is required
            _spriteRenderer.color = highlightColor;
        }

        public void StopHighlight()
        {
            _spriteRenderer.color = DefaultValues.UnpossessedColor;
        }

        public void StopPossessing()
        {
            _spriteRenderer.color = DefaultValues.UnpossessedColor;
        }

        private void ActivateConveyorBelt()
        {
            conveyorBeltIsOn = true;
            _surfaceEffector2D.speed = conveyorBeltSpeed;
        }

        private void DeactivateConveyorBelt()
        {
            conveyorBeltIsOn = false;
            _surfaceEffector2D.speed = 0;
        }

        #region EditorOnly
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, .8f);
            Gizmos.DrawCube(new Vector2(_collider.bounds.center.x, _collider.bounds.min.y - .1f), new Vector2(_collider.bounds.size.x, .2f));
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }

        public void Jump()
        {
            throw new System.NotImplementedException();
        }

        public void PlaySound(int soundNumber)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
