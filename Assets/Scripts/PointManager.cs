using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField, Tooltip("�E����A�C�e��")]
    List<GameObject> _hitItems = new List<GameObject>();
    GameObject _haveObject;
    ItemGrid _gridScript;
    IPickableItem _iPickScript;
    [Tooltip("�c�[�������Ă�?")] bool _isHave = false;

    void Start()
    {
        _gridScript = GetComponent<ItemGrid>();
    }
    void Update()
    {
        //�A�C�e���E�������Ƃ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isHave)
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
        if (hitObj.TryGetComponent(out IPickableItem items))
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
        _hitItems.Remove(hitObj);
        _iPickScript = items;
        _haveObject = hitObj;
        _isHave = true;
        Input.ResetInputAxes();
        hitObj.transform.parent = this.gameObject.transform;
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
        _isHave = false;
        _haveObject.transform.parent = null;
        _haveObject = null;
        _iPickScript = null;
    }
}
