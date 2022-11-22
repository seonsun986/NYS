using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YJ_AudioManager : MonoBehaviour
{
    public static YJ_AudioManager instance;

    Camera main;

    AudioSource audioSet;
    
    [SerializeField]
    [Header("BGM")]
    public AudioClip plazaBGM;
    public AudioClip myRoomBGM;
    public AudioClip createCharacterBGM;
    public AudioClip createRoomBGM;
    public AudioClip fairyBGM;
    public AudioClip t_bookShelfBGM;
    public AudioClip c_bookShelfBGM; //��������


    public bool bgmOnOff = true;
    public bool effectOnOff = true;

    // ���� on/off�� ���� ĵ���� ã��
    GameObject canvas;


    private void Awake()
    {
        // ���࿡ instance�� null�̶��
        if (instance == null)
        {
            // instance�� ���� �ְڴ�.
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���� �ı��ϰڴ�.
            Destroy(gameObject);
        }
    }

    void Start()
    {
        main = Camera.main;
        audioSet = main.GetComponent<AudioSource>();

        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "PlazaScene" && main == null)
        {
            main = Camera.main;
            canvas = GameObject.Find("Canvas");
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = plazaBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "TeacherScene(Candy)" && main == null || SceneManager.GetActiveScene().name == "TeacherScene(ClassRoom)" && main == null || SceneManager.GetActiveScene().name == "TeacherScene(Christmas)" && main == null)
        {
            main = Camera.main;
            canvas = GameObject.Find("Canvas");
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = createRoomBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "MyRoomScene" && main == null)
        {
            main = Camera.main;
            canvas = GameObject.Find("Canvas");
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = myRoomBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "CreateCharacter" && main == null)
        {
            main = Camera.main;
            canvas = GameObject.Find("Canvas");
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = createCharacterBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "Fairy_IHate" && main == null)
        {
            main = Camera.main;
            canvas = GameObject.Find("Canvas");
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = fairyBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "BookShelfScene" && main == null || SceneManager.GetActiveScene().name == "EditorChildren" && main == null)
        {
            main = Camera.main;
            canvas = GameObject.Find("Canvas");
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = t_bookShelfBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "EditorScene" && main == null)
        {
            AudioSource bgmsound = GameObject.Find("Sound").transform.GetChild(0).gameObject.GetComponent<AudioSource>();
            if (!bgmOnOff)
            {
                bgmsound.volume = 0;
            }
            canvas = GameObject.Find("Canvas");
        }


        if (main != null && SceneManager.GetActiveScene().name != "ConnectionScene")
        {
            if (!bgmOnOff)
            {
                audioSet.volume = 0;
            }
            else
            {
                audioSet.volume = 1;
            }
        }

        if( canvas != null )
        {
            if (!effectOnOff)
            {
                canvas.GetComponent<AudioSource>().volume = 0;
            }
            else
            {
                canvas.GetComponent<AudioSource>().volume = 1;
            }
        }
    }

    //public void OnClickOnOff()
    //{
    //    Toggle toggle = GetComponent<Toggle>();
    //    onOff = toggle.isOn;
    //    Debug.Log("on/off ��ü Ȯ�� : " + onOff + "��� ���� bool�� Ȯ�� : " + toggle.isOn);
    //}
}
