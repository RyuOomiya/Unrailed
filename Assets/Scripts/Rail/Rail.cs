using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
    public ItemType Type { get => _type; }

    public void Set(ItemType type)
    {
        _type = type;
    }
    void Start()
    {
        
    }

    void Update()
    {

    }
    public void Action(GameObject hitObj)
    {
        //HintRail�������ăX�y�[�X���������烌�[����ݒu����
        if (hitObj.TryGetComponent(out HintRail hintrail) && hintrail.CanSet)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�ݒu�[");
                RailManager.Instance._rails.Add(this);
                gameObject.transform.position
                     = new Vector3(hitObj.transform.position.x, -0.25f, hitObj.transform.position.z);
                //PointManager�N���X���Q�Ƃ���HaveObjReset���\�b�h���Ăяo��
                PointManager pointManager= transform.parent.gameObject.GetComponent<PointManager>();
                pointManager.HaveObjReset();
            } 
        }
    }
}