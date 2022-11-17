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
        // 녹음 시작
        if (record == false)
        {
            StartRecordMicrophone();
            record = true;
            recordingBtn.sprite = stopRecordImg;
        }

        // 녹음 끝
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
        print("녹음중");
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

            // 보이스 클립을 넣은 리스트 생성 (제이슨변환용)
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
        print("녹음멈춤");

    }

    public GameObject popUp1Panel;
    public GameObject PopUp1;
    public GameObject popUp2;
    public GameObject popUp3;

    public Toggle PopUp1Check;
    public Toggle popUp2Check;
    public Toggle popUp3Check;
    public Button ttsBtn;
    public Button recordBtn;

    public Sprite ttsChecked;
    public Sprite ttsUnCheked;

    int recordNum;
    public void RecordPopUp()
    {
        if(recordNum < 1)
        {
            // 녹음 중이 아닐 때 -> 녹음 시작
            if (recordingBtn.sprite == recordImg)
            {
                if (PopUp1Check.isOn == false)
                {
                    popUp1Panel.SetActive(true);
                    PopUp1.SetActive(true);
                    iTween.ScaleTo(PopUp1, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.5f));
                }

                else
                {
                    Record();
                    ttsBtn.interactable = false;
                }

            }

            else
            {
                Record();
                ttsBtn.interactable = false;
                recordNum++;
            }
        }

        // 이미 현재 페이지에 녹음 파일이 있을 경우
        else
        {
            // 녹음 중이 아닐 때 -> 녹음 시작
            if (recordingBtn.sprite == recordImg)
            {
                if (popUp3Check.isOn == false)
                {
                    popUp1Panel.SetActive(true);
                    popUp3.SetActive(true);
                    iTween.ScaleTo(popUp3, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.5f));
                }

                else
                {
                    Record();
                    ttsBtn.interactable = false;
                }

            }

            else
            {
                Record();
                ttsBtn.interactable = false;
                recordNum++;
            }
        }     
    }

    public void PopUp1Ok()
    {
        iTween.ScaleTo(PopUp1, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
        popUp1Panel.SetActive(false);
        ttsBtn.interactable = false;
        StartCoroutine(IERecord());
    }

    public void PopUp3Ok()
    {
        iTween.ScaleTo(popUp3, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
        popUp1Panel.SetActive(false);
        ttsBtn.interactable = false;
        StartCoroutine(IERecord());
    }


    // 팝업 없어질때까지 기다렸다가 시작하기 위해서
    public IEnumerator IERecord()
    {
        yield return new WaitForSeconds(0.7f);
        Record();
    }


    public void PopUp1No()
    {
        popUp1Panel.SetActive(false);
        iTween.ScaleTo(PopUp1, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
    }

    public void PopUp3No()
    {
        popUp1Panel.SetActive(false);
        iTween.ScaleTo(popUp3, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
    }

    public void PopUp2()
    {
        if (popUp2Check.isOn == false)
        {
            popUp1Panel.SetActive(true);
            popUp2.SetActive(true);
            iTween.ScaleTo(popUp2, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.5f));
        }

        else
        {
            ttsBtn.GetComponent<Image>().sprite = ttsChecked;
            recordBtn.interactable = false;
        }
    }

    public void PopUp2Ok()
    {
        popUp1Panel.SetActive(false);
        iTween.ScaleTo(popUp2, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
        ttsBtn.GetComponent<Image>().sprite = ttsChecked;
        recordBtn.interactable = false;
    }

    public void PopUp2No()
    {
        popUp1Panel.SetActive(false);
        iTween.ScaleTo(popUp2, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f));
    }

}
