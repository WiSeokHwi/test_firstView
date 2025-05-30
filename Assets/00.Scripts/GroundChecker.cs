using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Boxcast Property")]
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask groundLayer;

    [Header("Debug")]
    [SerializeField] private bool drawGizmo;

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Gizmos.color = Color.cyan;

        // BoxCast가 검사하는 위치 = 현재 위치 - 아래 방향 * 거리
        Vector3 boxCenter = transform.position - transform.up * maxDistance;

        // 회전 고려
        Quaternion rotation = transform.rotation;

        // BoxCast는 boxSize가 반지름이므로, DrawCube에선 *2 필요
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize * 2f);
    }

    public bool IsGrounded()
    {
        return Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, groundLayer);
    }
}
