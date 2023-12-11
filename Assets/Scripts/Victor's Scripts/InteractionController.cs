using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    //Helps with interaction actions
    private bool isInRange;
    private bool isPressingToActivate;

    //Grabs the trigger that can be activated or deactivated
    private GameObject trigger;

    void Start()
    {
        //Ignores the collision between the interaction controller and the grid
        Physics.IgnoreLayerCollision(11,12);  
    }
    private void OnTriggerEnter(Collider collision)
    {
        //Checks if the colliding object is a trigger and can be triggered
        if (collision.gameObject.GetComponent<IInteractible>() != null && collision.gameObject.GetComponent<IInteractible>().Triggerable())
        {
            trigger = collision.gameObject;
            isInRange = true;
        } 
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<IInteractible>() != null && collision.gameObject.GetComponent<IInteractible>().Triggerable()) 
        {
            isInRange = false;
        }
    }
    void Update()
    {
        //if the trigger is in range & can be triggered & is deactivated, activates it
        if(isInRange && isPressingToActivate && !trigger.GetComponent<IInteractible>().CheckIfActive())
        {
            trigger.GetComponent<IInteractible>().SetActive(true);
        }
        //if the trigger is in range & can be triggered & is activated, deactivates it
        else if(isInRange && isPressingToActivate && trigger.GetComponent<IInteractible>().CheckIfActive())
        {
            trigger.GetComponent<IInteractible>().SetActive(false);
        }
    }

    //Checks if the trigger is in range
    public bool getIsInRange() 
    {
        return isInRange;
    }
    //Set if the trigger is in range
    public void setIsInRange(bool value) 
    {
        isInRange = value;
    }
    //Checks if the arachno bot made the input to change the trigger's activation/deactivation
    public bool getIsPressingToActivate()
    {
        return isPressingToActivate;
    }
    //Set the trigger active/inactive once the player made the input
    public void setIsPressingToActivate(bool value)
    {
        isPressingToActivate = value;
    }
}
