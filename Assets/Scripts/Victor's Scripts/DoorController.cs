using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //Grabs the activator that will trigger the object
    public GameObject activator;

    //Grabs the object's animator
    public Animator _animator;

    //Grabs all the particle effects
    public List<ParticleSystem> steamEmissors;

    void Start()
    {
        DeactivateSteamEmissors();
    }
    void Update()
    {
        //plays the corresponding animation based on the trigger's activation value
        if (activator.GetComponent<IInteractible>().CheckIfActive())
        {
            _animator.Play("Base Layer.Open");
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            _animator.Play("Base Layer.Close");
        }
    }
    private void ActivateSteamEmissors() 
    {
        //Loops each particle in the list and play it
        foreach (ParticleSystem emissor in steamEmissors) 
        {
            emissor.Play();
        }
    }
    private void DeactivateSteamEmissors()
    {
        //Loops each particle in the list and stop it
        foreach (ParticleSystem emissor in steamEmissors)
        {
            emissor.Stop();
        }
    }
}
