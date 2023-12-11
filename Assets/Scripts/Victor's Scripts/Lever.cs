using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractible
{
    public GameObject lever;
    public SpriteRenderer center;
    public int time;
    public Transform destination;
    private Vector2 originPosition;
    private Quaternion originRotation;
    private bool isActivated;

    void Start()
    {
        originPosition = lever.transform.position;
        originRotation = lever.transform.rotation;
    }

    void Update()
    {
        if (isActivated)
        {
            lever.transform.position = Vector2.Lerp(lever.transform.position, destination.position, time * Time.deltaTime);
            lever.transform.rotation = Quaternion.Lerp(lever.transform.rotation, destination.rotation, time * Time.deltaTime);
        }
        else
        {
            lever.transform.position = Vector2.Lerp(lever.transform.position, originPosition, time * Time.deltaTime);
            lever.transform.rotation = Quaternion.Lerp(lever.transform.rotation, originRotation, time * Time.deltaTime);
        }
    }

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

        if (value)
        {
            center.color = new Color(0, 255, 0);
        }
        else
        {
            center.color = new Color(255, 0, 0);
        }
    }
}
