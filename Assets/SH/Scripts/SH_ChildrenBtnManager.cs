using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SH_ChildrenBtnManager : MonoBehaviour
{

    void Start()
    {
    }
    public float currentTime;
    int currentPage;
    public GameObject nextBtn;
    int i = 0;
    //public List<float> nextBtnShowTime = new List<float>();
    void Update()
    {

        if (SH_ChildrenFairyManager.Instance.pages[SH_ChildrenFairyManager.Instance.currentPage].GetComponent<AudioSource>().clip == null) return;
        // Á¶°Ç 1
        if (SH_ChildrenFairyManager.Instance.PassPopUp.activeSelf == true || SH_ChildrenFairyManager.Instance.FailPopUp.activeSelf == true) return;
        if (SH_ChildrenFairyManager.Instance.currentPage == 3 || SH_ChildrenFairyManager.Instance.currentPage == 5 || SH_ChildrenFairyManager.Instance.currentPage == 9 || SH_ChildrenFairyManager.Instance.currentPage == 14) return;
        print(SH_ChildrenFairyManager.Instance.pages[SH_ChildrenFairyManager.Instance.currentPage].GetComponent<AudioSource>().clip.length);
        currentTime += Time.deltaTime;
        if(currentTime > SH_ChildrenFairyManager.Instance.pages[SH_ChildrenFairyManager.Instance.currentPage].GetComponent<AudioSource>().clip.length && i<1)
        {
            nextBtn.SetActive(true);
            i++;
        }
    }

    public void NextBtn()
    {
        currentTime = 0;
        nextBtn.transform.localScale = new Vector3(0, 0, 0);
        nextBtn.SetActive(false);
        i = 0;
    }

    public void ResetBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
