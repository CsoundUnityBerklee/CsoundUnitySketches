using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsoundOnCollisionEnter : MonoBehaviour
{
    [SerializeField] private CsoundUnity csound;
    [SerializeField] private string triggerChannel;
    private int toggle;

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound();
    }

    public void PlaySound()
    {
        toggle = 1 - toggle;
        csound.SetChannel(triggerChannel, toggle);
    }
}
