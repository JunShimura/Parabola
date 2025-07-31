using UnityEngine;

public class CanonLogical : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] GameObject bulletprefab;
    [SerializeField] float apexHeightOffset = 5f; // 頂点の高さオフセット
    [SerializeField] float simulationInterval = 0.02f; // シミュレーション間隔

    void Update()
    {
        if (targetObject == null) return;

        Vector3 start = transform.position;
        Vector3 end = targetObject.transform.position;

        if (Input.GetMouseButtonDown(0) && bulletprefab != null)
        {
            GameObject bullet = Instantiate(bulletprefab, start, Quaternion.identity);
            BulletUpdater updater = bullet.AddComponent<BulletUpdater>();
            Vector3[] trajectory = GenerateSymmetricParabolaTrajectory(start, end, apexHeightOffset, simulationInterval);
            updater.Initialize(trajectory, simulationInterval);
        }
    }

    // 頂点を中間座標の真上に置き、Yが対称な放物線を生成（物理法則に基づく）
    Vector3[] GenerateSymmetricParabolaTrajectory(Vector3 start, Vector3 end, float apexOffset, float interval)
    {
        Vector3 mid = (start + end) * 0.5f;
        float apexY = Mathf.Max(start.y, end.y) + apexOffset;

        // XZ方向の距離と単位ベクトル
        Vector3 diffXZ = new Vector3(end.x - start.x, 0, end.z - start.z);
        float distance = diffXZ.magnitude;
        Vector3 dirXZ = diffXZ.normalized;

        // 頂点までの水平距離
        float halfDistance = distance * 0.5f;

        // 頂点までの高さ差
        float h0 = start.y;
        float h1 = apexY;

        // 重力
        float g = -Physics.gravity.y;

        // 頂点までの飛翔時間を物理法則から算出
        // h1 = h0 + Vy * t - 0.5 * g * t^2
        // halfDistance = Vxz * t
        // t = halfDistance / Vxz
        // h1 = h0 + Vy * t - 0.5 * g * t^2
        // Vy = (h1 - h0 + 0.5 * g * t^2) / t

        // 水平速度は任意（ここでは1秒で到達するように仮定）
        float t_half = 1.0f;
        float Vxz = halfDistance / t_half;
        float Vy = (h1 - h0 + 0.5f * g * t_half * t_half) / t_half;

        // 全体の飛翔時間
        float totalTime = t_half * 2.0f;

        int steps = Mathf.Max(2, Mathf.CeilToInt(totalTime / interval));
        Vector3[] positions = new Vector3[steps + 1];

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps * totalTime;
            // XZ座標
            Vector3 posXZ = start + dirXZ * (Vxz * t);
            // Y座標
            float y = h0 + Vy * t - 0.5f * g * t * t;
            positions[i] = new Vector3(posXZ.x, y, posXZ.z);
        }
        // 最後の座標は必ずend
        positions[positions.Length - 1] = end;
        return positions;
    }
}