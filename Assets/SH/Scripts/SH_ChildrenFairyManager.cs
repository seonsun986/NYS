using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SH_ChildrenFairyManager : MonoBehaviour
{
    public static SH_ChildrenFairyManager Instance;
    // 동화책 페이지를 담아 둘 리스트
    public List<GameObject> pages = new List<GameObject>();
    // 선택했을 때 나타날 오브젝트들의 리스트(토끼, 호랑이, 곰 등등)
    public List<GameObject> selectObject;
    // 여동생인지 남동생인지 선택하면 채워질 텍스트  
    public List<Text> broText;
    // 여동생, 남동생 게임 오브젝트 리스트
    public List<GameObject> sister;
    public List<GameObject> brother;
    // 엄마 아빠인지 선택하면 채워질 텍스트
    public List<Text> parentText;
    // 오디오 클립 리스트
    public List<AudioClip> audioClips;
    // 뒷 배경 리스트
    public List<GameObject> bgImage;
    [Header("선택지가 있는 페이지")]
    // 선택 페이지 리스트(이때는 넥스트 버튼이 선택하면 넘어가면 안됨)
    public List<int> SelectPage = new List<int>();
    public GameObject PassPopUp;
    public GameObject FailPopUp;
    public GameObject selectPopUp;
    public int currentPage;

    public GameObject nextBtn;
    public GameObject preBtn;

    bool pass;
    public GameObject bookWorld;            // 책 골랐을 때 나타나는 월드 오브젝트

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    bool mushroomB;
    bool eggB;
    bool spinachB;
    bool riceB;
    bool onionB;
    bool potatoB;
    public GameObject girlNo;
    public GameObject girlYes;
    public GameObject boyYes;
    public GameObject boyNo;


    void Update()
    {
        if (bookWorldOpen == true)
        {
            currentTime += Time.deltaTime;
            if (currentTime > bookOpenTime)
            {
                // 남동생 선택했을 때
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    bookWorld.GetComponent<AudioSource>().clip = audioClips[12];
                }
                // 여동생 선택했을 때
                else
                {
                    bookWorld.GetComponent<AudioSource>().clip = audioClips[13];
                }

                bookWorld.SetActive(true);
                // 크기 커지면서 보이도록
                iTween.ScaleTo(bookWorld, iTween.Hash("x", 1, "y", 1, "z", 1, "easeType", "easeOutQuad", "time", 0.5f));

                currentTime = 0;
                bookWorldOpen = false;
            }
        }

        // 주방
        if (currentPage == 1 || currentPage == 4 || currentPage == 5 || currentPage == 8 || currentPage == 9 || currentPage == 10 || currentPage == 11 || currentPage == 22)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // 주방만 키기
            bgImage[1].SetActive(true);
        }

        else if (currentPage == 2 || currentPage == 3)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // 들판만 키기
            bgImage[2].SetActive(true);
        }

        else if (currentPage == 6)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // 축구장만 키기
            bgImage[3].SetActive(true);
        }

        else if (currentPage == 7 || currentPage == 18)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // 전구배경만 키기
            bgImage[4].SetActive(true);
        }
        else if (currentPage >= 19 && currentPage <= 21)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // 초록왕국 배경만 키기

            bgImage[10].SetActive(true);
        }

        // 먹어볼까요? 때 여동생? 남동생?
        if (currentPage == 14)
        {
            // 남동생 선택했을 때
            if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
            {
                if (boyNo.activeSelf == false)
                {
                    boyNo.SetActive(true);
                    boyYes.SetActive(true);
                }

            }
            // 여동생 선택했을 때
            else
            {
                if (girlNo.activeSelf == false)
                {
                    girlNo.SetActive(true);
                    girlYes.SetActive(true);
                }
            }
        }
        else
        {
            if (girlNo.activeSelf == true || boyNo.activeSelf == true)
            {
                boyNo.SetActive(false);
                boyYes.SetActive(false);
                girlNo.SetActive(false);
                girlYes.SetActive(false);

            }
        }

        // 싫어하는 음식 선택하고 나서
        // 버섯
        if (mushroomB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // 우주배경 켜주기
            bgImage[5].SetActive(true);
        }
        // 달걀
        else if (eggB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // 우주배경 켜주기
            bgImage[5].SetActive(true);
        }
        //시금치
        else if (spinachB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // 우주배경 켜주기
            bgImage[6].SetActive(true);
        }
        //밥
        else if (riceB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // 눈배경 켜주기
            bgImage[8].SetActive(true);
        }

        // 양파
        else if (onionB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // 바다배경 켜주기
            bgImage[7].SetActive(true);
        }
        // 감자
        else if (potatoB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // 사막배경 켜주기
            bgImage[9].SetActive(true);
        }

        // 1페이지가 켜졌을 때 && 엄마나 아빠 둘 중에 하나를 선택했을 때
        startCurrentTime += Time.deltaTime;
        if(startCurrentTime > startTextChangeTime)
        {
            startText.text = "내 동생의 성별을 정해줄래요?";
        }
        if(pages[0].activeSelf == true)
        {
            page0CurrentTime += Time.deltaTime;
            if(page0CurrentTime>page0TextChangeTime && page0Count<1)
            {
                for(int i =0;i<page0_pre.Count;i++)
                {
                    page0_pre[i].SetActive(false);
                }

                for(int j=0;j<page0_change.Count;j++)
                {
                    page0_change[j].SetActive(true);
                }

                page0CurrentTime = 0;
                page0Count++;
            }
        }

        if (pages[8].activeSelf == true)
        {
            page8CurrentTime += Time.deltaTime;
            if (page8CurrentTime > page8TextChangeTime && page8Count < 1)
            {
                for (int i = 0; i < page8_pre.Count; i++)
                {
                    page8_pre[i].SetActive(false);
                }

                for (int j = 0; j < page8_change.Count; j++)
                {
                    page8_change[j].SetActive(true);
                }

                page8CurrentTime = 0;
                page8Count++;
            }
        }



    }
    public GameObject momBtn;
    public GameObject dadBtn;
    public float startTextChangeTime;
    public float startCurrentTime;
    public float page0TextChangeTime;
    public float page0CurrentTime;
    public List<GameObject> page0_pre = new List<GameObject>();
    public List<GameObject> page0_change = new List<GameObject>();
    public Text startText;
    public List<GameObject> page1_pre = new List<GameObject>();
    public List<GameObject> page1_change = new List<GameObject>();
    int page1Count;
    int page0Count;
    public List<GameObject> page8_pre = new List<GameObject>();
    public List<GameObject> page8_change = new List<GameObject>();
    int page8Count;
    public float page8TextChangeTime;
    public float page8CurrentTime;

    // 여동생을 선택했을 때
    public void SelectSis()
    {
        pages[0].GetComponent<AudioSource>().clip = audioClips[0];
        pages[2].GetComponent<AudioSource>().clip = audioClips[7];
        pages[4].GetComponent<AudioSource>().clip = audioClips[9];
        pages[6].GetComponent<AudioSource>().clip = audioClips[11];
        pages[0].GetComponent<AudioSource>().enabled = true;
        // 남동생 끄기
        for (int i = 0; i < brother.Count; i++)
        {
            brother[i].SetActive(false);
        }
        // 여동생 키기
        for (int j = 0; j < sister.Count; j++)
        {
            sister[j].SetActive(true);
        }


    }
    

    // 남동생을 선택했을 때
    public void SelectBro()
    {
        pages[0].GetComponent<AudioSource>().clip = audioClips[1];
        pages[2].GetComponent<AudioSource>().clip = audioClips[6];
        pages[4].GetComponent<AudioSource>().clip = audioClips[8];
        pages[6].GetComponent<AudioSource>().clip = audioClips[10];
        pages[0].GetComponent<AudioSource>().enabled = true;
        // 남동생 키키
        for (int i = 0; i < brother.Count; i++)
        {
            brother[i].SetActive(true);
        }
        // 여동생 끄기
        for (int j = 0; j < sister.Count; j++)
        {
            sister[j].SetActive(false);
        }
    }

    // 엄마를 선택했을 때
    public void SelectMom()
    {
        // 남동생일 때
        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[4];
        }
        // 여동생일 때
        else
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[5];
        }

        pages[1].GetComponent<AudioSource>().enabled = true;

        // 버튼 끄기
        momBtn.SetActive(false);
        dadBtn.SetActive(false);
    }

    // 아빠를 선택했을 때
    public void SelectDad()
    {
        // 남동생일 때
        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[2];
        }
        // 여동생일 때
        else
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[3];
        }

        pages[1].GetComponent<AudioSource>().enabled = true;

        // 버튼 끄기
        momBtn.SetActive(false);
        dadBtn.SetActive(false);
    }


    public GameObject foodBubble;

    public void NextPage()
    {
        // 현재 페이지가 마지막 페이지일때는 리턴시킨다
        if (currentPage == pages.Count - 1) return;

        if(currentPage == 1 && page1Count <1)
        {
            foodBubble.SetActive(true);
            iTween.ScaleTo(foodBubble, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.3f, "easeType", "easeOutQuad"));
            for (int i = 0; i < page1_pre.Count; i++)
            {
                page1_pre[i].SetActive(false);
            }
            for (int j = 0; j < page1_change.Count; j++)
            {
                page1_change[j].SetActive(true);
            }
            // 남동생일 때
            if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
            {
                pages[1].GetComponent<AudioSource>().clip = audioClips[74];
            }
            // 여동생일 때
            else
            {
                pages[1].GetComponent<AudioSource>().clip = audioClips[75];
            }

            pages[1].GetComponent<AudioSource>().Play();
            page1Count++;
            return;
        }
        // 퀴즈 페이지나 선택 페이지면 반드시 선택을 해야지 넘어가게 한다
        for (int i = 0; i < SelectPage.Count; i++)
        {
            // 선택 페이지일 때 
            if (currentPage == SelectPage[i])
            {
                if (pass != true)
                {
                    // 아직 선택을 하지 않았다면
                    selectPopUp.SetActive(true);
                    return;
                }
                else
                {
                    if (PassPopUp.activeSelf == true)
                    {
                        PassPopUp.SetActive(false);
                    }
                }

            }
        }

        // 만약 패스 버튼이었다면 팝업을 꺼준다
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            GameObject PassPopUpj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
            if (PassPopUpj.name.Contains("Pass"))
            {
                PassPopUpj.SetActive(false);
                nextBtn.SetActive(true);
                preBtn.SetActive(true);
            }

        }

        pages[currentPage].SetActive(false);

        // 먹어볼까요? 페이지(이때는 3페이지를 뛰어넘어야 한다)
        if (currentPage == 15)
        {
            pages[currentPage + 3].SetActive(true);
            currentPage += 3;
        }
        else
        {
            pages[currentPage + 1].SetActive(true);
            currentPage += 1;
        }

        // 나온 오브젝트를 꺼준다
        if (selectObj != null) selectObj.SetActive(false);


        pass = false;
    }

    public void PrePage()
    {
        // 현재 페이지가 처음 페이지일때는 리턴시킨다
        if (currentPage == 0) return;
        pages[currentPage].SetActive(false);
        pages[currentPage - 1].SetActive(true);
        currentPage -= 1;
    }
    RaycastHit passHit;
    public void OnPassPopUp()
    {

        PassPopUp.SetActive(true);
        // 패스팝업 떴을때는 꺼주기
        nextBtn.SetActive(false);
        preBtn.SetActive(false);
        pass = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out passHit))
        {
            GameObject selectBtn = passHit.transform.gameObject;
            string selectBtnName = selectBtn.name.Substring(0, selectBtn.name.Length - 3);

            selectBtn.transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < selectObject.Count; i++)
            {
                if (selectBtnName == selectObject[i].name)
                {
                    selectObj = selectObject[i];
                    if (selectBtnName == "Rabbit")
                    {
                        selectObj.transform.GetChild(1).localPosition = new Vector3(0, 0.024f, 0.24f);
                        // 남동생 선택했을 때
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[72];
                        }
                        // 여동생 선택했을 때
                        else
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[73];
                        }
                    }
                    else if (selectBtnName == "Ball")
                    {
                        // 남동생 선택했을 때
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[66];
                        }
                        // 여동생 선택했을 때
                        else
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[67];
                        }
                    }
                    selectObj.SetActive(true);
                }
            }

        }



    }

    GameObject selectObj;
    RaycastHit failHit;
    // 책을 골랐을 경우 책이 열리고 나서 고래가 떴으면 좋겠다!
    float currentTime;
    public float bookOpenTime;
    bool bookWorldOpen;
    public void OnFailPopUp()
    {
        FailPopUp.SetActive(true);

        // 패스팝업 떴을때는 꺼주기
        nextBtn.SetActive(false);
        preBtn.SetActive(false);

        // 해당 FailBtn과 관련된 애니메이션 재생하자!
        // 그러러면 일단 내가 선택한 버튼이 뭔지 알아야한다 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out failHit))
        {
            GameObject selectBtn = failHit.transform.gameObject;
            string selectBtnName = selectBtn.name.Substring(0, selectBtn.name.Length - 3);

            // 버튼을 모두 끈다
            selectBtn.transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < selectObject.Count; i++)
            {
                if (selectBtnName == selectObject[i].name)
                {
                    selectObj = selectObject[i];
                    if (selectBtnName == "Bear")
                    {
                        selectObj.transform.GetChild(1).localPosition = new Vector3(0, 2.22f, 2);
                        // 남동생 선택했을 때
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[68];
                        }
                        // 여동생 선택했을 때
                        else
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[69];
                        }

                    }
                    else if (selectBtnName == "Tiger")
                    {
                        selectObj.transform.GetChild(1).localPosition = new Vector3(0, 0.68f, 1.81f);
                        // 남동생 선택했을 때
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[70];
                        }
                        // 여동생 선택했을 때
                        else
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[71];
                        }
                    }
                    else if (selectBtnName == "Book")
                    {
                        bookWorldOpen = true;
                    }
                    else if (selectBtnName == "Box")
                    {
                        // 남동생 선택했을 때
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[64];
                        }
                        // 여동생 선택했을 때
                        else
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[65];
                        }
                    }
                    if (selectObj.transform.childCount > 1)
                    {
                        if (selectObj.transform.GetChild(1).GetComponent<Rigidbody>() != null)
                        {
                            selectObj.transform.GetChild(1).GetComponent<Rigidbody>().velocity = Vector3.zero;
                        }
                    }

                    selectObj.SetActive(true);
                }
            }
        }


    }

    public void PassTrue()
    {
        pass = true;
    }

    public void PopUpClose()
    {
        GameObject closeBtn = EventSystem.current.currentSelectedGameObject;
        closeBtn.transform.parent.gameObject.SetActive(false);
    }

    public void TryAgain()
    {
        GameObject closeBtn = EventSystem.current.currentSelectedGameObject;
        closeBtn.transform.parent.gameObject.SetActive(false);

        nextBtn.SetActive(true);
        preBtn.SetActive(true);


        // 다시 현재 페이지를 켜준다
        pages[currentPage].SetActive(true);
        // 나온 오브젝트를 꺼준다
        selectObj.SetActive(false);
        if (selectObj.transform.childCount > 1)
        {
            if (selectObj.transform.GetChild(1).GetComponent<Rigidbody>() != null)
            {
                selectObj.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = false;       // 당근 중력 꺼주기..
            }
        }

        if (bookWorld.activeSelf == true)
        {
            bookWorld.SetActive(false);
        }
    }

    public GameObject selectPages;
    RaycastHit fillHit;
    public void FillEmptyBox()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out fillHit))
        {
            string GoName = fillHit.transform.name;
            string selectBtnName = GoName.Substring(0, GoName.Length - 3);
            selectPages = GameObject.Find(selectBtnName + "Pages");

            // 선택한 페이지에 따라서 Page들을 Pages에 추가해준다
            for (int i = 0; i < selectPages.transform.childCount; i++)
            {
                pages.Insert(i + 10, selectPages.transform.GetChild(i).gameObject);
            }



            // 선택한게 버섯일 때
            if (selectBtnName == "Mushroom")
            {

                // 남동생일 때
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[14];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[16];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[18];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[20];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[24];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[26];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[28];
                }
                // 여동생일 때
                else
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[15];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[17];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[19];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[21];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[23];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[25];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[27];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[29];
                }

                mushroomB = true;

            }

            // 선택한게 계란일 때
            else if (selectBtnName == "Egg")
            {


                // 남동생일 때
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[30];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[32];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[34];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[36];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];        // Page17 재활용
                    pages[18].GetComponent<AudioSource>().clip = audioClips[24];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[26];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[28];


                }
                // 여동생일 때 
                else
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[31];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[33];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[35];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[37];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[23];        // Page17 재활용
                    pages[18].GetComponent<AudioSource>().clip = audioClips[25];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[27];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[29];

                }

                eggB = true;

            }

            // 선택한게 시금치일 때
            else if (selectBtnName == "Spinach")
            {


                // 남동생일 때
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[38];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[40];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[42];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[44];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[24];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[26];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[28];

                }
                // 여동생일 때
                else
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[39];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[41];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[43];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[45];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[25];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[27];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[29];

                }

                spinachB = true;

            }

            // 선택한게 양파일 때
            else if (selectBtnName == "Onion")
            {


                // 남동생일 때
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[46];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[48];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[50];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[44];

                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[24];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[26];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[28];
                }
                // 여동생일 때
                else
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[47];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[49];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[51];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[45];

                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[25];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[27];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[29];
                }
                onionB = true;

            }

            // 선택한게 밥일 때

            else if (selectBtnName == "Rice")
            {


                // 남동생일 때
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[52];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[54];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[56];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[44];

                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[24];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[26];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[28];
                }
                // 여동생일 때
                else
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[53];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[55];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[57];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[45];

                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[25];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[27];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[29];
                }
                riceB = true;
            }

            // 선택한게 감자일 때

            else
            {


                // 남동생일 때
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[58];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[60];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[62];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[44];

                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[24];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[26];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[28];
                }
                // 여동생일 때
                else
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[59];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[61];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[63];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[45];

                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];
                    pages[18].GetComponent<AudioSource>().clip = audioClips[25];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[27];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[29];
                }
                potatoB = true;
            }

            // NextPage 함수 그냥 여기서 실행함
            pages[currentPage].SetActive(false);
            pages[currentPage + 1].SetActive(true);
            currentPage += 1;

        }

    }

    public void TryNo()
    {
        pages[currentPage].SetActive(false);
        pages[currentPage + 2].SetActive(true);
        currentPage += 2;
    }

    public void Girl()
    {
        for (int i = 0; i < broText.Count; i++)
        {
            broText[i].text = "여동생";
            broText[0].color = new Color(1, 0.646303f, 0.3433962f);
        }
    }

    public void Boy()
    {
        for (int i = 0; i < broText.Count; i++)
        {
            broText[i].text = "남동생";
            broText[0].color = new Color(0.6626405f, 0.183f, 1);

        }
    }

    bool parentSelect;
    public void Mom()
    {
        for (int i = 0; i < parentText.Count; i++)
        {
            parentText[i].text = "엄마";
            parentText[i].color = new Color(1, 0.9625695f, 0.3607843f, 1);
        }
        parentSelect = true;
    }

    public void Dad()
    {
        for (int i = 0; i < parentText.Count; i++)
        {
            parentText[i].text = "아빠";
            parentText[i].color = new Color(0.4745098f, 0.7215f, 0.8196079f, 1);

        }
        parentSelect = true;
    }

    bool play;
    // 재생 할때
    public Sprite Playing;
    // 멈출 때
    public Sprite Pause;
    public Image playPauseBtn;

    public void PauseOrPlay()
    {
        if (play == false)
        {
            Time.timeScale = 0;
            play = true;
            playPauseBtn.sprite = Pause;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            play = false;
            playPauseBtn.sprite = Playing;
            AudioListener.pause = false;
        }
    }


    public GameObject questionBox;
    public void QuestionBox()
    {
        questionBox.SetActive(false);
    }

    public void BackBtn()
    {
        
    }

    public void MomBtnOn()
    {
        iTween.ScaleTo(momBtn, iTween.Hash("x", 1.2f, "y", 1.2f, "time", 0.3f));
        momBtn.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void MomBtnOff()
    {
        iTween.ScaleTo(momBtn, iTween.Hash("x", 1.0f, "y", 1.0f, "time", 0.3f));
        momBtn.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void DadBtnOn()
    {
        iTween.ScaleTo(dadBtn, iTween.Hash("x", 1.2f, "y", 1.2f, "time", 0.3f));
        dadBtn.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void DadBtnOff()
    {
        iTween.ScaleTo(dadBtn, iTween.Hash("x", 1.0f, "y", 1.0f, "time", 0.3f));
        dadBtn.transform.GetChild(0).gameObject.SetActive(false);
    }
}
