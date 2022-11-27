using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_VoiceRecord : MonoBehaviour
{

    public static SH_VoiceRecord Instance;

    AudioClip recordClip;
    bool record;

    public Image recordingBtn;
    public Sprite recordImg;
    public Sprite stopRecordImg;
    public List<VoiceInfo> voiceInfos = new List<VoiceInfo>();
    public int num;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //voiceClip = new List<AudioClip>();

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

    public GameObject recording;

    public void StartRecordMicrophone()
    {
        //foreach(var device in Microphone.devices)
        //{
        //    Debug.Log("Name: " + device);
        //}
        recordClip = Microphone.Start(Microphone.devices[0], true, 100, 44100);
        print("녹음중");
        recording.SetActive(true);

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
            voiceClip[SH_BtnManager.Instance.currentScene] = recordClip;

            //recordClip = Resources.Load<AudioClip>("Page" + SH_BtnManager.Instance.currentScene);
            //if (voiceClip.Count == 0)
            //{
            //    voiceClip.Add(recordClip);

            //}
            //else
            //{
            //    // 보이스 클립을 넣은 리스트 생성 (제이슨변환용)
            //    if (voiceClip.Count > SH_BtnManager.Instance.currentScene)
            //    {
            //        voiceClip.RemoveAt(SH_BtnManager.Instance.currentScene);
            //        voiceClip.Insert(SH_BtnManager.Instance.currentScene, recordClip);
            //    }
            //    else
            //    {
            //        voiceClip.Insert(SH_BtnManager.Instance.currentScene, recordClip);
            //    }
            //}

        }
        print("녹음멈춤");
        StartCoroutine(StopRecord());
    }

    public GameObject recordingStop;
    IEnumerator StopRecord()
    {
        recording.SetActive(false);
        recordingStop.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        recordingStop.SetActive(false);
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

    public void RecordPopUp()
    {
        if(num < 1)
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
                num++;
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
                num++;
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


    // 페이지를 넘길때마다의 레코드 넘버를 저장해야할거같은데..
    public void Reset()
    {
        // 해당 페이지에 있는 보이스 정보 담기
        VoiceInfo voiceInfo = new VoiceInfo();
        voiceInfo.ttsBtn = ttsBtn.GetComponent<Image>().sprite;
        voiceInfo.recordNum = num;

        // 리스트에 담아주기
        voiceInfos.Add(voiceInfo);
        // 초기화 시켜주기
        ttsBtn.interactable = true;
        recordBtn.interactable = true;
        ttsBtn.GetComponent<Image>().sprite = ttsUnCheked;
        recordingBtn.sprite = recordImg;
    }

    public void Change()
    {
        if(voiceInfos.Count < SH_BtnManager.Instance.Scenes.Count)//  SH_BtnManager.Instance.currentSceneNum + 1)
        {
            // 해당 페이지에 있는 보이스 정보 담기
            VoiceInfo voiceInfo = new VoiceInfo();
            voiceInfo.ttsBtn = ttsBtn.GetComponent<Image>().sprite;
            voiceInfo.recordNum = num;
            // 리스트에 담아주기
            voiceInfos.Add(voiceInfo);
        }
    }

    public class VoiceInfo
    {
        // 이게 어떻게 체크되어있는지에 따라서 달라진다
        public Sprite ttsBtn;
        public int recordNum;
    }

}
