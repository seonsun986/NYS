using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    void Update()
    {     
        if (bookWorldOpen == true)
        {
            currentTime += Time.deltaTime;
            if(currentTime > bookOpenTime)
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
        if(currentPage == 1 || currentPage == 4 || currentPage == 5 || currentPage == 8 || currentPage == 9 || currentPage == 10 || currentPage == 11 || currentPage == 22)
        {
            for(int i =0;i<bgImage.Count;i++)
            {
                bgImage[i].SetActive(false);
            }
            // �ֹ游 Ű��
            bgImage[1].SetActive(true);
        }

        else if(currentPage == 2 || currentPage == 3)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // ���Ǹ� Ű��
            bgImage[2].SetActive(true);
        }

        else if(currentPage == 6)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // �౸�常 Ű��
            bgImage[3].SetActive(true);
        }

        else if(currentPage == 7 || currentPage == 18)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // ������游 Ű��
            bgImage[4].SetActive(true);
        }
        else if(currentPage >= 19 && currentPage <=21)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }
            // �ʷϿձ� ��游 Ű��

            bgImage[10].SetActive(true);
        }

        // �Ⱦ��ϴ� ���� �����ϰ� ����
        // ����
        if(mushroomB == true && currentPage >= 12 && currentPage <=17)
        {
            for (int i = 0; i < bgImage.Count; i++)
            {
                bgImage[i].SetActive(false);
            }

            // ���ֹ�� ���ֱ�
            bgImage[5].SetActive(true);
        }
        // �ް�
        else if(eggB== true && currentPage >= 12 && currentPage <= 17)
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


    }

    public GameObject broBtn;
    public GameObject sisBtn;
    public GameObject momBtn;
    public GameObject dadBtn;
    // �������� �������� ��
    public void SelectSis()
    {
        pages[0].GetComponent<AudioSource>().clip = audioClips[0];
        pages[2].GetComponent<AudioSource>().clip = audioClips[7];
        pages[4].GetComponent<AudioSource>().clip = audioClips[9];
        pages[6].GetComponent<AudioSource>().clip = audioClips[11];
        pages[0].GetComponent<AudioSource>().enabled = true;
        // ������ ����
        for(int i =0;i<brother.Count;i++)
        {
            brother[i].SetActive(false);
        }
        // ������ Ű��
        for(int j =0; j<sister.Count;j++)
        {
            sister[j].SetActive(true);
        }

        // ��ư ����
        broBtn.SetActive(false);
        sisBtn.SetActive(false);
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

        // ��ư ����
        broBtn.SetActive(false);
        sisBtn.SetActive(false);
    }

    // ������ �������� ��
    public void SelectMom()
    {
        // �������� ��
        if(pages[0].GetComponent<AudioSource>().clip == audioClips[1])
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




    public void NextPage()
    {
        // ���� �������� ������ �������϶��� ���Ͻ�Ų��
        if (currentPage == pages.Count -1) return;

        // ���� �������� ���� �������� �ݵ�� ������ �ؾ��� �Ѿ�� �Ѵ�
        for(int i =0; i<SelectPage.Count;i++)
        {
            // ���� �������� �� 
            if(currentPage == SelectPage[i])
            {
                if(pass != true)
                {
                    // ���� ������ ���� �ʾҴٸ�
                    selectPopUp.SetActive(true);
                    return;
                }   
                else
                {
                    if(PassPopUp.activeSelf == true)
                    {
                        PassPopUp.SetActive(false);
                    }
                }

            }
        }

        // ���� �н� ��ư�̾��ٸ� �˾��� ���ش�
        GameObject PassPopUpj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        if (PassPopUpj.name.Contains("Pass"))
        {
            PassPopUpj.SetActive(false);
        }

        pages[currentPage].SetActive(false);
        // �Ծ���? ������(�̶��� 3�������� �پ�Ѿ�� �Ѵ�)
        if(currentPage == 15)
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
        pass = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out passHit))
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
                    else if(selectBtnName == "Ball")
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
        // �ش� FailBtn�� ���õ� �ִϸ��̼� �������!
        // �׷����� �ϴ� ���� ������ ��ư�� ���� �˾ƾ��Ѵ� 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out failHit))
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
                    else if(selectBtnName == "Box")
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

        if(bookWorld.activeSelf == true)
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
        for(int i =0; i<broText.Count;i++)
        {
            broText[i].text = "������";
        }
    }

    public void Boy()
    {
        for (int i = 0; i < broText.Count; i++)
        {
            broText[i].text = "������";
        }
    }

    public void Mom()
    {
        for (int i = 0; i < parentText.Count; i++)
        {
            parentText[i].text = "����";
        }
    }

    public void Dad()
    {
        for (int i = 0; i < parentText.Count; i++)
        {
            parentText[i].text = "�ƺ�";
        }
    }

}
