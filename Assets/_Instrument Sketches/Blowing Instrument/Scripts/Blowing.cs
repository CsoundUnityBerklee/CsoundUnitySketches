using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowing : MonoBehaviour
{


    public bool playerInRange;

    // Processing
    [Range(16, 1024)] //Power of 8
    [SerializeField] int sampleWindow = 512;
    [SerializeField] float threshold = 0.1f;
    [SerializeField] float loudnessSensibility = 50;
    // Audio Source Components
    AudioClip microphoneClip;
    [SerializeField] AudioSource source;
    // Results
    float totalLoudness = 0;
    //float processedLoudness;
    [SerializeField] float blowingRange = 0.03f;
    // Particles
    public ParticleSystem smoker;

    CsoundUnity csound;
    public CsoundChannelRangeSO range;
     
     // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
        smoker = GetComponent<ParticleSystem>();


        // Turns off at the start
        var emission = smoker.emission;
        emission.enabled = false;

        //Get CsoundUnity Component
        csound = GetComponent<CsoundUnity>();
    }

     void Update()
    {
        if (playerInRange){
            float loudness = GetLoudnessFromMicrophone() * loudnessSensibility;
            CsoundMap.MapValueToChannelRange(range, 0.1f, 15f, loudness, csound);


            //CsoundMap.SetCsoundChannelBasedOnAxis()
         if (loudnessSensibility < threshold){
             loudness = 0;
            } 
        }
    }
    
     float GetLoudnessFromAudioClip(int clipPosition, AudioClip _clip){

        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0){
            return 0;
        }

        float [] waveData = new float[sampleWindow];
        _clip.GetData(waveData,startPosition);

        //compute loudness

        totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++){

            //INTENSITY OF SIGNAL
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        float processedLoudness = totalLoudness / sampleWindow;

        ParticlesGo(processedLoudness);

          return processedLoudness;
    }

    void MicrophoneToAudioClip(){

        // Gets the first microphone in device list
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate); //boolean --> isLooping
    }

    float GetLoudnessFromMicrophone(){

        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    void ParticlesGo(float pLoudness){

        if (pLoudness > blowingRange)
            ParticleSystem(true);
        else
            ParticleSystem(false);
        }


    public void PlayerInRange(bool inRange){
        playerInRange = inRange;
    }

    void ParticleSystem(bool isEnabled)
    {
        var emission = smoker.emission;
        emission.enabled = isEnabled;
    }


    

}


