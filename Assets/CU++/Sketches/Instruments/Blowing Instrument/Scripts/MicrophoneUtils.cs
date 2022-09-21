using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by foma
/// https://github.com/fomalsd
/// </summary>
public class MicrophoneUtils
{
    private static Dictionary<string, RecordingParameters> ongoingRecordings = new Dictionary<string, RecordingParameters>();

    public static AudioClip Start(
        string deviceName,
        bool loop,
        int lengthSec,
        int frequency)
    {

        if (IsRecording(deviceName))
        {
            if (!ongoingRecordings.ContainsKey(deviceName))
            {
                Debug.LogError("Something is using Microphone class directly to record, please change it to use MicrophoneUtils instead!");
                End(deviceName);
                return StartNewRecording();
            }

            var ongoingRecording = ongoingRecordings[deviceName];
            if (loop != ongoingRecording.loop
                || lengthSec != ongoingRecording.lengthSec
                || frequency != ongoingRecording.frequency)
            {
                Debug.LogWarningFormat("MicUtils: attempting to record same device from another place " +
                                       "but recording params don't quite match:\ncurrent: {0}\nrequested: {1}",
                    ongoingRecording, new RecordingParameters { loop = loop, lengthSec = lengthSec, frequency = frequency });
                //todo: possibly somehow convert audioclip data to suit desired parameters if that becomes a problem,
                //      for now just return the clip with first specified parameters
            }

            ongoingRecording.users++;
            Debug.LogFormat("Microphone {0} now has {1} users", deviceName, ongoingRecording.users);

            return ongoingRecording.audioClip;
        }

        return StartNewRecording();


        AudioClip StartNewRecording()
        {
            var audioClip = Microphone.Start(deviceName, loop, lengthSec, frequency);
            var parameters = new RecordingParameters
            {
                loop = loop,
                lengthSec = lengthSec,
                frequency = frequency,
                audioClip = audioClip,
                users = 1
            };
            ongoingRecordings.Add(deviceName, parameters);
            Debug.LogFormat("Started the '{0}' input device: {1}", deviceName, parameters);
            return audioClip;
        }
    }

    public static bool IsRecording(string deviceName)
    {
        return Microphone.IsRecording(deviceName);
    }

    /// How many places are using this input source right now
    public static int GetUsersRecording(string deviceName)
    {
        if (!IsRecording(deviceName))
        {
            return 0;
        }

        if (!ongoingRecordings.ContainsKey(deviceName))
        {
            Debug.LogErrorFormat("Someone's recording '{0}' without MicUtils knowing!", deviceName);
            return -1;
        }

        return ongoingRecordings[deviceName].users;
    }

    public static void End(string deviceName)
    {
        if (!IsRecording(deviceName))
        {
            return;
        }
        if (!ongoingRecordings.ContainsKey(deviceName))
        {
            JustStop();
            return;
        }

        var ongoingRecording = ongoingRecordings[deviceName];
        if (ongoingRecording.users <= 1)
        {
            ongoingRecordings.Remove(deviceName);
            JustStop();
        }
        else
        {
            ongoingRecording.users--;
            Debug.LogFormat("Microphone {0} now has {1} users", deviceName, ongoingRecording.users);
        }


        void JustStop()
        {
            Debug.LogFormat("Stopping the '{0}' input device", deviceName);
            Microphone.End(deviceName);
        }
    }

}

public class RecordingParameters
{
    public bool loop;
    public int lengthSec;
    public int frequency;
    public int users;
    public AudioClip audioClip;

    public override string ToString()
    {
        return $"{nameof(loop)}: {loop}, {nameof(lengthSec)}: {lengthSec}, {nameof(frequency)}: {frequency}, {nameof(users)}: {users}";
    }
}