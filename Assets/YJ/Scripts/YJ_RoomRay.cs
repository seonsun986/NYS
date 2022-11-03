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
        Gizmos.color = _rayColor;

        // �Լ� �Ķ���� : ���� ��ġ, Box�� ���� ������, Ray�� ����, RaycastHit ���, Box�� ȸ����, BoxCast�� ������ �Ÿ�
        if (true == Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, Vector3.down, out RaycastHit hit, transform.rotation, _maxDistance))
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
        }
        else
        {
            // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position, Vector3.down * _maxDistance);
        }
    }
}
