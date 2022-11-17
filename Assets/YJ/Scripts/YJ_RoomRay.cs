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

    void OnDrawGizmos()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Room");

        Gizmos.color = _rayColor;

        // �Լ� �Ķ���� : ���� ��ġ, Box�� ���� ������, Ray�� ����, RaycastHit ���, Box�� ȸ����, BoxCast�� ������ �Ÿ�
        if (true == Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, Vector3.down, out RaycastHit hit, transform.rotation, _maxDistance, layerMask))
        {
            // Hit�� �������� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

            // Hit�� ������ �ڽ��� �׷��ش�.
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, transform.lossyScale);

            if (hit.transform.name == "Bank02")
            {
                return;
            }

            transform.position = hit.point + new Vector3(Random.Range(-5,5),2);
            //iTween.ScaleTo(gameObject, iTween.Hash("x", 1.2, "y", 1.2, "z", 1.2, "easeType", "easeOutExpo", "time", 0.5f));
            iTween.ScaleTo(gameObject, iTween.Hash("x", 1.2, "y", 1.2, "z", 1.2, "easeType", "easeInOutBack", "time", 0.5f));
        }
        else
        {
            // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position, Vector3.down * _maxDistance);
        }
    }
}
