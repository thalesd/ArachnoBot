using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IInteractible
{
    //Grabs the moving part of the button
    public GameObject button;

    //Grabs how low can the moving part go
    public Transform lowerLimit;

    //Defines how fast the moving part will move
    public int time;

    //Sets the original position of the moving part
    private Vector3 origin;

    //Tells if the object is activated or not
    private bool isActivated;

    void OnTriggerEnter(Collider collider) 
    {
        //detect if a box tag object is colliding with the object
        if (collider.gameObject.tag == "Box") 
        {
            isActivated = true;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Box")
        {
            isActivated = false;
        }
    }

    void Start()
    {
        origin = button.transform.position;
    }

    void Update() 
    {
        //Moves the moving part based on the object being active or not
        if (isActivated)
        {
            button.transform.position = Vector3.Lerp(button.transform.position, lowerLimit.position, time * Time.deltaTime);
        }
        else
        {
            button.transform.position = Vector3.Lerp(button.transform.position, origin, time * Time.deltaTime);
        }
    }

    //See IInteractible for more details
    public bool Triggerable()
    {
        return false;
    }

    //See IInteractible for more details
    public bool CheckIfActive()
    {
        return isActivated;
    }

    //See IInteractible for more details
    public void SetActive(bool value)
    {
        isActivated = value;
    }
}

