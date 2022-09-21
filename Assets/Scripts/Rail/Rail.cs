using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;

    public ItemType Type { get => _type; }

    void Start()
    {
        //�ŏ��̃��[�������X�g�ɓ����
        RailManager.Instance.AddRail(this);
    }

    void Update()
    {

    }
    public void Action(GameObject hitObj)
    {
        //HintRail�������ăX�y�[�X���������烌�[����ݒu����
        if (hitObj.TryGetComponent(out HintRail hintrail))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameObject.transform.position
                     = new Vector3(hitObj.transform.position.x, 0, hitObj.transform.position.z);
                //PointManager�N���X���Q�Ƃ���HaveObjReset���\�b�h���Ăяo��
                PointManager pointManager= transform.parent.gameObject.GetComponent<PointManager>();
                pointManager.HaveObjReset();
            }
            
        }
    }
}