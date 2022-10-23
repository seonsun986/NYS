using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// text�� ���� class
public class TextData
{
    public string type;
    public Vector3 position;
    public string font;
    public int size;
    public string content;
}

// obj�� ���� class
public class OjbData
{
    public string type;
    public string prefab;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

public class SH_SceneObj : MonoBehaviour
{
    // �� ������Ʈ �ȿ� �����ϴ� �͵� : InputField, Gameobject
    // �̶� InputField�� canvas�ȿ� �ٽ� �����Ѵ�'
    // ���ӿ�����Ʈ�� �� ������Ʈ �ڽ����� ������ InputField���� ��쿡�� Canvas���� �÷����ϴµ�
    int mySceneNum;
    public enum ObjType
    {
        text,
        obj,
    }

    public ObjType objType;
    void Start()
    {
        if(GetComponent<SH_InputField>())
        {
            objType = ObjType.text;
        }
        else
        {
            objType = ObjType.obj;
        }
        // ���� ���� �ִ� Scene ��ȣ�� �����Ѵ�
        mySceneNum = SH_BtnManager.Instance.i;
        print(objType);
    }

    void Update()
    {
        
    }
}
