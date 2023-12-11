using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GateOpenerTrigger : MonoBehaviour
    {
        public GameObject gateObject;
        public float time;
        public Transform destination;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Robot")
            {
                StartCoroutine(CoMoveToDestination());
            }
        }

        IEnumerator CoMoveToDestination()
        {
            float t = 0;

            while(gateObject.transform.position != destination.position)
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime / time;

                gateObject.transform.position = Vector2.Lerp(gateObject.transform.position, destination.position, t);
            }
        }
    }
}
