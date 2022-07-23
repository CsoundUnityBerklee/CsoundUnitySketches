using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides methods to send data in the CsoundChannelDataSO and CsoundScoreEventSO assets through a CsoundUnity component.
/// </summary>
public class CsoundSender : MonoBehaviour
{
    [Tooltip("Reference to the CsoundUnity component to send values to. Will automatically get the component attached to the same object if left empty.")]
    public CsoundUnity csoundUnity;
    [Space]
    public CsoundSenderPresets InstrumentPresets;
    [Space]
    public CsoundSenderScoreEvents ScoreEvents;
    [Space]
    public CsoundSenderTrigger ChannelTrigger;
    [Space]
    public CsoundSenderRandomValues RandomChannelValues;

    #region UNITY LIFE CYCLE
    private void Awake()
    {
        //Gets the CsoundUnity component attached to the object if the inspector field is empty.
        if (csoundUnity == null)
        {
            csoundUnity = GetComponent<CsoundUnity>();

            if (csoundUnity == null)
                Debug.LogError("No CsoundUnity component attached to " + gameObject.name);
        }
    }

    void Start()
    {
        StartCoroutine(Initialization());
    }

    private IEnumerator Initialization()
    {
        while (!csoundUnity.IsInitialized)
        {
            Debug.Log("CSOUND NOT INITIALIZED");
            yield return null;
        }

        Debug.Log("CSOUND INITIALIZED");

        //Calls SetPreset if setPresetOnStart is true.
        if (InstrumentPresets.setPresetOnStart)
            SetPreset(InstrumentPresets.presetIndexOnStart);

        //Call SendCoreEvent is scoreEventIndexOnStart istrue.
        if (ScoreEvents.sendScoreEventOnStart)
            SendScoreEvent(ScoreEvents.scoreEventIndexOnStart);

        //Set defined trigger channel to 1 if triggerOnStart is true.
        if (ChannelTrigger.triggerOnStart)
            SetTrigger(1);

        //Calls SetChannelsToRandomValue if setChannelRandomValuesOnStart is true.
        if (RandomChannelValues.setChannelRandomValuesOnStart)
            SetChannelsToRandomValue();
    }
    #endregion

    #region PRESETS
    /// <summary>
    /// Reset values for the currently indexed ChannelData preset.
    /// </summary>
    /// <param name="index"></param>
    public void ResetPreset()
    {
        if (InstrumentPresets.debugPresets)
            Debug.Log("CSOUND " + gameObject.name + " set preset: " + InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex]);

