using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointManager : MonoBehaviour
{
    [Header("シングルトン関連")]
    private static PointManager _instance;
    public static PointManager Instance { get => _instance; }

    [SerializeField, Tooltip("拾えるアイテム")]
    public List<GameObject> _hitItems = new List<GameObject>();
    GameObject _haveObject;
    ItemGrid _gridScript;
    IPickableItem _iPickScript;
    [SerializeField] bool _canDrop = false;
    GameObject _nearItem;
    

    public ItemType PickedType { get => _iPickScript.GetType(); }
    public bool HasObj { get => _iPickScript != null; }
    [SerializeField,Tooltip("アイテム持ってる？")] bool _isHave { get => _haveObject != null; }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _gridScript = GetComponent<ItemGrid>();
    }
    void Update()
    {
        NearItemSeach();
        CanDrop();
        PickOrDrop();
        HitItemRoop();
    }

    //当たったアイテムをリストに追加
    private void OnTriggerEnter(Collider other)
    {
        //地面だったら_hitItemsリストに追加しない
        if (other.gameObject.TryGetComponent(out Ground ground))
        {
            return;
        }
        else
        {
            _hitItems.Add(other.gameObject);
        }
    }

    //離れたアイテムをリストから削除
    private void OnTriggerExit(Collider other)
    {
        _hitItems.Remove(other.gameObject);
    }

    /// <summary> ActionメソッドのためにhitItemリストを回す </summary>
    void HitItemRoop()
    {
        for (int i = 0; i < _hitItems.Count; i++)
        {
            if (_iPickScript != null) _iPickScript.Action(_hitItems[i]);
        }
    }

    /// <summary> 一番近いアイテムを探す </summary>
    void NearItemSeach()
    {
        _nearItem = _hitItems.OrderBy(x =>
               Vector3.SqrMagnitude(gameObject.transform.position - x.transform.position)
               ).FirstOrDefault();
    }

    /// <summary> アイテム拾うか落とすか </summary>
    void PickOrDrop()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //アイテムを落とす
            if (HasObj && _canDrop)
            {
                ItemDrop();
            }
            //そのアイテムが拾えるか確認
            if (_hitItems.Count > 0)
            {
                HitObjSeach(_nearItem);
            }
        }
    }
    /// <summary> その場にアイテムを落とせるのか調べる </summary>
    void CanDrop()
    {
        if (_hitItems.Count < 1)
        {
            _canDrop = true;
        }
        //_hitItemsを回す
        foreach (GameObject obj in _hitItems)
        {
            if(obj != null)
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
    }

    /// <summary> 拾えるアイテムか確認 </summary>
    /// <param name="hitObj">当たったobj</param>
    public void HitObjSeach(GameObject hitObj)
    {
        if (hitObj.TryGetComponent(out IPickableItem items) && items.GetType() != ItemType.NotItem && !_isHave)
        {
            ItemPick(hitObj, items);
        }
    }

    /// <summary> アイテムを取る </summary>
    /// <param name="hitObj">拾ったアイテム</param>
    /// <param name="items">IPickableItemのスクリプトの変数</param>
    void ItemPick(GameObject hitObj, IPickableItem items)
    {
        //拾ったRailが設置済みのRailなら_railsリストから消す
        if (hitObj.TryGetComponent(out Rail rail) && RailManager.Instance._rails.Contains(rail) && !_isHave)
        {
            RailManager.Instance._rails.Remove(rail);
        }
        _hitItems.Remove(hitObj);
        _iPickScript = items;
        _haveObject = hitObj;
        Input.ResetInputAxes();
        hitObj.transform.parent = this.gameObject.transform;
        hitObj.transform.position = this.gameObject.transform.position;
    }

    /// <summary> アイテム落とす </summary>
    private void ItemDrop()
    {
        if (!HasObj) return;

        //一番近いマスに落とす
        _haveObject.transform.position
            = new Vector3(_gridScript.Point.transform.position.x,
                        0, _gridScript.Point.transform.position.z);
        Debug.Log(_haveObject.transform.position);
        HaveObjReset();
    }

    /// <summary> 持っているアイテムの情報をリセットする処理 </summary>
    public void HaveObjReset()
    {
        _haveObject.transform.parent = null;
        _haveObject = null;
        _iPickScript = null;
    }
}
