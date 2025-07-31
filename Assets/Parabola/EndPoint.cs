using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] GameObject floorObject;

    void Update()
    {
        if (Camera.main == null || floorObject == null) return;

        // �}�E�X�J�[�\���ʒu����Ray�𐶐�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // floorObject��Collider���擾
        Collider floorCollider = floorObject.GetComponent<Collider>();
        if (floorCollider == null) return;

        // RaycastHit���
        RaycastHit hit;
        // floorObject��Layer�����Ƀq�b�g������ꍇ��LayerMask���g��
        if (floorCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            // �q�b�g�����ʒu�ɂ���GameObject���ړ�
            transform.position = hit.point;
        }
    }
}
