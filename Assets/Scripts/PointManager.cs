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
    /// アイテム拾うか落とすか
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
    /// その場にアイテムを落とせるのか調べる
    /// </summary>
    void CanDrop()
    {
        if (_hitItems.Count < 1)
        {
            _canDrop = true;
        }
        //_hitItemsを回す
        foreach (GameObject obj in _hitItems)
        {
            //HintRailに触れてた時false
            if (obj.TryGetComponent(out HintRail hintRail))
            {
                _canDrop = false;
            }
            else
            {
                //Railに触れててそのRailが設置済みのRailの時もfalse
                if (obj.TryGetComponent(out Rail rail) && RailManager.Instance._rails.Contains(rail))
                {
                    _canDrop = false;
                }
                //どっちでもなかったらtrue
                else
                {
                    _canDrop = true;
                }
            }
        }
    }

    /// <summary>
    /// ツール用サーチ
    /// </summary>
    /// <param name="hitObj">当たったobj</param>
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

    /// <summary>アイテムを取る</summary>
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
