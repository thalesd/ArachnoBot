using System;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IControllableEntity
    {
        //Check if the entity is on the ground
        bool CheckIfGrounded();

        //Moves the entity
        void Move(Vector2 direction, float speed, float deltaTime);

        //Makes the entity interact(each entity with its own action)
        void Action();

        //Posesses the selected entity
        void Possess();

        //Returns to the main arachno bot
        void StopPossessing();

        //Hightlights the object
        void Highlight(Color highlightColor);

        //Stop an object's hightlight
        void StopHighlight();

        void Interact();

        void Jump();

        void PlaySound(int soundNumber);
    }
}
