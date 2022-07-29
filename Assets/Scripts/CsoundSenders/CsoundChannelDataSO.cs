using UnityEngine;

[CreateAssetMenu(fileName = "CsoundChannelData", menuName = "Csound/ChannelData")]
public class CsoundChannelDataSO : ScriptableObject
{
    [System.Serializable]
    public struct CsoundChannelData
    {
        public string name;
        public float fixedValue;
    }

    public CsoundChannelData[] channelData;
}


