using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slider : MonoBehaviour
{
    public Rotate rotationSpeed;

    // Variables to set the range of movement of the slider
    float _min, _max;

    // Variable that will be sent to RealtimeRotatingObject
    float rotationsPerMinute;

    // Property to update value only when it changes (opposed to every frame)
    public float RotationsPerMinute
    {
        get
        {
            return rotationsPerMinute;
        }
        set
        {
            if (rotationsPerMinute == value) return;
            rotationsPerMinute = value;
            if (OnVariableChange != null)
            {
                OnVariableChange(rotationsPerMinute);
                Debug.Log(rotationsPerMinute);
                rotationSpeed._speed = rotationsPerMinute;

            }   
            
        }
    }

    public delegate void OnVariableChangeDelegate(float newVal);
    public event OnVariableChangeDelegate OnVariableChange;




    private void Start()
    {
        // these values were fine tuned for the crown (assuming RM is in the center)
        _min = 1.88f;
        _max = 2.75f;

     
        // Subscribing 
        OnVariableChange += VariableChangeHandler;


    }

    private void VariableChangeHandler(float newVal)
    { }


    // Update is called once per frame
    void Update()
    {

       float rawValue = Mathf.Clamp(transform.position.y, _min, _max); // clamps slider values into the permitted range

        transform.position = new Vector3(transform.position.x, rawValue, transform.position.z); // physical movement of slider

        RotationsPerMinute = 0.01f + Mathf.InverseLerp(_min, _max, rawValue); // maps to Rotations per Minute


    }
}
