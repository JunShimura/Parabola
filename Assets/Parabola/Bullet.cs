using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    public bool initialized = true; // 初期化状態

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialized = true; // 初期化状態をtrueに設定
    }

    
    void FixedUpdate()
    {
        if (!initialized)
        {
            if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                transform.forward = rb.linearVelocity.normalized;
            }
            else
            {
                rb.isKinematic = true;
                Destroy(GetComponent<Collider>()); // コライダーを削除して、再度衝突しないようにする   
            }
        }
        else
        {
            initialized = false;
            return;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(initialized) return; // 初期化状態の場合は何もしない
        // 弾が何かに衝突したときの処理
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        rb.isKinematic = true;
        Destroy(GetComponent<Collider>()); // コライダーを削除して、再度衝突しないようにする   
    }
}
