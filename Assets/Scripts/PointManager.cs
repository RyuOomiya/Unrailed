using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointManager : MonoBehaviour
{
    [SerializeField, Tooltip("�E����A�C�e��")]
    List<GameObject> _hitItems = new List<GameObject>();
    GameObject _haveObject;
    ItemGrid _gridScript;
    IPickableItem _iPickScript;
    [SerializeField] bool _canDrop = false;

    public ItemType PickedType { get => _iPickScript.Type; }
    public bool HasObj { get => _iPickScript != null; }

    void Start()
    {
        _gridScript = GetComponent<ItemGrid>();
    }
    void Update()
    {
        //_hitItems����
        foreach (GameObject obj in _hitItems)
        {
            //HintRail�ɐG��Ă���
            if (obj.TryGetComponent(out HintRail hintRail))
            {
                Debug.Log("�u���Ȃ���[");
                _canDrop = false;
            }
            else
            {
                //Rail�ɐG��ĂĂ���Rail���ݒu�ς݂�Rail�̎�
                if (obj.TryGetComponent(out Rail rail) && RailManager.Instance._rails.Contains(rail))
                {
                    Debug.Log("�u���Ȃ���[");
                    _canDrop = false;
                }
                else
                {
                    Debug.Log("�u�����[");
                    _canDrop = true;
                }
            }
            
            
            
            
            
        }

        //�A�C�e���E�������Ƃ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (HasObj && _canDrop)
            {
                ItemDrop();
            }
            if (_hitItems.Count > 0)
            {
                ToolSeach(_hitItems[0]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IPickableItem>() != null)
        {
            _hitItems.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _hitItems.Remove(other.gameObject);
    }

    /// <summary>
    /// �c�[���p�T�[�`
    /// </summary>
    /// <param name="hitObj">��������obj</param>
    public void ToolSeach(GameObject hitObj)
    {
        if (hitObj.TryGetComponent(out IPickableItem items) && items.Type != ItemType.NotItem)
        {
            ItemPick(hitObj, items);
        }

        if (_iPickScript != null)
        {
            _iPickScript.Action(hitObj);
        }
    }

    /// <summary>
    /// �A�C�e�������
    /// </summary>
    /// <param name="hitObj">�E�����A�C�e��</param>
    /// <param name="items">IPickableItem�̃X�N���v�g�̕ϐ�</param>
    void ItemPick(GameObject hitObj, IPickableItem items)
    {
        //�E����Rail���ݒu�ς݂�Rail�Ȃ�_rails���X�g�������
        if (hitObj.TryGetComponent(out Rail rail) && RailManager.Instance._rails.Contains(rail))
        {
            RailManager.Instance._rails.Remove(rail);
        }
        _hitItems.Remove(hitObj);
        _iPickScript = items;
        _haveObject = hitObj;
        Input.ResetInputAxes();
        hitObj.transform.parent = this.gameObject.transform;
        hitObj.transform.position = this.gameObject.transform.position;
        Debug.Log("Tool�������");
    }

    /// <summary>
    /// �A�C�e�����Ƃ�
    /// </summary>
    private void ItemDrop()
    {
        if (_haveObject == null)
        {
            return;
        }
        Debug.Log("�A�C�e���𗎂Ƃ���");
        //��ԋ߂��}�X�ɗ��Ƃ�
        _haveObject.transform.position
            = new Vector3(_gridScript.Point.transform.position.x,
                        0, _gridScript.Point.transform.position.z);
        Debug.Log(_haveObject.transform.position);
        HaveObjReset();
    }

    /// <summary>
    /// �A�C�e�����Ƃ����烊�Z�b�g
    /// </summary>
    public void HaveObjReset()
    {
        _haveObject.transform.parent = null;
        _haveObject = null;
        _iPickScript = null;
    }
}
