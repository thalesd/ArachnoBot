using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts 
{
    public class EletricFence : MonoBehaviour
    {
        //Grabs the lightning effect and the trigger to the object
        public GameObject lightningEffect;

        [SerializeField] private GameObject trigger;
        public bool allowDeath = false;

        //Grabs the world points that will create the lightning in between
        public Transform topNode;
        public Transform bottomNode;

        //Save the original scale of the object
        private Vector2 initialScale;

        //Defines how far from each other the nodes can be to allow lightning
        public float maxDistance;

        //Helps with collision
        private bool isCollidingWithEnviroment;

        void Start()
        {
            initialScale = transform.localScale;
            UpdateScale();
        }

        void Update()
        {
            //Checks if the trigger is active / is colliding with enviroment / nodes too far and deactivates the eletric fence 
            if (trigger.GetComponent<IInteractible>().CheckIfActive() || isCollidingWithEnviroment || Vector2.Distance(topNode.position, bottomNode.position) > maxDistance)
            {
                Deactivate();
            }
            else //if none the conditions met above, activates the eletric fence
            {
                Activate();
            }

            if (topNode.hasChanged || bottomNode.hasChanged) //Updates the scale of the lightning collider
            {
                UpdateScale();
            }

            if (Input.GetKeyDown(KeyCode.K) && allowDeath)
            {
                allowDeath = false;
            }
            else if(Input.GetKeyDown(KeyCode.K) && !allowDeath)
            {
                allowDeath = true;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            //If a floor object collides with it, deactivates
            if (collision.gameObject.tag == "Floor")
            {
                Deactivate();
                isCollidingWithEnviroment = true;
            }

            //If an arachno bot object collides with it, push back that arachnobot
            if (collision.gameObject.GetComponent<ControllableArachnoBot>())
            {
                //collision.gameObject.GetComponent<ControllableArachnoBot>().KnockBack();

                if (allowDeath)
                {
                    collision.gameObject.GetComponent<ControllableArachnoBot>().setIsDead();
                }
            }
        }
        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.tag == "Floor")
            {
                isCollidingWithEnviroment = false;
            }
        }

        void UpdateScale()
        {
            //Grabs the distance between the nodes
            float distanceBetweenNodes = Vector2.Distance(topNode.position, bottomNode.position);

            //Adjust lightning collision's scale and position to fit between the nodes
            transform.localScale = new Vector2(initialScale.x, distanceBetweenNodes);
            transform.position = (topNode.position + bottomNode.position) / 2;
            transform.up = topNode.position - bottomNode.position;
        }

        //Deactivates both the lightning effect and the collider
        void Deactivate()
        {
            lightningEffect.GetComponent<LineRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }

        //Activates both the lightning effect and the collider
        void Activate()
        {
            lightningEffect.GetComponent<LineRenderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
        }
    }
}
