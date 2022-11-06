using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO find out about path

public class ReadSample : MonoBehaviour
{
    CsoundUnity csound;
    [SerializeField] AudioClip _clip;
    string sampleName;

    // Start is called before the first frame update
    void Start()
    {
        // Find object based on name
        csound = GameObject.Find("PlaySample_ReadingClip").GetComponent<CsoundUnity>();
        if (!csound)
        {
            Debug.LogError("Can't find CsoundUnity?");
        }

         sampleName = _clip.name; // Path goes here --> error occurs if path is changed from original example

        CreateTableMono(sampleName);

        CreateTableStereo(sampleName);

    }

    void CreateTableMono(string nameOfSample)
    {
        //var samplesMono = CsoundUnity.GetSamples(name, CsoundUnity.SamplesOrigin.Resources, 1, false);

        //if (csound.CreateTable(9001, samplesMono) == -1)
        //{
        //    Debug.LogError("Couldn't create table");
        //}

    }

    void CreateTableStereo(string nameOfSample)
    {
        //var samplesStereo = CsoundUnity.GetSamples(nameOfSample, CsoundUnity.SamplesOrigin.Resources, 1, true);

        //if (csound.CreateTable(9000, samplesStereo) == -1)
        //{
        //    Debug.LogError("Couldn't create table");
        //}

    }

}
