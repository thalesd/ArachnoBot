using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts
{
    public class FinishLine : MonoBehaviour
    {
        public Image img;
        public Text txt;

        public float timeUntillTp;

        private bool noMoreImpulse;
        public bool stageFinished;
        private GameObject player;

        public float journeyTime;
        private float startTime;

        private float total;
        void Start()
        {
            img.color = new Color(0, 0, 0, 0);
            txt.color = new Color(255, 255, 255, 0);

        }

        public IEnumerator OpenMainMenu()
        {
            yield return new WaitForSeconds(timeUntillTp);
            SceneManager.LoadScene(0);
            Cursor.visible = true;
        }
        public IEnumerator Respawn()
        {
            yield return new WaitForSeconds(timeUntillTp - 3);
            SceneManager.LoadScene(1);
        }

        void Update()
        {
            if (stageFinished)
            {
                if(player != null)
                {
                    journeyTime += Time.deltaTime * 0.6f;

                    player.GetComponent<Animator>().SetBool("FinishedStage", true);
                    player.GetComponent<ControllableArachnoBot>().isOnGrid = true;
                    player.GetComponent<Rigidbody>().useGravity = false;
                    if(journeyTime <= 1)
                    {
                        player.transform.position = Vector3.Lerp(new Vector3(136.26f, 8.18f, 6.1f), new Vector3(136.26f, 12.63f, 6.1f), journeyTime);
                    }
                    else
                    {
                        player.transform.position = Vector3.Lerp(new Vector3(136.26f, 12.63f, 6.1f), new Vector3(136.26f, 12.63f, 12f), journeyTime - 1);
                    }
                }

                if (total <= 2)
                {
                    total += Time.deltaTime;
                }
                else
                {
                    FadeBG();
                    FadeText(total - 3);
                    StartCoroutine(OpenMainMenu());
                    total += Time.deltaTime;
                }
            }

            //TODO: Remover logica de morte do update.
            if (Player.Instance.possessedObject.gameObject.GetComponent<ControllableArachnoBot>()?.getIsDead() ?? false)
            {
                if (total <= 2)
                {
                    total += Time.deltaTime;
                }
                else
                {
                    FadeBGDead();
                    StartCoroutine(Respawn());
                    total += Time.deltaTime;
                }
            }
        }

        public void FadeBG()
        {
            img.color = new Color(0, 0, 0, total - 3);
        }
        public void FadeBGDead()
        {
            img.color = new Color(0, 0, 0, total - 3);
        }

        public void FadeText(float alpha)
        {
            txt.color = new Color(255, 255, 255, alpha - 3);
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Robot" && Input.GetAxisRaw("Vertical") > 0)
            {
                stageFinished = true;
                player = other.gameObject;
                /*if (!noMoreImpulse)
                {
                    player.GetComponent<Rigidbody>().AddForce(3f * Vector2.up, ForceMode.VelocityChange);
                    noMoreImpulse = true;
                }*/
            }
        }
    }

}