using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.forward = rb.linearVelocity.normalized;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // 弾が何かに衝突したときの処理
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        Destroy(gameObject); // 衝突後に弾を削除
    }
}
