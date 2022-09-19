using UnityEngine;

public class STARTERScoreEventOnStart : MonoBehaviour
{
    private CsoundUnity csoundUnity;
    private string scoreEvent = "i 2 1 2 650 0";

    // Start is called before the first frame update
    void Start()
    {
        //Get the CsoundUnity component attached to the object.
        csoundUnity = GetComponent<CsoundUnity>();
        //Send the score event string defined as a variable.
        csoundUnity.SendScoreEvent(scoreEvent);

        //Print score event on the console.
        Debug.Log("CSOUND SCORE EVENT: " + gameObject.name + " " + scoreEvent);
    }
}
