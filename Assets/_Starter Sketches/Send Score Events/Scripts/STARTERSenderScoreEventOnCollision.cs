using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STARTERSenderScoreEventOnCollision : MonoBehaviour
{
    private CsoundSender csoundSender;
    private float collisionImpulseMultiplier = 200;
    // Start is called before the first frame update
    void Start()
    {
        //Gets the CsoundSender component attached to the object.
        csoundSender = GetComponent<CsoundSender>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if the other object has its tag set to "Collision"
        if (collision.gameObject.CompareTag("Collision"))
        {
            //Uses the collision impulse data to alter the value of p4, making it so the weaker the collision, the lower the pitch.
            float p4 = collision.impulse.magnitude * collisionImpulseMultiplier;
            //Access the first p field array value in the current score event index defined in the Csound Sender inspector.
            csoundSender.ScoreEvents.scoreEventsList[csoundSender.ScoreEvents.scoreEventCurrentIndex].pFields[0] = p4;
            //Send score event as defined Csound Sender inspector.
            csoundSender.SendScoreEvent();
        }
    }
}
