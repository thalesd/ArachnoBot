using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts 
{
    public class ClawBase : MonoBehaviour, IControllableEntity
    {
        //Defines the layerMask that is considered as ground
        [SerializeField] private LayerMask WhatIsGroundLayer;

        //Defines the limits which the object can move
        public Transform limitLeft;
        public Transform limitRight;

        //Grabs the references to all parts of the object
        public GameObject claw;
        public GameObject piston;
        public GameObject gears;

        //Defines the speed which the object moves
        public float speed;

        //Helps with gears animation
        private float turns;

        //Grabs object's components
        private Rigidbody _rb;
        private BoxCarrierControllableMovement _controllableMovement;
        private AudioSource audSourc;

        void Awake() 
        {
            _rb = GetComponent<Rigidbody>();
            _controllableMovement = new BoxCarrierControllableMovement();
            audSourc = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (Player.Instance.possessedObject != gameObject)
            {
                _controllableMovement.Move(_rb, new Vector2(0, 0));
                audSourc.Stop();
            }
        }
        public void Action() //See more details at IControllableEntity
        {
            //Makes so the entity's action is another object's action
            claw.GetComponent<ClawGrab>().ClawAction();
        }

        public bool CheckIfGrounded() //See more details at IControllableEntity
        {
            return true;
        }

        public void Highlight(Color highlightColor) //See more details at IControllableEntity
        {
        }

        public void Move(Vector2 direction, float speed, float deltaTime) //See more details at IControlablleEntity
        {
            //Makes so the object can only move within the delimited area
            if (transform.position.x <= limitRight.position.x && transform.position.x >= limitLeft.position.x)
            {
                //Moves the object based on Player(script) data
                _controllableMovement.Move(_rb, direction * speed * deltaTime);
                if (!audSourc.isPlaying && direction.x != 0)
                {
                    audSourc.Play();
                }
                else if(direction.x == 0)
                {
                    audSourc.Stop();
                }

                turns += direction.x;
                gears.transform.rotation = Quaternion.Euler(-gears.transform.rotation.x + turns * 8, 90, 90);
            }
            else if (transform.position.x > limitRight.position.x)
            {
                //Makes so it can only move right when reaching left limit
                if (direction.x < 0) 
                {
                    _controllableMovement.Move(_rb, direction * speed * deltaTime);
                }
                else
                {
                    _controllableMovement.Move(_rb, new Vector2(0,0));
                }
            }
            else if (transform.position.x < limitRight.position.x) //Makes so it can only move left when reaching right limit
            {
                if (direction.x > 0)
                {
                    _controllableMovement.Move(_rb, direction * speed * deltaTime);
                }
                else
                {
                    _controllableMovement.Move(_rb, new Vector2(0, 0));
                }
            }

            //Activates pipe movement on entity's script
            piston.gameObject.GetComponent<ClawPipe>().MovingPipe();
        }

        public void Possess() //See more details at IControllableEntity
        {
        }

        public void StopHighlight() //See more details at IControllableEntity
        {
        }

        public void StopPossessing() //See more details at IControlableEntity
        {
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
    }
}
