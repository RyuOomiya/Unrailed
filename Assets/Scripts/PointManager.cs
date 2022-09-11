using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    GameObject _haveObject;
    ItemGrid _gridScript;
    IPickableItem _iPickScript;
    [Tooltip("�c�[�������Ă�?")]bool _isHave = false;

    private void Start()
    {
        _gridScript = GetComponent<ItemGrid>();
    }
    private void Update()
    {
        ItemDrop();
    }
    private void OnTriggerStay(Collider other)
    {
        ToolSeach(other.gameObject);
    }

    /// <summary>
    /// �c�[���p�T�[�`
    /// </summary>
    /// <param name="hitObj">��������obj</param>
    public void ToolSeach(GameObject hitObj)
    {
        if (hitObj.TryGetComponent(out IPickableItem items))
        {
            //Debug.Log("�����Ă܂�");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ItemPick(hitObj ,items);
            }
        }

        if (_iPickScript != null)
        {
            _iPickScript.Action(hitObj);
        }
    }
    
    /// <summary>
    /// �A�C�e�����
    /// </summary>
    void ItemPick(GameObject hitObj , IPickableItem items)
    {
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
        if (_isHave)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�A�C�e���𗎂Ƃ���");
                _haveObject.transform.position =
                   new Vector3(_gridScript.Point.transform.position.x, 
                                0, _gridScript.Point.transform.position.z); //��ԋ߂��}�X�ɗ��Ƃ�
                _haveObject.transform.parent = null;
                _haveObject = null;     //���Ƃ����烊�Z�b�g
                _iPickScript = null;    //���Ƃ����烊�Z�b�g
                _isHave = false;
            }
        }
    }
}
