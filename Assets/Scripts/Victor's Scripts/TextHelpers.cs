using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class TextHelpers : MonoBehaviour
    {
        public GameObject image;
        [SerializeField] private GameObject trigger;
        public bool trgrIsNull;
        public bool trgr;

        private SpriteRenderer _sr;

        public bool inRange;

        void Start()
        {
            _sr = image.GetComponent<SpriteRenderer>();

            _sr.color = new Color(0.7f, 0.7f, 0.7f, 0);
        }

        void Update()
        {
            if (inRange && trgr)
            {
                if (_sr.color.a == 0)
                {
                    StartCoroutine(FadeIn());
                }
            }
            else
            {
                if (_sr.color.a == 1)
                {
                    StartCoroutine(FadeOut());
                }
            }

            if (trigger == null)
            {
                trgrIsNull = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Robot")
            {
                inRange = true;

                if (other.GetComponent<ControllableArachnoBot>() != null && Player.Instance.possessedObject == other.gameObject)
                {
                    if (other.GetComponent<ControllableArachnoBot>().afterAnim)
                    {
                        if (!trgrIsNull)
                        {
                            if (!trigger.GetComponent<IInteractible>().CheckIfActive())
                            {
                                trgr = true;
                            }
                            else
                            {
                                trgr = false;
                            }
                        }
                        else
                        {
                            trgr = true;
                        }
                    }
                }
                else if (other.GetComponent<ClawBase>() && Player.Instance.possessedObject == other.gameObject)
                {
                    trgr = true;
                }
                else
                {
                    trgr = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Robot")
            {
                inRange = false;
            }
        }

        private IEnumerator FadeIn()
        {
            for (int i = 0; i <= 100; i++)
            {
                _sr.color = new Color(0.7f, 0.7f, 0.7f, (float)i / 100);
                yield return new WaitForSeconds(0.01f * Time.deltaTime);
            }
        }
        private IEnumerator FadeOut()
        {
            for (int i = 100; i >= 0; i--)
            {
                _sr.color = new Color(0.7f, 0.7f, 0.7f, (float)i / 100);
                yield return new WaitForSeconds(0.01f * Time.deltaTime);
            }
        }
    }
}
