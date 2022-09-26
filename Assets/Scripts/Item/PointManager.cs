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
    GameObject _nearItem;

    public ItemType PickedType { get => _iPickScript.Type; }
    public bool HasObj { get => _iPickScript != null; }
    [SerializeField] bool _isHave { get => _haveObject != null; }

    void Start()
    {
        _gridScript = GetComponent<ItemGrid>();
    }
    void Update()
    {
        _nearItem = _hitItems.OrderBy(x =>
                 Vector3.SqrMagnitude(gameObject.transform.position - x.transform.position)
                 ).FirstOrDefault();
        CanDrop();
        PickOrDrop();
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
    /// �A�C�e���E�������Ƃ���
    /// </summary>
    void PickOrDrop()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (HasObj && _canDrop)
            {
                ItemDrop();
            }
            if (_hitItems.Count > 0)
            {
                HitObjSeach(_nearItem);
            }
        }
    }
    /// <summary>
    /// ���̏�ɃA�C�e���𗎂Ƃ���̂����ׂ�
    /// </summary>
    void CanDrop()
    {
        if (_hitItems.Count < 1)
        {
            _canDrop = true;
        }
        //_hitItems����
        foreach (GameObject obj in _hitItems)
        {
            //HintRail�ɐG��Ă���false
            if (obj.TryGetComponent(out HintRail hintRail))
            {
                _canDrop = false;
            }
            else
            {
                //Rail�ɐG��ĂĂ���Rail���ݒu�ς݂�Rail�̎���false
                if (obj.TryGetComponent(out Rail rail) && RailManager.Instance._rails.Contains(rail))
                {
                    _canDrop = false;
                }
                //�ǂ����ł��Ȃ�������true
                else
                {
                    _canDrop = true;
                }
            }
        }
    }

    /// <summary>
    /// �c�[���p�T�[�`
    /// </summary>
    /// <param name="hitObj">��������obj</param>
    public void HitObjSeach(GameObject hitObj)
    {
        if (hitObj.TryGetComponent(out IPickableItem items) && items.Type != ItemType.NotItem && !_isHave)
        {
            ItemPick(hitObj, items);
        }

        if (_iPickScript != null)
        {
            _iPickScript.Action(hitObj);
        }
    }

    /// <summary>�A�C�e�������</summary>
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
