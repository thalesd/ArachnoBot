using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudios : MonoBehaviour
{
    public List<AudioClip> listOfRandomAudios;
    public float minTime, maxTime;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SoundDelay());
    }

    private IEnumerator SoundDelay()
    {
        float timeRange = Random.Range(minTime, maxTime);
        int chosenSound = Random.Range(0, listOfRandomAudios.Count);

        yield return new WaitForSeconds(timeRange);

        audioSource.clip = listOfRandomAudios[chosenSound];
        audioSource.Play();

        StartCoroutine(SoundDelay());
    }
}
