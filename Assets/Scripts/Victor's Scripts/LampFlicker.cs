using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    private GameObject lightSource;
    public GameObject trigger;

    private float randomDelay;
    private float flickAmount;
    private float randomFlickerTime;

    void Start()
    {
        randomFlickerTime = 0.1f;
        lightSource = GameObject.Find(gameObject.name + "/Point Light");
        SetIntensity(0f);
        StartCoroutine(Flicker());
    }

    void Update()
    {
        if (trigger.GetComponent<IInteractible>().CheckIfActive() && lightSource.GetComponent<Light>().intensity < 4.26f)
        {
            Invoke("FadeIntensity", 0.1f);
        }
    }

    private IEnumerator Flicker()
    {
        flickAmount = Random.Range(2,12);
        randomDelay = Random.Range(3,6);
        for (int i = 0; i <= flickAmount; i++)
        {
            yield return new WaitForSeconds(randomFlickerTime);
            SetState();
        }
        yield return new WaitForSeconds(randomDelay);
        StartCoroutine(Flicker());
    }

    public void SetState()
    {
        if (lightSource.activeInHierarchy)
        {
            lightSource.SetActive(false);
        }
        else
        {
            lightSource.SetActive(true);
        }
    }

    public void SetIntensity(float intensity)
    {
        lightSource.GetComponent<Light>().intensity = intensity;
    }
    public void FadeIntensity()
    {
        lightSource.GetComponent<Light>().intensity += Time.deltaTime;
    }
}
