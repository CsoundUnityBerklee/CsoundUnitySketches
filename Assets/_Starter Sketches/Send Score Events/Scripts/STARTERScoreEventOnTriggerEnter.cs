using UnityEngine;

public class STARTERScoreEventOnTriggerEnter : MonoBehaviour
{
    private CsoundUnity csoundUnity;
    private string scoreEvent = "i 2 0 0.5 100 0";

    // Start is called before the first frame update
    void Start()
    {
        //Get the CsoundUnity component attached to the object.
        csoundUnity = GetComponent<CsoundUnity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the other object tag is set to "Trigger"...
        if (other.CompareTag("Trigger"))
        {
            //...send score event as defined in the variable.
            csoundUnity.SendScoreEvent(scoreEvent);
            //Prints score event on the console.
            Debug.Log("CSOUND SCORE EVENT: " + gameObject.name + " " + scoreEvent);
        }
    }
}
