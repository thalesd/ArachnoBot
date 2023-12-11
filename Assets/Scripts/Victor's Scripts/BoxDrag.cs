using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDrag : MonoBehaviour
{
    void FixedUpdate()
    {
      //Keeps track of the cursor on the screen
      Vector2 inputMouse = Input.mousePosition;
      Vector2 mousePos = Camera.main.ScreenToWorldPoint(inputMouse);
      RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
      
      //Checks if the object is called "Box"
      if (hit.collider != null && hit.collider.gameObject.name == "Box")
      {
         GameObject obj = hit.collider.gameObject;

         //Checks if the player is holding the left mouse button
         if (Input.GetKey(KeyCode.Mouse0))
         {
            //Allows the object to be dragged
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            obj.transform.position = mousePos;
         }
         else 
         {
            //Changes the object back to normal
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
         }
      }
    }
}
