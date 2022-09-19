using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STARTERSScoreEventOnCollision : MonoBehaviour
{
    private CsoundUnity csoundUnity;
    private string scoreEventBase = "i 2 0 0.3";
    private float p4;
    private float collisionImpulseMultiplier = 100;

    // Start is called before the first frame update
    void Start()
    {
        //Get the CsoundUnity component attached to the object.
        csoundUnity = GetComponent<CsoundUnity>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if the other object has its tag set to "Collision"
        if (collision.gameObject.CompareTag("Collision"))
        {
            //Uses the collision impulse data to alter the value of p4, making it so the weaker the collision, the lower the pitch.
            float p4 = collision.impulse.magnitude * collisionImpulseMultiplier;
            //Concatenates the base score event variable with the dynamic p4 value.
            string scoreEventConcatenated = scoreEventBase + " " + p4;
            //Passes concatenated score event to Csound.
            csoundUnity.SendScoreEvent(scoreEventConcatenated);
            //Prints concatenated score event in the console.
            Debug.Log("CSOUND SCORE EVENT: " + gameObject.name + " " + scoreEventConcatenated);
        }
    }
}
