using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public List<PhotonView> players = new List<PhotonView> ();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 현재 방에 있는 아이들
    public List<PhotonView> children = new List<PhotonView>();
    public PhotonView photonView;
    public void AddPlayer(PhotonView pv)
    {
        children.Add(pv);
    }
}
