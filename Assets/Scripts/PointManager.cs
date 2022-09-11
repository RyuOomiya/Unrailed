using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    GameObject _haveObject;
    ItemGrid _gridScript;
    IPickableItem _iPickScript;
    [Tooltip("ツール持ってる?")]bool _isHave = false;

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
    /// ツール用サーチ
    /// </summary>
    /// <param name="hitObj">当たったobj</param>
    public void ToolSeach(GameObject hitObj)
    {
        if (hitObj.TryGetComponent(out IPickableItem items))
        {
            //Debug.Log("入ってます");
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
    /// アイテム取る
    /// </summary>
    void ItemPick(GameObject hitObj , IPickableItem items)
    {
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
        if (_isHave)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("アイテムを落とした");
                _haveObject.transform.position =
                   new Vector3(_gridScript.Point.transform.position.x, 
                                0, _gridScript.Point.transform.position.z); //一番近いマスに落とす
                _haveObject.transform.parent = null;
                _haveObject = null;     //落としたらリセット
                _iPickScript = null;    //落としたらリセット
                _isHave = false;
            }
        }
    }
}
