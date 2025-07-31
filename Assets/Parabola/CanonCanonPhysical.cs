using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] GameObject targetObject; // ターゲットオブジェクト
    [SerializeField] GameObject bulletprefab; // 弾のプレハブ
    [SerializeField] float initialSpeed = 10f; // 初速
    [SerializeField] float maxElevationAngle = 45f; // 最大仰角（度）
    [SerializeField] bool mximizeFlyTime = false; // 滞空時間の最大化フラグ

    void Update()
    {
        if (targetObject == null) return;

        Vector3 start = transform.position;
        Vector3 end = targetObject.transform.position;
        Vector3 velocity = CalculateParabolaVelocity(start, end, initialSpeed, Physics.gravity.y, maxElevationAngle, mximizeFlyTime);

        // Canon自身のforwardを発射方向に向ける
        if (velocity.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        if (Input.GetMouseButtonDown(0) && bulletprefab != null)
        {
            // 発射方向にforwardを向けて生成
            GameObject bullet = Instantiate(bulletprefab, start, Quaternion.LookRotation(velocity));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = velocity; // velocityを直接セット
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    // 放物線の初速ベクトルを計算
    Vector3 CalculateParabolaVelocity(Vector3 start, Vector3 end, float speed, float gravityY, float maxAngleDeg, bool maximizeFlyTime)
    {
        Vector3 diff = end - start;
        Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);
        float distance = diffXZ.magnitude;
        float yOffset = diff.y;
        float g = Mathf.Abs(gravityY);

        float speed2 = speed * speed;
        float discriminant = speed2 * speed2 - g * (g * distance * distance + 2 * yOffset * speed2);

        if (distance < 0.01f)
        {
            // ほぼ同じ位置の場合は真上に撃つ
            return Vector3.up * speed;
        }

        float angleRad;
        if (discriminant < 0)
        {
            // 到達不能: 最大仰角でターゲット方向に発射
            angleRad = Mathf.Deg2Rad * maxAngleDeg;
        }
        else
        {
            float sqrt = Mathf.Sqrt(discriminant);
            float tLow = (speed2 + sqrt) / (g * distance);
            float tHigh = (speed2 - sqrt) / (g * distance);

            float t = tLow;
            if (maximizeFlyTime && tHigh > 0)
            {
                t = tHigh;
            }
            angleRad = Mathf.Atan(1 / t);
        }

        Vector3 velocity = diffXZ.normalized * speed * Mathf.Cos(angleRad);
        velocity.y = speed * Mathf.Sin(angleRad);
        return velocity;
    }
}