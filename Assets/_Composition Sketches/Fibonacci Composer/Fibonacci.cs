using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Dhomont-inspired Fibonacci Piece
//The13thRabbit <— 13 chars 
//Duration: 2:24 —> (144 seconds) 13th number of the Fibonacci sequence

public class Fibonacci : MonoBehaviour
{

    
    [Header("Fibonacci")] //Fibonacci Logic

    [Tooltip("'th Fibonacci Numbers")]
    [SerializeField] int _number = 13; //************

    [Tooltip("Same number as Number")]
    [SerializeField] int[] FibonacciSequence; //*********


    
    [Header("Golden Ratio")] //Golden Ratio Logic
    [SerializeField]float a;
    [SerializeField]float b;
    float _phi = 1.618f;

    [SerializeField] AudioClip goldenSample;
    [SerializeField] GameObject goldenReference;


     [Header("Sequencer")]
     [Tooltip("In Seconds")]
     [SerializeField] int _length;

    //Sequencer
    [Range(0, 144)] // just for this example
    public int _seconds;
    

 
    [Header("Spatial Audio")]
    public GameObject[] _objects; //***********

    //Audio Sources
    [SerializeField] GameObject audioObject;
    [SerializeField] AudioClip [] _clips;
    


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
           AudioSource _source = _instance.GetComponent<AudioSource>();
            _source.clip = _clips[i];
            


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

        // Normal Sequence

        switch(_seconds){
            
            case 0: 
            _objects[0].GetComponent<AudioSource>().Play();
            break;

             case 1: 
            _objects[1].GetComponent<AudioSource>().Play();
            _objects[2].GetComponent<AudioSource>().Play();
            break;

            case 2: 
            _objects[3].GetComponent<AudioSource>().Play();
            break;

            case 3: 
            _objects[4].GetComponent<AudioSource>().Play();
            break;

            case 5: 
            _objects[5].GetComponent<AudioSource>().Play();
            break;

            case 8: 
            _objects[6].GetComponent<AudioSource>().Play();
            break;

            case 13: 
            _objects[7].GetComponent<AudioSource>().Play();
            break;

            case 21: 
            _objects[8].GetComponent<AudioSource>().Play();
            break;

            case 34: 
            _objects[9].GetComponent<AudioSource>().Play();
            break;

            case 55: 
            _objects[10].GetComponent<AudioSource>().Play();
            break;

            case 89: 
            _objects[11].GetComponent<AudioSource>().Play();
            break;

            case 144: 
            for (int i = 0; i < _objects.Length; i ++){
                // Turns off all Audio Sources
                _objects[i].GetComponent<AudioSource>().Stop();
            }
            break;

            // Restarts Piece
            case 150:
            _seconds = 0;
            break;

        }

        // Golden Ratio Event
        
        int goldenMoment = (int) Math.Round(a, 0);

        
        if (_seconds == goldenMoment){

        Vector3 _position = new Vector3(goldenMoment, goldenMoment, goldenMoment);
         Quaternion _rotation = Quaternion.Euler(goldenMoment, goldenMoment, goldenMoment);
        
        // Golden object, source, and clip
        GameObject goldenObject = Instantiate(goldenReference, _position, _rotation);
        AudioSource goldenSource = goldenObject.GetComponent<AudioSource>();
        goldenSource.clip = goldenSample;
        goldenObject.name = "Golden Object";
        goldenSource.Play();
        Debug.Log("Golden Moment!");
        }
       
               

    }

}



