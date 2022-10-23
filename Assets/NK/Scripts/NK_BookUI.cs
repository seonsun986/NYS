using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BookUI : MonoBehaviour
{
    public enum Book
    {
        백설공주,
        신데렐라,
        오즈의마법사,
        용이야기,
    }

    public Book selectedBook = Book.백설공주;
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
        SelectBook(Book.백설공주);
    }

    public void ClickBook2()
    {
        SelectBook(Book.신데렐라);
    }

    public void ClickBook3()
    {
        SelectBook(Book.오즈의마법사);
    }
    public void ClickBook4()
    {
        SelectBook(Book.용이야기);
        gameObject.SetActive(false);
        fairyTaleManager.SetActive(true);
    }
}
