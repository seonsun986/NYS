using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_VoiceRecord : MonoBehaviour
{


    AudioClip recordClip;
    bool record;

    public Image recordingBtn;
    public Sprite recordImg;
    public Sprite stopRecordImg;

    private void Start()
    {
        voiceClip = new List<AudioClip>();

    }
    public void Record()
    {
        // ���� ����
        if(record == false)
        {
            StartRecordMicrophone();
            record = true;
            recordingBtn.sprite = stopRecordImg;
        }

        // ���� ��
        else
        {
            StopRecordMicrophone();
            record = false;
            recordingBtn.sprite = recordImg;
        }
    }
    public void StartRecordMicrophone()
    {
        //foreach(var device in Microphone.devices)
        //{
        //    Debug.Log("Name: " + device);
        //}
        recordClip = Microphone.Start(Microphone.devices[0], true, 100, 44100);
        print("������");
    }

    public List<AudioClip> voiceClip;

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

            // ���̽� Ŭ���� ���� ����Ʈ ���� (���̽���ȯ��)
            if (voiceClip.Count > SH_BtnManager.Instance.currentScene)
            {
                voiceClip.RemoveAt(SH_BtnManager.Instance.currentScene);
                voiceClip.Insert(SH_BtnManager.Instance.currentScene, recordClip);
            }
            else
            {
                voiceClip.Insert(SH_BtnManager.Instance.currentScene, recordClip);
            }
        }
        print("��������");

    }


}
