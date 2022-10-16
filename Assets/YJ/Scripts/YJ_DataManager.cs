using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region �α��� �� �Ѱ��� ����
[System.Serializable]
public class LoginInfo
{
    public string ID;
    public string PW;
}
#endregion


#region �޾ƿ� ���� ����
[System.Serializable]
public class UserInfo
{
    public string ID;
    public string PW;
    public string name;
    public string birth;
    public string position;
}
#endregion



public class YJ_DataManager : MonoBehaviour
{
    public static YJ_DataManager instance;

    //public List<string> listPhotoUrl = new List<string>();

    private void Awake()
    {
        // ���࿡ instance�� null�̶��
        if (instance == null)
        {
            // instance�� ���� �ְڴ�.
            instance = this;
            // ���� �ı����� �ʵ��� �ϰڴ�.
            DontDestroyOnLoad(gameObject);
        }
        // �׷��� ������
        else
        {
            // ���� �ı��ϰڴ�.
            Destroy(gameObject);
        }

        //�׽�Ʈ ����
        //listPhotoUrl.Add("https://img-lb.inews24.com/image_joy/201409/facebookexternalhit/face/626x352/1411714196274_1_155055.jpg");
        //listPhotoUrl.Add("https://s.pstatic.net/static/www/mobile/edit/20220901/cropImg_728x360_103965399363771670.jpeg");
        //listPhotoUrl.Add("https://s.pstatic.net/dthumb.phinf/?src=%22https%3A%2F%2Fs.pstatic.net%2Ftvcast.phinf%2F20220901_154%2FJ3z5X_1662039736463RSiWq_JPEG%2F6310adac65495848f6a8af74_edit_0_1662038045923.jpg%22&type=nf464_260");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
