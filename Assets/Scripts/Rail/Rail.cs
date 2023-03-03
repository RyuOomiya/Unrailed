using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
    [SerializeField] ItemGrid _grid;
    bool _isMove = false;

    public void Set(ItemType type)
    {
        _type = type;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(_isMove && other.TryGetComponent(out IPickableItem ip) && ip.GetType() == ItemType.Tool)
        {
                Debug.Log("a");
                other.transform.position = new Vector3(_grid.Ground.transform.position.x
                                                        , -0.45f
                                                        , _grid.Ground.transform.position.z);
                _isMove = false;
        }
    }
    public void Action(GameObject hitObj)
    {
        //HintRail�������ăX�y�[�X���������烌�[����ݒu����
        if (hitObj.TryGetComponent(out HintRail hintrail) && hintrail.CanSet && hintrail.GetComponent<MeshRenderer>().enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isMove = true;
                Debug.Log("�ݒu�[");
                RailManager.Instance._rails.Add(this);
                gameObject.transform.position
                     = new Vector3(hitObj.transform.position.x, -0.45f, hitObj.transform.position.z);
                //PointManager�N���X���Q�Ƃ���HaveObjReset���\�b�h���Ăяo��
                PointManager pointManager= transform.parent.gameObject.GetComponent<PointManager>();
                pointManager.HaveObjReset();
                hintrail.ChangeSetActive(false);
            } 
        }
    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}