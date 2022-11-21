using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_PlazaAnimal : MonoBehaviour
{
    public enum State
    {
        Idle, // ������ �ִ½ð� 3��
        Move, // �����̴� �ð� 3��
        Eat, // �ٴ� ���Ա� 5��
        Rest, // ���� 3��
        Interaction // Ŭ�� �� ��ȣ�ۿ� 3��?
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


    // Idle���� �������� �ٸ� ���·� �Ѱ��ֱ�
    int changeState = 0;
    float changeTime = 0;
    void Idle()
    {
        changeTime += Time.deltaTime;

        // 3�ʰ� ������
        if (changeTime > 3)
        {
            // ���� ���ڹޱ�
            changeState = Random.Range(0, 3);
            Debug.Log(changeState);

            //changeState = 1; Move TEST

            // ���ڿ� ���� ���� ��ȯ�ϱ�
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
        // �����̴� �ִϸ��̼� ���
        anim.SetBool("Move", true);
        
        // 5�� �������� ���� �� ������ ���� ���� ������
        if (!move)
        {
            center = Random.insideUnitSphere * 5;
            center.y = 0;
            center += transform.position;

            dir = center - transform.position;
            dir.Normalize();

            move = true;
        }

        // �� �������� Ray ���
        Ray ray = new Ray(transform.position, dir);
        // ���� �� ���� ���� �׸���
        Debug.DrawLine(transform.position, center, Color.red);
        // Ȥ�� �����°��� ��, �÷��̾�, �ٸ������̸� �� �� �ٽ� ���
        if (Physics.Raycast(ray, out hit, 3))
        {
            if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 6 || hit.collider.gameObject.tag == "Animal")
            {
                move = false;
            }
        }
        
        if (move)
        {
            // ���ư� �������� õõ�� �����ֱ�
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2);

            // ��ǥ ������ ������ �Դٸ� Idle ���·� �ٽ� �����ֱ�
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
        // �Դ� �ִϸ��̼� ���
        anim.SetBool("Eat", true);

        changeTime += Time.deltaTime;

        // 2�ʰ� ������ Idle�� ��ȯ
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
        // ���� �ִϸ��̼� ���
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
        // �÷��̾� �Ĵٺ���
        Vector3 dir = player - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20);
        //transform.rotation = Quaternion.Euler(0, -player.y, 0);

        if (!soundOn)
        {
            audioSource.PlayOneShot(clickSound);
            soundOn = true;
        }

        // ���� �ִϸ��̼� ���
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
