using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_VoiceRecord : MonoBehaviour
{


    AudioClip recordClip;

    public void StartRecordMicrophone()
    {
        //foreach(var device in Microphone.devices)
        //{
        //    Debug.Log("Name: " + device);
        //}
        recordClip = Microphone.Start(Microphone.devices[0], true, 100, 44100);
        print("≥Ï¿Ω¡ﬂ");
    }

    public void StopRecordMicrophone()
    {
        int lastTime = Microphone.GetPosition(null);

        if (lastTime == 0)
            return;
        else
        {
            Microphone.End(Microphone.devices[0]);

            float[] samples = new float[recordClip.samples];

            recordClip.GetData(samples, 0);

            float[] cutSamples = new float[lastTime];

            System.Array.Copy(samples, cutSamples, cutSamples.Length - 1);

            recordClip = AudioClip.Create("Notice", cutSamples.Length, 1, 44100, false);

            recordClip.SetData(cutSamples, 0);

            SH_SavWav.Save("Page" + SH_BtnManager.Instance.currentScene, recordClip);
        }
        print("≥Ï¿Ω∏ÿ√„");

    }


}
