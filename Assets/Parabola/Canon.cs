using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] GameObject targetObject; // ターゲットオブジェクト
    [SerializeField] GameObject bulletprefab; // 弾のプレハブ
    [SerializeField] float initialSpeed = 10f; // 初速

    void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = targetObject.transform.position;
        Vector3 velocity = CalculateParabolaVelocity(start, end, initialSpeed, Physics.gravity.y);

        // Canon自身のforwardを発射方向に向ける
        if (velocity.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        if (Input.GetMouseButtonDown(0) && targetObject != null && bulletprefab != null)
        {
            // 発射方向にforwardを向けて生成
            GameObject bullet = Instantiate(bulletprefab, start, Quaternion.LookRotation(velocity));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(velocity, ForceMode.VelocityChange);
            }
        }
    }

    // 放物線の初速ベクトルを計算
    Vector3 CalculateParabolaVelocity(Vector3 start, Vector3 end, float speed, float gravityY)
    {
        Vector3 diff = end - start;
        Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);
        float distance = diffXZ.magnitude;

        float yOffset = diff.y;
        float g = Mathf.Abs(gravityY);

        // 2次方程式で飛翔時間を算出
        float speed2 = speed * speed;
        float underSqrt = speed2 * speed2 - g * (g * distance * distance + 2 * yOffset * speed2);
        if (underSqrt < 0) underSqrt = 0; // 到達不能な場合

        float sqrt = Mathf.Sqrt(underSqrt);
        float t = (speed2 + sqrt) / (g * distance);

        // 初速ベクトル
        Vector3 velocity = diffXZ.normalized * speed * Mathf.Cos(Mathf.Atan(1 / t));
        velocity.y = speed * Mathf.Sin(Mathf.Atan(1 / t));
        return velocity;
    }
}