using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blowing : MonoBehaviour
{
    public bool playerInRange; // Control when to detect Microphone Input

    // Processing Audio
    [SerializeField] int sampleWindow = 512;
    [SerializeField] float threshold = 0.1f;
    [SerializeField] float loudnessSensibility = 50;

    // Audio Source Components
    AudioClip microphoneClip;
    [SerializeField] AudioSource source;

    // Results
    float totalLoudness = 0;
    [SerializeField] float blowingRange = 0.03f;

    // Particles
    public ParticleSystem smoker;

    // CsoundUnity
    CsoundUnity csound;
    public CsoundChannelRangeSO range;

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
        smoker = GetComponent<ParticleSystem>();
        smoker.Play(); // start particles


        // Turns off at the start
        var emission = smoker.emission;
        emission.enabled = false;
    }

    void Update()
    {
        if (playerInRange)
        { // Determined through Character Collision Trigger Class
            float loudness = GetLoudnessFromMicrophone() * loudnessSensibility;
            CsoundMap.MapValueToChannelRange(range, 0.1f, 15f, loudness, csound);

            if (loudnessSensibility < threshold)
                loudness = 0;
        }
    }



    float GetLoudnessFromAudioClip(int clipPosition, AudioClip _clip)
    {

        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        _clip.GetData(waveData, startPosition);


        totalLoudness = 0; // compute loudness

        for (int i = 0; i < sampleWindow; i++)
        {

            //INTENSITY OF SIGNAL
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        float processedLoudness = totalLoudness / sampleWindow;

        ParticlesGo(processedLoudness);

        return processedLoudness;
    }

    void MicrophoneToAudioClip()
    {

        // Gets the first microphone in device list
        string microphoneName = Microphone.devices[0];
        microphoneClip = MicrophoneUtils.Start(microphoneName, true, 20, AudioSettings.outputSampleRate); //boolean --> isLooping
    }

    float GetLoudnessFromMicrophone()
    {

        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    void ParticlesGo(float pLoudness)
    {

        if (pLoudness > blowingRange)
            PlayParticles();

        else
            StopParicles();
    }




    public void PlayerInRange(bool inRange)
    {
        playerInRange = inRange;
    }

    void PlayParticles()
    {
        var emission = smoker.emission;
        emission.enabled = true;

    }

    void StopParicles()
    {
        var emission = smoker.emission;
        emission.enabled = false;
    }




}
