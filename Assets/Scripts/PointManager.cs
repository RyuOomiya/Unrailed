using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField, Tooltip("拾えるアイテム")]
    List<GameObject> _hitItems = new List<GameObject>();
    GameObject _haveObject;
    ItemGrid _gridScript;
    IPickableItem _iPickScript;
    [Tooltip("ツール持ってる?")] bool _isHave = false;

    void Start()
    {
        _gridScript = GetComponent<ItemGrid>();
    }
    void Update()
    {
        //アイテム拾うか落とすか
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
    /// ツール用サーチ
    /// </summary>
    /// <param name="hitObj">当たったobj</param>
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
    /// アイテムを取る
    /// </summary>
    /// <param name="hitObj">拾ったアイテム</param>
    /// <param name="items">IPickableItemのスクリプトの変数</param>
    void ItemPick(GameObject hitObj, IPickableItem items)
    {
        _hitItems.Remove(hitObj);
        _iPickScript = items;
        _haveObject = hitObj;
        _isHave = true;
        Input.ResetInputAxes();
        hitObj.transform.parent = this.gameObject.transform;
        Debug.Log("Toolを取った");
    }

    /// <summary>
    /// アイテム落とす
    /// </summary>
    private void ItemDrop()
    {
        if (_haveObject == null)
        {
            return;
        }
        Debug.Log("アイテムを落とした");
        //一番近いマスに落とす
        _haveObject.transform.position
            = new Vector3(_gridScript.Point.transform.position.x,
                        0, _gridScript.Point.transform.position.z); 
        Debug.Log(_haveObject.transform.position);
        HaveObjReset();
    }

    /// <summary>
    /// アイテム落としたらリセット
    /// </summary>
    public void HaveObjReset()
    {
        _isHave = false;
        _haveObject.transform.parent = null;
        _haveObject = null;
        _iPickScript = null;
    }
}
