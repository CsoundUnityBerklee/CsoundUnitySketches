using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// Study on Noise

public class FibNoise : MonoBehaviour
{

    
    [Header("Fibonacci")] //Fibonacci Logic

    [Tooltip("'th Fibonacci Numbers")]
    [SerializeField] int _number = 12; //************

    [Tooltip("Same number as Number")]
    [SerializeField] int[] FibonacciSequence; //*********


    
    [Header("Golden Ratio")] //Golden Ratio Logic
    [SerializeField]float a;
    [SerializeField]float b;
    float _phi = 1.618f;

    [SerializeField] GameObject goldenReference;


     [Header("Sequencer")]
     [Tooltip("In Seconds")]
     [SerializeField] int _length;

    //Sequencer
    [Range(0, 89)] // just for this example
    public int _seconds;
    

 
    [Header("Spatial Audio")]
    public GameObject[] _objects; //***********

    //Audio Sources
    [SerializeField] GameObject audioObject;
   
    


    // Start is called before the first frame update
    void Start()
    {
        GetFibonacci(); // Gets the numbers of the Fibonacci Sequence
        _length = FibonacciSequence[FibonacciSequence.Length - 1]; // Gets the duration of the piece
        
        GetGoldenRatio();
        
        InvokeRepeating("OutputTime", 1f, 1f); //Counter --> One value per second

        InvokeRepeating("Sequencer", 1f, 1f);
        
        Spatializer(); //Positioning of Sound Objects

        GetObjects(); // Reference to positioned sound objects

    }

        
   
    public void GetFibonacci()
    {
        int term1 = 0;
        int term2 = 1;
        int nextTerm = 0;

        for (int i = 0; i <= (_number -1); i++)
        {
            

            if (i == 0)
            {
                FibonacciSequence[i] = i;
            }

            else if (i == 1)
            {
                FibonacciSequence[i] = i;
            }

            else
            {
                nextTerm = term1 + term2;
                term1 = term2;
                term2 = nextTerm;

                FibonacciSequence[i] = nextTerm;

            }
        }
    }

    void GetGoldenRatio(){

        a = (float)_length/_phi;
        b = (float)a / _phi;
    }

    void OutputTime()
    {
        _seconds = (int)Time.time;
    }


    void Spatializer()
    {
        
        for (int i = 0; i < FibonacciSequence.Length; i++) {

            // Randomization of Position & Rotation
            int randomX = (int)UnityEngine.Random.Range(0, FibonacciSequence.Length);
            int randomY = (int)UnityEngine.Random.Range(0, FibonacciSequence.Length);
            int randomZ = (int)UnityEngine.Random.Range(0, FibonacciSequence.Length);

            int randomRotX = (int)UnityEngine.Random.Range(0, FibonacciSequence.Length);
            int randomRotY = (int)UnityEngine.Random.Range(0, FibonacciSequence.Length);
            int randomRotZ = (int)UnityEngine.Random.Range(0, FibonacciSequence.Length);

            // Dealing with negatives
            int _offset = (int)UnityEngine.Random.Range(-1, 1); // dealing with positive and negative values

            if (_offset == 0)
            {
                _offset = 1;
            }

         
            // Instantiation
            Vector3 _position = new Vector3(FibonacciSequence[randomX] * _offset, FibonacciSequence[randomY], FibonacciSequence[randomZ] * _offset);

            Quaternion _rotation = Quaternion.Euler(FibonacciSequence[randomRotX] * _offset, FibonacciSequence[randomRotY] * _offset,
                FibonacciSequence[randomRotZ] * _offset);

            GameObject _instance = Instantiate(audioObject, _position, _rotation);
            _instance.name = "" + i;

            // Audio
           CsoundUnity _source = _instance.GetComponent<CsoundUnity>();

           
        
            


        }

    }


    void GetObjects(){

         for (int a = 0; a < (FibonacciSequence.Length); a++)
        {
            _objects[a] = GameObject.Find(a.ToString());
        }
    }


    void Sequencer()
    {
        // Write Sequence

        bool Play;
        int randomObject = (int)UnityEngine.Random.Range(0, FibonacciSequence.Length);

        Play = Array.Exists(FibonacciSequence, x => x == _seconds); //Checks if current second is a Fibonacci #
        
        if (Play){
             // Logic to determine the score event

            //1 strt dur amp cfSt cfNd bwSt bwNd PnSt  PnNd rev Atk   Rel   Fade

            float strt = 0.0f;
            
            float dur = UnityEngine.Random.Range(0, (_length-_seconds));
            
            float amp = UnityEngine.Random.Range(-50f, 0);
            
            float cfts = UnityEngine.Random.Range(20f, 4000f);
            
            float cfNd = UnityEngine.Random.Range(20f, 3000f);
           
            float bwSt = UnityEngine.Random.Range(20f, 1000f);
            
            float PnSt = UnityEngine.Random.Range(0.01f, 0.9f);
            
            float PnNd = UnityEngine.Random.Range(0.01f, 1.0f);
            
            float rev = UnityEngine.Random.Range(0.3f, 0.8f);
            
            float atk = UnityEngine.Random.Range(0.3f, 0.8f);
            
            float rel = UnityEngine.Random.Range(3.0f, 12.0f);
            
            float fade = UnityEngine.Random.Range(-26f, 3f);
            



            string CsoundEvent = String.Format("i1 {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}",
             strt, dur, amp, cfts, cfNd, bwSt, PnSt, PnNd, rev, atk, rel, fade);
            
            Debug.Log(_seconds + ": " + CsoundEvent);
            
           // Logic to send the score event
           
            _objects[randomObject].GetComponent<CsoundUnity>().SendScoreEvent(CsoundEvent);
            Play = false;
        }

        // Golden Ratio Event
        
        int goldenMoment = (int) Math.Round(a, 0);

        
        if (_seconds == goldenMoment){

        Vector3 _position = new Vector3(goldenMoment, goldenMoment, goldenMoment);
         Quaternion _rotation = Quaternion.Euler(goldenMoment, goldenMoment, goldenMoment);
        
        // Golden object, source, and clip
        GameObject goldenObject = Instantiate(goldenReference, _position, _rotation);
        CsoundUnity goldenSource = goldenObject.GetComponent<CsoundUnity>();
        //goldenSource.clip = goldenSample;
        goldenObject.name = "Golden Object";
        
        float durationGoldenMoment = (float) (_length - goldenMoment);

        string goldenString = String.Format("i1 0 {0} -10 4834 4294 450 200 0.6 0.1 .62 02.00 06.00 -22", durationGoldenMoment);
        goldenSource.SendScoreEvent(goldenString);
        Debug.Log("Golden Moment!");
        }
       
               

    }

}



