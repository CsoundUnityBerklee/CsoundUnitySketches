using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollision : MonoBehaviour
{

    /// <summary>
    /// Events to trigger when a player collides with this object. 
    /// </summary>
    public UnityEvent triggerEnterEvents;

    /// <summary>
    /// Events to trigger when player exits the collider on this object 
    /// </summary>
    /// 
    public UnityEvent triggerExitEvents;


    [Header("Cooldown Options")]
    [Tooltip("Amount of time after a trigger before another can occur.")]
    public float cooldownTimer = 0f;


    private bool coolingEnter;

    private bool coolingExit;


    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            
            triggerEnterEvents.Invoke();
            StartCoroutine(StartCooldownEnter());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {

            triggerExitEvents.Invoke();
            StartCoroutine(StartCooldownExit());

        }
    }

    IEnumerator StartCooldownEnter()
    {
        coolingEnter = true;
        yield return new WaitForSeconds(cooldownTimer);
        coolingEnter = false;
    }



    IEnumerator StartCooldownExit()
    {
        coolingExit = true;
        yield return new WaitForSeconds(cooldownTimer);
        coolingExit = false;
    }
}
