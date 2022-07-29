using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //Sends currently indexed score event from the list in the inspector.
        csoundSender.SendScoreEvent();
        //Waits for the current score event duration.
        yield return new WaitForSeconds(csoundSender.ScoreEvents.scoreEventsList[csoundSender.ScoreEvents.scoreEventCurrentIndex].p3Duration);
        //Reset index back to 0 if it reaches the end of the list.
        if(csoundSender.ScoreEvents.scoreEventCurrentIndex == csoundSender.ScoreEvents.scoreEventsList.Count - 1)
        {
            csoundSender.ScoreEvents.scoreEventCurrentIndex = 0;
        }
        else
        {
            csoundSender.ScoreEvents.scoreEventCurrentIndex++;
        }
        //Repeats the cycle
        StartCoroutine(ScoreEventCycle());
    }
}
