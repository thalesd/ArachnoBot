using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEmitter : MonoBehaviour
{
    public List<AudioSource> sources;
    public List<AudioClip> audioClipList;

    public void PlaySound(int clip)
    {
        foreach(AudioSource source in sources)
        {
            if(source.clip == null)
            {
                source.clip = audioClipList[clip];
                source.Play();
                break;
            }
        }
    }

    public void StopSound(int clip)
    {
        foreach (AudioSource source in sources)
        {
            if (source.clip == audioClipList[clip])
            {
                source.Stop();
                source.clip = null;
                break;
            }
        }
    }
}
