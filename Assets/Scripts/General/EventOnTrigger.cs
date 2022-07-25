using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    public string tag;
    public UnityEvent eventOnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag))
            eventOnTrigger?.Invoke();
        else if (tag == null)
            eventOnTrigger?.Invoke();
    }
}
