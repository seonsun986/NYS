using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BookUI : MonoBehaviour
{
    public enum Book
    {
        �鼳����,
        �ŵ�����,
        �����Ǹ�����,
        ���̾߱�,
    }

    public Book selectedBook = Book.�鼳����;
    public GameObject fairyTaleManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SelectBook(Book book)
    {
        selectedBook = book;
        print(book);
    }

    public void ClickBook1()
    {
        SelectBook(Book.�鼳����);
    }

    public void ClickBook2()
    {
        SelectBook(Book.�ŵ�����);
    }

    public void ClickBook3()
    {
        SelectBook(Book.�����Ǹ�����);
    }
    public void ClickBook4()
    {
        SelectBook(Book.���̾߱�);
        gameObject.SetActive(false);
        fairyTaleManager.SetActive(true);
    }
}
