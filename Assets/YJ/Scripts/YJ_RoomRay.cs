using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �浹�ϴ� ��ü�� �ִ� �� �ڽ� ������� RayCast�� �Ѵ�.
/// </summary>
public class YJ_RoomRay : MonoBehaviour
{
    // ray�� ����
    [SerializeField]
    private float _maxDistance = 20.0f;

    // ray�� ����
    [SerializeField]
    private Color _rayColor = Color.red;

    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if (time > 1)
        {
            iTween.ScaleTo(transform.GetChild(1).gameObject, iTween.Hash("x", 0.025, "y", 0.025, "z", 0.025, "easeType", "easeInOutBack", "time", 1f));
        }
    }

    List<Vector3> veclist = new List<Vector3>();

    void OnDrawGizmos()
    {
        veclist.Add(new Vector3(0, 0, 0));

        if (gameObject.name == "Type1")
        {
            veclist.Add(new Vector3(-1.03f, 0, -0.65f));
            veclist.Add(new Vector3(1.1f, 0, -0.65f));
            veclist.Add(new Vector3(1.1f, 0, 0.84f));
            veclist.Add(new Vector3(-1.03f, 0, 0.84f));
        }
        else if(gameObject.name == "Type2")
        {
            veclist.Add(new Vector3(-0.8f, 0, -0.72f));
            veclist.Add(new Vector3(0.75f, 0, -0.72f));
            veclist.Add(new Vector3(0.75f, 0, 0.9f));
            veclist.Add(new Vector3(-0.8f, 0, 0.9f));
        }
        else if(gameObject.name == "Type3")
        {
            veclist.Add(new Vector3(-1.02f, 0, -0.98f));
            veclist.Add(new Vector3(0.99f, 0, -0.98f));
            veclist.Add(new Vector3(0.985f, 0, 1.055f));
            veclist.Add(new Vector3(-1.02f, 0, 1.055f));
        }


        int layerMask = 1<<12;// << LayerMask.NameToLayer("Terrain");
        Gizmos.color = _rayColor;


        for (int i = 0; i < veclist.Count; i++)
        {
            if (true == Physics.BoxCast(transform.position + veclist[i], transform.lossyScale * 0.5f, Vector3.down, out RaycastHit hit, transform.rotation, _maxDistance, layerMask))
            {
                // Hit�� �������� ray�� �׷��ش�.
                Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

                Debug.Log(hit.collider.gameObject.name);

                // Hit�� ������ �ڽ��� �׷��ش�.
                Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, transform.lossyScale * 2);

                transform.position = hit.point + new Vector3(Random.Range(0, 6), 5, Random.Range(-2, 4));
            }
            //else
            //{
                // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
                //Gizmos.DrawRay(transform.position, Vector3.down * _maxDistance);
                //Gizmos.DrawRay(transform.position + veclist[1], Vector3.down * _maxDistance); // ������ ��
                //Gizmos.DrawRay(transform.position + veclist[2], Vector3.down * _maxDistance); // ���� ��
                //Gizmos.DrawRay(transform.position + veclist[3], Vector3.down * _maxDistance); // ���� �Ʒ�
                //Gizmos.DrawRay(transform.position + veclist[4], Vector3.down * _maxDistance); // ������ �Ʒ�
                //Gizmos.DrawRay(transform.position + new Vector3(-1.02f, 0, -0.98f), Vector3.down * _maxDistance);
                //Gizmos.DrawRay(transform.position + new Vector3(0.99f, 0, -0.98f), Vector3.down * _maxDistance);
                //Gizmos.DrawRay(transform.position + new Vector3(0.985f, 0, 1.055f), Vector3.down * _maxDistance);
                //Gizmos.DrawRay(transform.position + new Vector3(-1.02f, 0, 1.055f), Vector3.down * _maxDistance);
            //}
        }


        //// ���� ��ġ, Box�� 5��� �������� ��, Ray�� ����, RaycastHit, Box�� ȸ����, BoxCast�� ������ �Ÿ�
        //if (true == Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, Vector3.down, out RaycastHit hit, transform.rotation, _maxDistance, layerMask))
        //{
        //    // Hit�� �������� ray�� �׷��ش�.
        //    Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

        //    Debug.Log(hit.collider.gameObject.name);

        //    // Hit�� ������ �ڽ��� �׷��ش�.
        //    Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, transform.lossyScale * 2);

        //    transform.position = hit.point + new Vector3(Random.Range(4, 6), 5, Random.Range(-2, 4));
        //    Debug.Log("�׷��� ���� �� ��ġ�� ? " + transform.position);
        //}
    }
}
