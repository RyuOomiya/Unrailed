using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
    [SerializeField] ItemGrid _grid;
    public MeshRenderer _railColor;
    [Tooltip("�ݒu����Ă��Ȃ����[���̃}�e���A��")] public Material _railMaterial;
    [Tooltip("�ݒu���ꂽ���[���̃}�e���A��")] public Material _installedRailMaterial;
    public bool _isMove = false;

    void Awake()
    {
        if( _grid == null )
        {
            _grid = FindObjectOfType<ItemGrid>();
        }

        _railColor = GetComponent<MeshRenderer>();
    }
    public void Set(ItemType type)
    {
        _type = type;
    }
    
    void OnTriggerEnter(Collider other)
    {
        //���[����ݒu���ɂ����ɃA�C�e������������ߏ�Ɉړ�
        if(_isMove && other.TryGetComponent(out IPickableItem ip) && ip.GetType() == ItemType.Tool)
        {
                other.transform.position = new Vector3(_grid.Ground.transform.position.x
                                                        , -0.45f
                                                        , _grid.Ground.transform.position.z);
                _isMove = false;
        }
    }
    public void Action(GameObject hitObj)
    {
        ////HintRail�������ăX�y�[�X���������烌�[����ݒu����
        //if (hitObj.TryGetComponent(out HintRail hintrail) && hintrail.CanSet 
        //            && hintrail.GetComponent<MeshRenderer>().enabled == true)
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        _isMove = true;�@//ontriggerenter���̏������J�n
        //        Debug.Log("���[���ݒu");
        //        //_railColor.material = _installedRailMaterial; //�ݒu�������[���̃}�e���A���𒣂�ւ���
        //        //RailManager.Instance._rails.Add(this);  //���X�g�ɒǉ�
        //        //�}�X�ڂɐݒu
        //        //gameObject.transform.position
        //        //     = new Vector3(hitObj.transform.position.x, -0.45f, hitObj.transform.position.z);
        //        //PointManager�N���X���Q�Ƃ���HaveObjReset���\�b�h���Ăяo��
        //        PointManager pointManager= transform.parent.gameObject.GetComponent<PointManager>();
        //        //pointManager.HaveObjReset();
        //        hintrail.ChangeSetActive(false); //hintrail�̃n�C���C�g�\�����I�t
        //    } 
        //}
    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}