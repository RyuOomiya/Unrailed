using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointManager : MonoBehaviour
{
    [SerializeField, Tooltip("拾えるアイテム")]
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
        //_hitItemsを回す
        foreach (GameObject obj in _hitItems)
        {
            //HintRailに触れてた時
            if (obj.TryGetComponent(out HintRail hintRail))
            {
                Debug.Log("置けないよー");
                _canDrop = false;
            }
            else
            {
                //Railに触れててそのRailが設置済みのRailの時
                if (obj.TryGetComponent(out Rail rail) && RailManager.Instance._rails.Contains(rail))
                {
                    Debug.Log("置けないよー");
                    _canDrop = false;
                }
                else
                {
                    Debug.Log("置けるよー");
                    _canDrop = true;
                }
            }
            
            
            
            
            
        }

        //アイテム拾うか落とすか
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
    /// ツール用サーチ
    /// </summary>
    /// <param name="hitObj">当たったobj</param>
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
    /// アイテムを取る
    /// </summary>
    /// <param name="hitObj">拾ったアイテム</param>
    /// <param name="items">IPickableItemのスクリプトの変数</param>
    void ItemPick(GameObject hitObj, IPickableItem items)
    {
        //拾ったRailが設置済みのRailなら_railsリストから消す
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
        _haveObject.transform.parent = null;
        _haveObject = null;
        _iPickScript = null;
    }
}
