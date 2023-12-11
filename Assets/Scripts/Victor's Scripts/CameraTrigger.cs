using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    //Grabs the camera
    public GameObject cam;

    //Points that will be use to translate the camera from one side to another
    public Transform pointA;
    public Transform pointB;

    //Determines if the player has gone trought the trigger
    public bool colliding;

    //Determines if the camera is translating to point A
    public bool toA;

    //Determines the speed which the camera travels from point A to point B
    public float speed;

    public void Start()
    {
        toA = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Change the variables based on the coliision trigger
        if (other.gameObject.tag == "Robot" && toA == false)
        {
            colliding = true;
            toA = true;
        }
        else if (other.gameObject.tag == "Robot")
        {
            colliding = true;
            toA = false;
        }
        else 
        {
            colliding = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Robot")
        {
            colliding = false;
        }
    }

    //Lerps the camera from point A to point B
    void GoToB() 
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, pointB.transform.position, speed * Time.fixedDeltaTime);
    }
    //Lerps the camera from point B to point A
    void GoToA()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, pointA.transform.position, speed * Time.fixedDeltaTime);
    }
    public void Update() 
    {
        //Changes the camera's position based on the variables
        if (toA && colliding)
        {
            GoToA();
        }
        else if(colliding)
        {
            GoToB();
        }
    }
}