        //Passes each channel fixed value to Csound.
        foreach (CsoundChannelDataSO.CsoundChannelData channelData in InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex].channelData)
        {
            csoundUnity.SetChannel(channelData.name, channelData.fixedValue);
        }
    }

    /// <summary>
    /// Uses the indexed ChannelData asset fixed values to set instrument presets.
    /// </summary>
    /// <param name="index"></param>
    public void SetPreset(int index)
    {
        if (InstrumentPresets.debugPresets)
            Debug.Log("CSOUND " + gameObject.name + " set preset: " + InstrumentPresets.presetList[index]);

        //Passes each channel fixed value to Csound.
        foreach (CsoundChannelDataSO.CsoundChannelData channelData in InstrumentPresets.presetList[index].channelData)
        {
            csoundUnity.SetChannel(channelData.name, channelData.fixedValue);
        }

        //Set current preset index to the passed index.
        InstrumentPresets.presetCurrentIndex = index;
    }

    /// <summary>
    /// Adds a ChannelsData asset to the preset list and sets it as a preset.
    /// </summary>
    /// <param name="channelData"></param>
    public void SetPreset(CsoundChannelDataSO channelData)
    {
        //Adds new channel data to the preset list as the last item.
        InstrumentPresets.presetList.Add(channelData);
        //Calls SetPreset passing in the last item as the index.
        SetPreset(InstrumentPresets.presetList.Count - 1);
    }

    #endregion

    #region TRIGGER
    /// <summary>
    /// Toggles the defined trigger channel value between 0 and 1.
    /// </summary>
    public void ToggleTrigger()
    {
        //Toggles the value of the trigger chanel between 0 and 1.
        ChannelTrigger.triggerValue = 1 - ChannelTrigger.triggerValue;
        //Passes value to Csound.
        csoundUnity.SetChannel(ChannelTrigger.triggerChannelName, ChannelTrigger.triggerValue);

        if(ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " trigger: " + ChannelTrigger.triggerValue);
    }

    /// <summary>
    /// Passes in a trigger channel and its current value to toggle it between 0 and 1.
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="value"></param>
    public void ToggleTrigger(string channelName, int value)
    {
        //Toggle the passed in value between 1 and 0.
        int toggledValue = 1 - value;
        //Passes value to Csound.
        csoundUnity.SetChannel(channelName, toggledValue);

        if (ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " trigger: " + channelName + " , " + toggledValue);
    }

    /// <summary>
    /// Passes a value to the defined Csound trigger channel.
    /// </summary>
    public void SetTrigger(int value)
    {
        //Sets the trigger channel value to the value passed as an argument.
        ChannelTrigger.triggerValue = value;
        //Passes value to Csound.
        csoundUnity.SetChannel(ChannelTrigger.triggerChannelName, ChannelTrigger.triggerValue);

        if (ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " csound trigger: " + ChannelTrigger.triggerValue);
    }

    /// <summary>
    /// Passes a value to the a Csound channel.
    /// </summary>
    public void SetTrigger(string channelName, int value)
    {
        //Sets the trigger channel value to the value passed as an argument.
        ChannelTrigger.triggerValue = value;
        //Passes value to Csound.
        csoundUnity.SetChannel(channelName, ChannelTrigger.triggerValue);

        if (ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " csound trigger: " + ChannelTrigger.triggerValue);
    }
    #endregion

    #region SCORE EVENTS
    /// <summary>
    /// Uses the ScoreEvent asset currently indexed to send a score event.
    /// </summary>
    public void SendScoreEvent()
    {
        csoundUnity.SendScoreEvent(ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND " + gameObject.name + " score event: " +
                ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex] + " " + ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());
    }

    /// <summary>
    /// Uses the indexed ScoreEvent asset to send a score event.
    /// </summary>
    /// <param name="index"></param>
    public void SendScoreEvent(int index)
    {
        ScoreEvents.scoreEventCurrentIndex = index;
        csoundUnity.SendScoreEvent(ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND " + gameObject.name + " score event: " +
                ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex] + " " + ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());
    }

    /// <summary>
    /// Adds a ScoreEvent asset to the list and sends it as a score event.
    /// </summary>
    /// <param name="scoreEvent"></param>
    public void SendScoreEvent(CsoundScoreEventSO scoreEvent)
    {
        csoundUnity.SendScoreEvent(scoreEvent.ConcatenateScoreEventString());

        if(ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND " + gameObject.name + " score event: " + scoreEvent + " " + scoreEvent.ConcatenateScoreEventString());
    }

    /// <summary>
    /// Adds a ScoreEvent asset to the list and sends it as a score event.
    /// </summary>
    /// <param name="scoreEvent"></param>
    public void SendScoreEventAndAddToList(CsoundScoreEventSO scoreEvent)
    {
        //Adds new ScoreEvent asset to the list as the last item.
        ScoreEvents.scoreEventsList.Add(scoreEvent);
        //Calls SendScoreEvent passing in the last item as the index.
        SendScoreEvent(ScoreEvents.scoreEventsList.Count - 1);
    }

    /// <summary>
    /// Passes in a string to be sent as a score event.
    /// </summary>
    /// <param name="scoreEvent"></param>
    public void SendScoreEvent(string scoreEvent)
    {
        csoundUnity.SendScoreEvent(scoreEvent);

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND" + gameObject.name + " score event: " + scoreEvent);
    }

    /// <summary>
    /// Passes in separate values for each p field to be sent as a score event.
    /// </summary>
    /// <param name="scorechar"></param>
    /// <param name="instrument"></param>
    /// <param name="delay"></param>
    /// <param name="duration"></param>
    public void SendScoreEvent(string scorechar, string instrument, float delay, float duration)
    {
        csoundUnity.SendScoreEvent(scorechar + " " + instrument + " " + delay + " " + duration);

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND" + gameObject.name + " score event: " + scorechar + " " + instrument + " " + delay + " " + duration);
    }

    /// <summary>
    /// Passes in separate values for each p field to be sent as a score event.
    /// </summary>
    public void SendScoreEvent(string scorechar, string instrument, float delay, float duration, float[] extraPFields)
    {
        string concatenatedPFields = string.Join(" ", extraPFields);
        csoundUnity.SendScoreEvent(scorechar + " " + instrument + " " + delay + " " + duration + " " + concatenatedPFields);

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND" + gameObject.name + " score event: " + scorechar + " " + instrument + " " + delay + " " + duration + " " + concatenatedPFields);

    }

    #endregion

    #region SET CHANNELS TO RANDOM VALUE
    /// <summary>
    /// Uses the currently indexed ChannelData asset in the randomValueChannels array to randomize values between the defined minValue and maxValue.
    /// </summary>
    public void SetChannelsToRandomValue()
    {
        if (!RandomChannelValues.useCurrentPresetRandomValues)
        {
            for (int i = 0; i < RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData.Length; i++)
            {
                //Get the random value from the scriptable object and passes it to Csound.
                csoundUnity.SetChannel(RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData[i].name,
                    RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
            }
        }
        else
        {
            for (int i = 0; i < InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex].channelData.Length; i++)
            {
                //Get the random value from the scriptable object and passes it to Csound.
                csoundUnity.SetChannel(InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex].channelData[i].name,
                    InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex].GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
            }
        }
    }

    /// <summary>
    /// Changes the indexed ChannelData asset in the randomValueChannels array and randomizes values between the defined minValue and maxValue.
    /// </summary>
    public void SetChannelsToRandomValue(int index)
    {
        RandomChannelValues.randomValueCurrentIndex = index;

        if (!RandomChannelValues.useCurrentPresetRandomValues)
        {
            for (int i = 0; i < RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData.Length; i++)
            {
                //Get the random value from the scriptable object and passes it to Csound.
                csoundUnity.SetChannel(RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData[i].name,
                    RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
            }
        }
        else
        {
            for (int i = 0; i < InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex].channelData.Length; i++)
            {
                //Get the random value from the scriptable object and passes it to Csound.
                csoundUnity.SetChannel(InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex].channelData[i].name,
                    InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex].GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
            }
        }
    }

    /// <summary>
    /// Passses in a ChannelData asset to randomize channel values between the defined minValue and maxValue.
    /// </summary>
    public void SetChannelsToRandomValue(CsoundChannelDataSO newChannelData)
    {
        for (int i = 0; i < newChannelData.channelData.Length; i++)
        {
            //Get the random value from the scriptable object and passes it to Csound.
            csoundUnity.SetChannel(newChannelData.channelData[i].name, newChannelData.GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
        }
    }

    /// <summary>
    /// Generates a random value between a minimum and maximum range and assigns that to a Csound channel.
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    public void SetChannelsToRandomValue(string channelName, float minValue, float maxValue)
    {
        //Generates random value.
        float randomValue = Random.Range(minValue, maxValue);
        //Passes value to Csound.
        csoundUnity.SetChannel(channelName, randomValue);

        if (RandomChannelValues.debugRandomChannelsValues)
            Debug.Log(gameObject.name + " channel value: " + channelName + " , " + randomValue);
    }

    /// <summary>
    /// Passes an array of string as channel names and generates an unique rnadom value for each channel within the same min and max range.
    /// </summary>
    /// <param name="channelNames"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    public void SetChannelsToRandomValue(string[] channelNames, float minValue, float maxValue)
    {
        //Passes value to Csound and generates a random value for each individual channel.
        foreach(string name in channelNames)
        {
            float randomValue = Random.Range(minValue, maxValue);
            csoundUnity.SetChannel(name, randomValue);

            if (RandomChannelValues.debugRandomChannelsValues)
                Debug.Log(gameObject.name + " channel value: " + name + " , " + randomValue);
        }
    }
    #endregion
}

