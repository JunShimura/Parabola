using UnityEngine;

public class BulletUpdater : MonoBehaviour
{
    public Vector3[] trajectory; // 軌道座標配列
    public float interval = 0.02f; // 1ステップの時間間隔
    int currentIndex = 0;

    public void Initialize(Vector3[] positions, float interval)
    {
        this.trajectory = positions;
        this.interval = interval;
        currentIndex = 0;
        if (trajectory != null && trajectory.Length > 0)
            transform.position = trajectory[0];
    }

    void FixedUpdate()
    {
        if (trajectory == null || currentIndex >= trajectory.Length) return;

        // 前の位置を保存
        Vector3 prevPos = transform.position;

        // 現在の位置に移動
        transform.position = trajectory[currentIndex];

        // 移動ベクトルを算出し、進行方向にforwardを向ける
        Vector3 moveDir = transform.position - prevPos;
        if (moveDir.sqrMagnitude > 0.000001f)
        {
            transform.forward = moveDir.normalized;
        }

        currentIndex++;

        // 着弾後の処理（必要に応じて）
        if (currentIndex >= trajectory.Length)
        {
            // Destroy(gameObject); // 自動消滅させたい場合
        }
    }
}