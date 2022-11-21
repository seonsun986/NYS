using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 이 아이는 오디오매니저!
public class YJ_AudioManager : MonoBehaviour
{
    // 메인카메라를 가져올것
    Camera main;

    // 오디오
    AudioSource audioSet;

    // PreviewScene에서는 BGM없애기

    // 브금목록
    [SerializeField]
    [Header("BGM")]
    public AudioClip plazaBGM;
    public AudioClip myRoomBGM;
    public AudioClip createCharacterBGM;
    public AudioClip createRoomBGM;
    public AudioClip EditorBGM;
    public AudioClip fairyBGM;
    public AudioClip t_bookShelfBGM;
    public AudioClip c_bookShelfBGM; //아직없음


    int winner = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        main = Camera.main;
        audioSet = main.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 광장
        if (SceneManager.GetActiveScene().name == "PlazaScene" && main == null)
        {
            main = Camera.main;
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = plazaBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "TeacherScene(Candy)" && main == null || SceneManager.GetActiveScene().name == "TeacherScene(ClassRoom)" && main == null || SceneManager.GetActiveScene().name == "TeacherScene(Christmas)" && main == null)
        {
            main = Camera.main;
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = createRoomBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "MyRoomScene" && main == null)
        {
            main = Camera.main;
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = myRoomBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "CreateCharacter" && main == null)
        {
            main = Camera.main;
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = createCharacterBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "Fairy_IHate" && main == null)
        {
            main = Camera.main;
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = fairyBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "BookShelfScene" && main == null || SceneManager.GetActiveScene().name == "EditorChildren" && main == null)
        {
            main = Camera.main;
            audioSet = main.GetComponent<AudioSource>();
            audioSet.clip = t_bookShelfBGM;
            audioSet.Play();
        }

        else if (SceneManager.GetActiveScene().name == "EditorScene" && main == null)
        {
            return;
        }
    }
}
