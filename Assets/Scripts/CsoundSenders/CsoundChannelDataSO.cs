using UnityEngine;

[CreateAssetMenu(fileName = "CsoundChannelData", menuName = "Csound/ChannelData")]
public class CsoundChannelDataSO : ScriptableObject
{
    [System.Serializable]
    public struct CsoundChannelData
    {
        public string name;
        public float fixedValue, minValue, maxValue;
    }

    public CsoundChannelData[] channelData;

    public float GetRandomValue(int index, bool debug)
    {
        //If minValue and maxValue are set to 0, return fixed value...
        if((channelData[index].minValue == 0) && (channelData[index].maxValue == 0))
        {
            return channelData[index].fixedValue;
        }
        //...else generate a random number between minValue and maxValue.
        else
        {
            float value = Random.Range(channelData[index].minValue, channelData[index].maxValue);

            if (debug)
                Debug.Log("CSOUND set random value: " + channelData[index].name + " set RANDOM value " + value);

            return value;
        }
    }
}


