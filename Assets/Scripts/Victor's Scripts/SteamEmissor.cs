using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamEmissor : MonoBehaviour, IBoosters
{
    //
    public GameObject trigger;
    public int strenght;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null && collision.GetComponent<Rigidbody2D>().gravityScale > 0)
        {
            collision.GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null && collision.GetComponent<Rigidbody2D>().gravityScale > 0 && trigger.GetComponent<IInteractible>().CheckIfActive())
        {
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * strenght);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null) 
        {
            collision.GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }

    void Update()
    {
        if (trigger.GetComponent<IInteractible>().CheckIfActive())
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else 
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public bool CheckIfActive()
    {
        return trigger.GetComponent<IInteractible>().CheckIfActive();
    }
}
