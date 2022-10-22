using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book
{
    public int id;
    public string title;
    public string createdAt;
    public List<Page> pages;
}

public class Page
{
    public string page;
    public List<TextData> texts;
    public List<ObjectData> objects;
}

public class TextData
{
    public string type;
    public Vector3 position;
    public Font font;
    public int size;
    public string content;
}

public class ObjectData
{
    public string type;
    public GameObject prefab;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
