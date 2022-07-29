using UnityEngine;

public class STARTERSSenderScoreEventOnTrigger : MonoBehaviour
{
    private CsoundSender csoundSender;

    private void Start()
    {
        //Gets the CsoundSender component attached to the object.
        csoundSender = GetComponent<CsoundSender>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the other object has its tag set to "Trigger"...
        if (other.CompareTag("Trigger"))
        {
            //...send score event as defined Csound Sender inspector.
            csoundSender.SendScoreEvent();
        }
    }
}
