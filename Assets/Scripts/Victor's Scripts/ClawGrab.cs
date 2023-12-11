using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawGrab : MonoBehaviour
{
    //Grabs the claw objects 
    public GameObject rightClaw;
    public GameObject leftClaw;

    //Stores the object that is going to be grabbed
    private GameObject stickObject;

    //Grabs the entity's animator 
    public Animator anim;

    //Defines if the claw is opened or closed
    public bool isGrabbing;

    void Start()
    {
        Physics.IgnoreLayerCollision(11, 10);
    }
    public void Update()
    {
        //Sets the animation of the claw based on isGrabbing variable
        anim.SetBool("isGrabbing", isGrabbing);

        //If the claw is grabbing something, it will deactivate both claw collisions 
        if (stickObject != null && isGrabbing) 
        {
            Physics.IgnoreCollision(rightClaw.GetComponent<Collider>(), stickObject.GetComponent<Collider>());
            Physics.IgnoreCollision(leftClaw.GetComponent<Collider>(), stickObject.GetComponent<Collider>());
        }
        
    }

    //Makes the claw grab the object
    void Grab(GameObject obj) 
    {
        //Makes the grabbed object parent of claw, makes it kinematic and a trigger
        if (obj != null) 
        {
            obj.GetComponent<Collider>().isTrigger = true;
            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.transform.parent = transform;
        }
    }

    //Releases the object that is being grabbed by the claw
    void Release(GameObject obj)
    {
        //Removes the object from parent and makes it dynamic
        if (obj != null)
        {
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.transform.parent = null;
        }
    }

    //Switch for isGrabbing and activates grab & release functions
    public void ClawAction() 
    {
        anim.SetBool("Activated", true);
        if (isGrabbing)
        {
           isGrabbing = false;
        }
        else
        {
           isGrabbing = true;
        }

        if (isGrabbing)
        {
            Grab(stickObject);
        }
        else
        {
            Release(stickObject);
        }
    }

    void OnTriggerStay(Collider collision)
    {
        //Defines stick object as the object that collided that is also tagged with box
        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Robot")
        {
           stickObject = collision.gameObject;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Robot")
        {
            collision.GetComponent<Collider>().isTrigger = false;
            stickObject = null;
        }
    }
}
