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
    // ��ȭå �������� ��� �� ����Ʈ
    public List<GameObject> pages = new List<GameObject>();
    // �������� �� ��Ÿ�� ������Ʈ���� ����Ʈ(�䳢, ȣ����, �� ���)
    public List<GameObject> selectObject;
    // ���������� ���������� �����ϸ� ä���� �ؽ�Ʈ  
    public List<Text> broText;
    // ������, ������ ���� ������Ʈ ����Ʈ
    public List<GameObject> sister;
    public List<GameObject> brother;
    // ���� �ƺ����� �����ϸ� ä���� �ؽ�Ʈ
    public List<Text> parentText;
    // ����� Ŭ�� ����Ʈ
    public List<AudioClip> audioClips;
    // �� ��� ����Ʈ
    public List<GameObject> bgImage;
    [Header("�������� �ִ� ������")]
    // ���� ������ ����Ʈ(�̶��� �ؽ�Ʈ ��ư�� �����ϸ� �Ѿ�� �ȵ�)
    public List<int> SelectPage = new List<int>();
    public GameObject PassPopUp;
    public GameObject FailPopUp;
    public GameObject selectPopUp;
    public int currentPage;

    public GameObject nextBtn;
    public GameObject preBtn;

    bool pass;
    public GameObject bookWorld;            // å ����� �� ��Ÿ���� ���� ������Ʈ

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
                // ������ �������� ��
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    bookWorld.GetComponent<AudioSource>().clip = audioClips[12];
                }
                // ������ �������� ��
                else
                {
                    bookWorld.GetComponent<AudioSource>().clip = audioClips[13];
                }

                bookWorld.SetActive(true);
                // ũ�� Ŀ���鼭 ���̵���
                iTween.ScaleTo(bookWorld, iTween.Hash("x", 1, "y", 1, "z", 1, "easeType", "easeOutQuad", "time", 0.5f));

                currentTime = 0;
                bookWorldOpen = false;
            }
        }

        // �ֹ�
        if (currentPage == 1 || currentPage == 4 || currentPage == 5 || currentPage == 8 || currentPage == 9 || currentPage == 10 || currentPage == 11 || currentPage == 22)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // �ֹ游 Ű��
            bgImage[1].SetActive(true);
        }

        else if (currentPage == 2 || currentPage == 3)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // ���Ǹ� Ű��
            bgImage[2].SetActive(true);
        }

        else if (currentPage == 6)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // �౸�常 Ű��
            bgImage[3].SetActive(true);
        }

        else if (currentPage == 7 || currentPage == 18)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // ������游 Ű��
            bgImage[4].SetActive(true);
        }
        else if (currentPage >= 19 && currentPage <= 21)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // �ʷϿձ� ��游 Ű��

            bgImage[10].SetActive(true);
        }

        // �Ծ���? �� ������? ������?
        if (currentPage == 14)
        {
            // ������ �������� ��
            if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
            {
                if (boyNo.activeSelf == false)
                {
                    boyNo.SetActive(true);
                    boyYes.SetActive(true);
                }

            }
            // ������ �������� ��
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

        // �Ⱦ��ϴ� ���� �����ϰ� ����
        // ����
        if (mushroomB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // ���ֹ�� ���ֱ�
            bgImage[5].SetActive(true);
        }
        // �ް�
        else if (eggB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // ���ֹ�� ���ֱ�
            bgImage[5].SetActive(true);
        }
        //�ñ�ġ
        else if (spinachB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // ���ֹ�� ���ֱ�
            bgImage[6].SetActive(true);
        }
        //��
        else if (riceB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // ����� ���ֱ�
            bgImage[8].SetActive(true);
        }

        // ����
        else if (onionB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // �ٴٹ�� ���ֱ�
            bgImage[7].SetActive(true);
        }
        // ����
        else if (potatoB == true && currentPage >= 12 && currentPage <= 17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // �縷��� ���ֱ�
            bgImage[9].SetActive(true);
        }

        // 1�������� ������ �� && ������ �ƺ� �� �߿� �ϳ��� �������� ��
        startCurrentTime += Time.deltaTime;
        if(startCurrentTime > startTextChangeTime)
        {
            startText.text = "�� ������ ������ �����ٷ���?";
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

    // �������� �������� ��
    public void SelectSis()
    {
        pages[0].GetComponent<AudioSource>().clip = audioClips[0];
        pages[2].GetComponent<AudioSource>().clip = audioClips[7];
        pages[4].GetComponent<AudioSource>().clip = audioClips[9];
        pages[6].GetComponent<AudioSource>().clip = audioClips[11];
        pages[0].GetComponent<AudioSource>().enabled = true;
        // ������ ����
        for (int i = 0; i < brother.Count; i++)
        {
            brother[i].SetActive(false);
        }
        // ������ Ű��
        for (int j = 0; j < sister.Count; j++)
        {
            sister[j].SetActive(true);
        }


    }
    

    // �������� �������� ��
    public void SelectBro()
    {
        pages[0].GetComponent<AudioSource>().clip = audioClips[1];
        pages[2].GetComponent<AudioSource>().clip = audioClips[6];
        pages[4].GetComponent<AudioSource>().clip = audioClips[8];
        pages[6].GetComponent<AudioSource>().clip = audioClips[10];
        pages[0].GetComponent<AudioSource>().enabled = true;
        // ������ ŰŰ
        for (int i = 0; i < brother.Count; i++)
        {
            brother[i].SetActive(true);
        }
        // ������ ����
        for (int j = 0; j < sister.Count; j++)
        {
            sister[j].SetActive(false);
        }
    }

    // ������ �������� ��
    public void SelectMom()
    {
        // �������� ��
        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[4];
        }
        // �������� ��
        else
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[5];
        }

        pages[1].GetComponent<AudioSource>().enabled = true;

        // ��ư ����
        momBtn.SetActive(false);
        dadBtn.SetActive(false);
    }

    // �ƺ��� �������� ��
    public void SelectDad()
    {
        // �������� ��
        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[2];
        }
        // �������� ��
        else
        {
            pages[1].GetComponent<AudioSource>().clip = audioClips[3];
        }

        pages[1].GetComponent<AudioSource>().enabled = true;

        // ��ư ����
        momBtn.SetActive(false);
        dadBtn.SetActive(false);
    }


    public GameObject foodBubble;

    public void NextPage()
    {
        // ���� �������� ������ �������϶��� ���Ͻ�Ų��
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
            // �������� ��
            if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
            {
                pages[1].GetComponent<AudioSource>().clip = audioClips[74];
            }
            // �������� ��
            else
            {
                pages[1].GetComponent<AudioSource>().clip = audioClips[75];
            }

            pages[1].GetComponent<AudioSource>().Play();
            page1Count++;
            return;
        }
        // ���� �������� ���� �������� �ݵ�� ������ �ؾ��� �Ѿ�� �Ѵ�
        for (int i = 0; i < SelectPage.Count; i++)
        {
            // ���� �������� �� 
            if (currentPage == SelectPage[i])
            {
                if (pass != true)
                {
                    // ���� ������ ���� �ʾҴٸ�
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

        // ���� �н� ��ư�̾��ٸ� �˾��� ���ش�
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

        // �Ծ���? ������(�̶��� 3�������� �پ�Ѿ�� �Ѵ�)
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

        // ���� ������Ʈ�� ���ش�
        if (selectObj != null) selectObj.SetActive(false);


        pass = false;
    }

    public void PrePage()
    {
        // ���� �������� ó�� �������϶��� ���Ͻ�Ų��
        if (currentPage == 0) return;
        pages[currentPage].SetActive(false);
        pages[currentPage - 1].SetActive(true);
        currentPage -= 1;
    }
    RaycastHit passHit;
    public void OnPassPopUp()
    {

        PassPopUp.SetActive(true);
        // �н��˾� �������� ���ֱ�
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
                        // ������ �������� ��
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[72];
                        }
                        // ������ �������� ��
                        else
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[73];
                        }
                    }
                    else if (selectBtnName == "Ball")
                    {
                        // ������ �������� ��
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[66];
                        }
                        // ������ �������� ��
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
    // å�� ����� ��� å�� ������ ���� ���� ������ ���ڴ�!
    float currentTime;
    public float bookOpenTime;
    bool bookWorldOpen;
    public void OnFailPopUp()
    {
        FailPopUp.SetActive(true);

        // �н��˾� �������� ���ֱ�
        nextBtn.SetActive(false);
        preBtn.SetActive(false);

        // �ش� FailBtn�� ���õ� �ִϸ��̼� �������!
        // �׷����� �ϴ� ���� ������ ��ư�� ���� �˾ƾ��Ѵ� 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out failHit))
        {
            GameObject selectBtn = failHit.transform.gameObject;
            string selectBtnName = selectBtn.name.Substring(0, selectBtn.name.Length - 3);

            // ��ư�� ��� ����
            selectBtn.transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < selectObject.Count; i++)
            {
                if (selectBtnName == selectObject[i].name)
                {
                    selectObj = selectObject[i];
                    if (selectBtnName == "Bear")
                    {
                        selectObj.transform.GetChild(1).localPosition = new Vector3(0, 2.22f, 2);
                        // ������ �������� ��
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[68];
                        }
                        // ������ �������� ��
                        else
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[69];
                        }

                    }
                    else if (selectBtnName == "Tiger")
                    {
                        selectObj.transform.GetChild(1).localPosition = new Vector3(0, 0.68f, 1.81f);
                        // ������ �������� ��
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[70];
                        }
                        // ������ �������� ��
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
                        // ������ �������� ��
                        if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                        {
                            selectObj.GetComponent<AudioSource>().clip = audioClips[64];
                        }
                        // ������ �������� ��
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


        // �ٽ� ���� �������� ���ش�
        pages[currentPage].SetActive(true);
        // ���� ������Ʈ�� ���ش�
        selectObj.SetActive(false);
        if (selectObj.transform.childCount > 1)
        {
            if (selectObj.transform.GetChild(1).GetComponent<Rigidbody>() != null)
            {
                selectObj.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = false;       // ��� �߷� ���ֱ�..
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

            // ������ �������� ���� Page���� Pages�� �߰����ش�
            for (int i = 0; i < selectPages.transform.childCount; i++)
            {
                pages.Insert(i + 10, selectPages.transform.GetChild(i).gameObject);
            }



            // �����Ѱ� ������ ��
            if (selectBtnName == "Mushroom")
            {

                // �������� ��
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
                // �������� ��
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

            // �����Ѱ� ����� ��
            else if (selectBtnName == "Egg")
            {


                // �������� ��
                if (pages[0].GetComponent<AudioSource>().clip == audioClips[1])
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[30];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[32];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[34];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[36];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[22];        // Page17 ��Ȱ��
                    pages[18].GetComponent<AudioSource>().clip = audioClips[24];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[26];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[28];


                }
                // �������� �� 
                else
                {
                    pages[10].GetComponent<AudioSource>().clip = audioClips[31];
                    pages[11].GetComponent<AudioSource>().clip = audioClips[33];
                    pages[13].GetComponent<AudioSource>().clip = audioClips[35];
                    pages[15].GetComponent<AudioSource>().clip = audioClips[37];
                    pages[17].GetComponent<AudioSource>().clip = audioClips[23];        // Page17 ��Ȱ��
                    pages[18].GetComponent<AudioSource>().clip = audioClips[25];
                    pages[20].GetComponent<AudioSource>().clip = audioClips[27];
                    pages[21].GetComponent<AudioSource>().clip = audioClips[29];

                }

                eggB = true;

            }

            // �����Ѱ� �ñ�ġ�� ��
            else if (selectBtnName == "Spinach")
            {


                // �������� ��
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
                // �������� ��
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

            // �����Ѱ� ������ ��
            else if (selectBtnName == "Onion")
            {


                // �������� ��
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
                // �������� ��
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

            // �����Ѱ� ���� ��

            else if (selectBtnName == "Rice")
            {


                // �������� ��
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
                // �������� ��
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

            // �����Ѱ� ������ ��

            else
            {


                // �������� ��
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
                // �������� ��
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

            // NextPage �Լ� �׳� ���⼭ ������
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
            broText[i].text = "������";
            broText[0].color = new Color(1, 0.646303f, 0.3433962f);
        }
    }

    public void Boy()
    {
        for (int i = 0; i < broText.Count; i++)
        {
            broText[i].text = "������";
            broText[0].color = new Color(0.6626405f, 0.183f, 1);

        }
    }

    bool parentSelect;
    public void Mom()
    {
        for (int i = 0; i < parentText.Count; i++)
        {
            parentText[i].text = "����";
            parentText[i].color = new Color(1, 0.9625695f, 0.3607843f, 1);
        }
        parentSelect = true;
    }

    public void Dad()
    {
        for (int i = 0; i < parentText.Count; i++)
        {
            parentText[i].text = "�ƺ�";
            parentText[i].color = new Color(0.4745098f, 0.7215f, 0.8196079f, 1);

        }
        parentSelect = true;
    }

    bool play;
    // ��� �Ҷ�
    public Sprite Playing;
    // ���� ��
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
