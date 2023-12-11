using Assets.Scripts.Helper;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ControllableBoxCarrier : MonoBehaviour, IControllableEntity
    {
        [SerializeField] private LayerMask WhatIsGroundLayer;

        [SerializeField] private float JumpSpeed;
        public float objectSpeed;
        public float animationSpeed;

        private Rigidbody _rb;
        [SerializeField] private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private BoxCarrierControllableMovement _controllableMovement;
        private AudioSource audSourc;

        private List<GameObject> carriedItems;

        [SerializeField] private Animator _animator;

        public void Start() 
        {
            carriedItems = new List<GameObject>();
        }

        void Update()
        {
            if(Player.Instance.possessedObject != gameObject)
            {
                _animator.speed = 0;
                audSourc.Stop();
            }
        }
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            audSourc = GetComponent<AudioSource>();

            _controllableMovement = new BoxCarrierControllableMovement();
        }

        public void OnTriggerEnter(Collider other)
        {
            carriedItems.Add(other.gameObject);
        }
        public void OnTriggerExit(Collider other)
        {
            carriedItems.Remove(other.gameObject);
        }

        public void Action()
        {
            if(CollisionHelper.CheckIfGrounded(_collider, WhatIsGroundLayer))
                _controllableMovement.Jump(_rb, Vector2.up * JumpSpeed * Time.deltaTime);
        }

        public bool CheckIfGrounded()
        {
            if (_rb.useGravity)
            {
                return CollisionHelper.CheckIfGrounded(_collider, WhatIsGroundLayer);
            }

            return false;
        }

        public void Move(Vector2 direction, float speed, float deltaTime)
        {
            _controllableMovement.Move(_rb, direction * speed * deltaTime);
            if (direction.x > 0)
            {
                foreach (GameObject item in carriedItems)
                {
                    item.transform.position += new Vector3(objectSpeed * Time.deltaTime, 0, 0);
                }
                _animator.speed = animationSpeed;
                _animator.Play("Base Layer.Moving");
            }
            else if (direction.x < 0)
            {
                foreach (GameObject item in carriedItems)
                {
                    item.transform.position -= new Vector3(objectSpeed * Time.deltaTime, 0, 0);
                }
                _animator.speed = animationSpeed;
                _animator.Play("Base Layer.ReverseMoving");
            }
            else if (direction.x == 0)
            {
                _animator.speed = 0;
            }

            if (!audSourc.isPlaying && direction.x != 0)
            {
                audSourc.Play();
            }
            else if (direction.x == 0)
            {
                audSourc.Stop();
            }
        }

        public void Possess()
        {
            //_spriteRenderer.color = DefaultValues.PossessedColor;
        }

        public void Highlight(Color highlightColor)
        {
            //Make code to highlight game object when mouseOver or interactable overlay is required
            //_spriteRenderer.color = highlightColor;
        }

        public void StopHighlight()
        {
            //_spriteRenderer.color = DefaultValues.UnpossessedColor;
        }

        public void StopPossessing()
        {
            //_spriteRenderer.color = DefaultValues.UnpossessedColor;
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
