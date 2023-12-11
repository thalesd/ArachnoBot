using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractible
{
    public GameObject button; //Grabs the gameobject that represents the button
    public bool isActivated; //To turn the machine that uses this object on and off

    public Animator anim; //Grabs the button animator

    //See IInteractible for more details
    public bool Triggerable()
    {
        return true;
    }
    public bool CheckIfActive()
    {
        return isActivated;
    }
    public void SetActive(bool value)
    {
        isActivated = value;
        anim.SetBool("Activated", value);
    }
}