[System.Serializable]
public class CsoundSenderPresets
{
    [Tooltip("Array containing ChannelData asssets to be used as instrument presets")]
    public List<CsoundChannelDataSO> presetList = new List<CsoundChannelDataSO>();
    [Tooltip("Defined which preset to be set on start")]
    public int presetIndexOnStart;
    [Tooltip("If true, sets the defined preset value on start")]
    public bool setPresetOnStart;
    [Tooltip("Prints channel names and values when changing presets.")]
    public bool debugPresets;

    [HideInInspector] public int presetCurrentIndex = 0;
}

[System.Serializable]
public class CsoundSenderScoreEvents
{
    [Tooltip("Array containing ScoreEvent asssets")]
    public List<CsoundScoreEventSO> scoreEventsList = new List<CsoundScoreEventSO>();
    [Tooltip("Defined which score event to send on start")]
    public bool sendScoreEventOnStart;
    [Tooltip("If true, sends the defined score event on start")]
    public int scoreEventIndexOnStart;
    [Tooltip("Prints score events.")]
    public bool debugScoreEvents;

    [HideInInspector] public int scoreEventCurrentIndex = 0;
}

[System.Serializable]
public class CsoundSenderTrigger
{
    [Tooltip("Defines the channel name that is used to start and stop the Csound instrument")]
    public string triggerChannelName;
    [Tooltip("If true, sets the trigger channel to a value of 1")]
    public bool triggerOnStart = false;
    [Tooltip("Prints trigger channel values.")]
    public bool debugTrigger;

    [HideInInspector] public int triggerValue = 0;
}

[System.Serializable]
public class CsoundSenderRandomValues
{
    [Tooltip("Array of ChannelData assets to be used to randomize channel values.")]
    public List <CsoundChannelDataSO> randomValueChannelsList = new List<CsoundChannelDataSO>();
    public int randomValueIndexOnStart;
    [Tooltip("If true, ignores the randomValueChannels field and uses the current preset minValues and maxValues to generate random values instead.")]
    public bool useCurrentPresetRandomValues = false;
    public bool setChannelRandomValuesOnStart = false;
    [Tooltip("Prints channel names and values when randomizing values.")]
    public bool debugRandomChannelsValues;

    [HideInInspector] public int randomValueCurrentIndex = 0;
}