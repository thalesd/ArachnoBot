using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    //Defines if the object can be triggered by an input
    bool Triggerable();

    //Checks if the interactible has been activated or not
    bool CheckIfActive();

    //Set the Interactible as activated or deactivated
    void SetActive(bool value);
}
