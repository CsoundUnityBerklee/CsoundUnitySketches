using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class STARTCScoreEventCycleOnCoroutine : MonoBehaviour
{
    private CsoundSender csoundSender;
    
    // Start is called before the first frame update
    void Start()
    {
        csoundSender = GetComponent<CsoundSender>();
        StartCoroutine(ScoreEventCycle());
    }

    private IEnumerator ScoreEventCycle()
    {
        //Sends Score Event and increment index.
        csoundSender.SendRandomScoreEvent();
        //Waits 0.5 seconds between score events.
        yield return new WaitForSeconds(0.5f);
        //Calls the coroutine again to repeat the cycle.
        StartCoroutine(ScoreEventCycle());
    }
}
