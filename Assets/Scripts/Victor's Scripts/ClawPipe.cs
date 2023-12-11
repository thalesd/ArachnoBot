using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helper;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClawPipe : MonoBehaviour
    {
        public GameObject claw;
        public GameObject base1;
        private Vector3 initialScale;
        public Transform lowLimit;
        public float speed;

        private bool isMoving;

        private AudioSource audSourc;

        void Start()
        {
            audSourc = GetComponent<AudioSource>();
            initialScale = transform.localScale;
        }

        public void MovingPipe()
        {
            float distanceBetweenBaseAndClaw = Vector3.Distance(base1.transform.position, claw.transform.position);
            isMoving = false;

            if (claw.transform.localPosition.y > 0f)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    MoveDown(distanceBetweenBaseAndClaw);
                }
            }
            else if (claw.transform.localPosition.y < lowLimit.localPosition.y)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    MoveUp(distanceBetweenBaseAndClaw);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.S))
                {
                    MoveDown(distanceBetweenBaseAndClaw);
                }
                if (Input.GetKey(KeyCode.W))
                {
                    MoveUp(distanceBetweenBaseAndClaw);
                }
            }
            PlaySound();
        }
        private void MoveUp(float distance)
        {
            isMoving = true;
            claw.transform.position = new Vector3(claw.transform.position.x, claw.transform.position.y + speed * Time.deltaTime, claw.transform.position.z);
            transform.position = (claw.transform.position + base1.transform.position) / 2;
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z * distance * 1.2f);
        }
        private void MoveDown(float distance)
        {
            isMoving = true;
            claw.transform.position = new Vector3(claw.transform.position.x, claw.transform.position.y - speed * Time.deltaTime, claw.transform.position.z);
            transform.position = (claw.transform.position + base1.transform.position) / 2;
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z * distance * 1.2f);
        }
        private void PlaySound()
        {
            if (!audSourc.isPlaying && isMoving)
            {
                audSourc.Play();
            }
            else if (!isMoving)
            {
                audSourc.Stop();
            }
        }
    }
}
