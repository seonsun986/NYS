using UnityEngine;

/// <summary>
/// 직선으로 충돌하는 객체가 있는 지 박스 모양으로 RayCast를 한다.
/// </summary>
public class YJ_RoomRay : MonoBehaviour
{
    // ray의 길이
    [SerializeField]
    private float _maxDistance = 20.0f;

    // ray의 색상
    [SerializeField]
    private Color _rayColor = Color.red;

    void OnDrawGizmos()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Room");

        Gizmos.color = _rayColor;

        // 함수 파라미터 : 현재 위치, Box의 절반 사이즈, Ray의 방향, RaycastHit 결과, Box의 회전값, BoxCast를 진행할 거리
        if (true == Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, Vector3.down, out RaycastHit hit, transform.rotation, _maxDistance, layerMask))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);

            // Hit된 지점에 박스를 그려준다.
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
            // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
            Gizmos.DrawRay(transform.position, Vector3.down * _maxDistance);
        }
    }
}
