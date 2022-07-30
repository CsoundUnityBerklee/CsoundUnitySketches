using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
    }
}
