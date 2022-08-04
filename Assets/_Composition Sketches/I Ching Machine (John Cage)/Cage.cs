// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Cage : MonoBehaviour
// {

//     //Samples
//     [SerializeField] AudioClip [] _clips;
   
//     //Channels
//     [Range(0, 16)]
//     [SerializeField] int numberOfChannels;
//     [SerializeField] int density;

//     //Sequencer
//     [Range(0, 1000)]
//     [SerializeField] int _length;
//     public int _seconds;
//     bool play = false;


//     //Spatializer
//     [SerializeField] float maxDistance;
//     [SerializeField] float maxRotation;


     

//     // Start is called before the first frame update
//     void Start()
//     {
//         InvokeRepeating("OutputTime", 1f, 1f); //Counter --> One value per second
//         InvokeRepeating("Sequencer", 1f, 1f);

//         int[][] startEvents = new int[numberOfChannels][];

   
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     void OutputTime()
//     {
//         _seconds = (int)Time.time;
//     }

//         void Sequencer()
//     {
//         // Write Sequence

        


//         //Play = Array.Exists(FibonacciSequence, x => x == _seconds); //Checks if current second is a Fibonacci #
        
//         if (Play){
//              // Logic to determine the score event


            
//             //Debug.Log(_seconds + ": " + CsoundEvent);
            
//            // Logic to send the score event
           
//             //_objects[randomObject].GetComponent<CsoundUnity>().SendScoreEvent(CsoundEvent);
//             Play = false;
//         }

//     }

//      void Spatializer()
//     {
        
//         for (int i = 0; i < numberOfChannels; i++) {

//             // Randomization of Position & Rotation
//             int randomX = (int)UnityEngine.Random.Range(-maxDistance, maxDistance);
//             int randomY = (int)UnityEngine.Random.Range(-maxDistance, maxDistance);
//             int randomZ = (int)UnityEngine.Random.Range(-maxDistance, maxDistance);

//             int randomRotX = (int)UnityEngine.Random.Range(-maxRotation, maxRotation);
//             int randomRotY = (int)UnityEngine.Random.Range(-maxRotation, maxRotation);
//             int randomRotZ = (int)UnityEngine.Random.Range(-maxRotation, maxRotation);


         
//             // Instantiation
//             Vector3 _position = new Vector3(randomX, randomY, randomZ);

//             Quaternion _rotation = Quaternion.Euler(randomRotX, randomRotY, randomRotZ);
              

//             GameObject _instance = Instantiate(audioObject, _position, _rotation);
//             _instance.name = "Channel " + i;

//             // Audio
//            AudioSource _source = _instance.GetComponent<AudioSource>();
//             _source.clip = _clips[(int)UnityEngine.Random.Range(0, _clips.Length)];
            


//         }

//     }

// }


