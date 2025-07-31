using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] GameObject floorObject;

    void Update()
    {
        if (Camera.main == null || floorObject == null) return;

        // マウスカーソル位置からRayを生成
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // floorObjectのColliderを取得
        Collider floorCollider = floorObject.GetComponent<Collider>();
        if (floorCollider == null) return;

        // RaycastHit情報
        RaycastHit hit;
        // floorObjectのLayerだけにヒットさせる場合はLayerMaskを使う
        if (floorCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            // ヒットした位置にこのGameObjectを移動
            transform.position = hit.point;
        }
    }
}
