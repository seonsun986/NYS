using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_PlazaAnimal : MonoBehaviour
{
    public enum State
    {
        Idle, // 가만히 있는시간 3초
        Move, // 움직이는 시간 3초
        Eat, // 바닥 뜯어먹기 5초
        Rest, // 쉬기 3초
        Interaction // 클릭 시 상호작용 3초?
    }

    public State state;
    Animator anim;
    public GameObject text;

    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip clickSound;

    void Start()
    {
        text.SetActive(false);
        anim = GetComponent<Animator>();
        state = State.Idle;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
            break;

            case State.Move:
                Move();
            break;

            case State.Eat:
                Eat();
            break;

            case State.Rest:
                Rest();
            break;

            case State.Interaction:
                Interaction();
            break;
        }
    }


    // Idle에서 랜덤으로 다른 상태로 넘겨주기
    int changeState = 0;
    float changeTime = 0;
    void Idle()
    {
        changeTime += Time.deltaTime;

        // 3초가 지나면
        if (changeTime > 3)
        {
            // 랜덤 숫자받기
            changeState = Random.Range(0, 3);
            Debug.Log(changeState);

            //changeState = 1; Move TEST

            // 숫자에 따라 상태 전환하기
            if (changeState == 0)
            {
                changeTime = 0;
            }
            else if (changeState == 1)
            {
                state = State.Move;
                changeTime = 0;
            }
            else if (changeState == 2)
            {
                state = State.Eat;
                changeTime = 0;
            }
            else if (changeState == 3)
            {
                state = State.Rest;
                changeTime = 0;
            }
        }
    }

    bool move = false;
    Vector3 center;
    Vector3 dir;
    RaycastHit hit;
    void Move()
    {
        // 움직이는 애니메이션 재생
        anim.SetBool("Move", true);
        
        // 5의 반지름을 가진 원 내에서 랜덤 값을 가져옴
        if (!move)
        {
            center = Random.insideUnitSphere * 5;
            center.y = 0;
            center += transform.position;

            dir = center - transform.position;
            dir.Normalize();

            move = true;
        }

        // 갈 방향으로 Ray 쏘기
        Ray ray = new Ray(transform.position, dir);
        // 내가 갈 방향 벡터 그리기
        Debug.DrawLine(transform.position, center, Color.red);
        // 혹시 가려는곳이 벽, 플레이어, 다른동물이면 갈 곳 다시 계산
        if (Physics.Raycast(ray, out hit, 3))
        {
            if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 6 || hit.collider.gameObject.tag == "Animal")
            {
                move = false;
            }
        }
        
        if (move)
        {
            // 나아갈 방향으로 천천히 돌려주기
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2);

            // 목표 지점과 가까이 왔다면 Idle 상태로 다시 돌려주기
            if (Vector3.Distance(transform.position, center) > 0.3f)
                transform.position += dir * 0.1f * Time.deltaTime;
            else
            {
                anim.SetBool("Move", false);
                state = State.Idle;
                move = false;
            }
        }
    }


    void Eat()
    {
        // 먹는 애니메이션 재생
        anim.SetBool("Eat", true);

        changeTime += Time.deltaTime;

        // 2초가 지나면 Idle로 전환
        if (ranNum < 1)
        {
            ranNum = Random.Range(2, 5);
        }
        if (changeTime > ranNum)
        {
            anim.SetBool("Eat", false);
            state = State.Idle;
            changeTime = 0;
            ranNum = 0;
        }
    }

    int ranNum = 0;
    void Rest()
    {
        // 쉬는 애니메이션 재생
        anim.SetBool("Rest", true);

        changeTime += Time.deltaTime;
        
        if (ranNum < 1)
        {
            ranNum = Random.Range(2, 5);
        }
        if (changeTime > ranNum)
        {
            anim.SetBool("Rest", false);
            state = State.Idle;
            changeTime = 0;
            ranNum = 0;
        }
    }

    public Vector3 player;
    bool soundOn = false;
    void Interaction()
    {
        // 플레이어 쳐다보기
        Vector3 dir = player - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20);
        //transform.rotation = Quaternion.Euler(0, -player.y, 0);

        if (!soundOn)
        {
            audioSource.PlayOneShot(clickSound);
            soundOn = true;
        }

        // 점프 애니메이션 재생
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump1"))
        {
            changeTime = 0;
            text.SetActive(true);
            text.transform.LookAt(Camera.main.transform.position);
            anim.SetBool("Interaction", true);
        }

        changeTime += Time.deltaTime;

        if (changeTime > 2)
        {
            anim.SetBool("Move", false);
            anim.SetBool("Eat", false);
            anim.SetBool("Rest", false);
            anim.SetBool("Interaction", false);
            text.SetActive(false);
            changeTime = 0;
            soundOn = false;
            state = State.Idle;
        }
    }
}
