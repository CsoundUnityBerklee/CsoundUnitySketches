using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSamples : MonoBehaviour
{

    [SerializeField] AudioClip[] audioClips;
    AudioSource _source;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = audioClips[0];
        _source.Play();
    }

    public void ChangeClip()
    {
        _source.clip = audioClips[Random.Range(0, audioClips.Length)];
        _source.Play();
    }
}
