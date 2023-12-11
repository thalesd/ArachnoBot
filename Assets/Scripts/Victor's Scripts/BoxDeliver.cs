using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDeliver : MonoBehaviour
{
    public GameObject box;
    public Transform spawnNewBoxPos;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Box")
        {
            Destroy(collision.gameObject);
            Instantiate(box, spawnNewBoxPos.position, Quaternion.Euler(0,0,0));
        }
    }
}
