using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, IPickableItem
{
    [Tooltip("一定間隔で壊すためのタイマー")]float _timer = 0;
    [Tooltip("タイマーとセットで使う")]bool _isSwing;
    [Tooltip("アイテムタイプ"),SerializeField] ItemType _type;

    void Update()
    {
       CoolTime();
    }

    /// <summary>
    /// 木を壊すインターバル
    /// </summary>
    void CoolTime()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _isSwing = false;
        }
    }
    public void Action(GameObject hitObj)
    {
        if (hitObj.TryGetComponent(out Tree tree) && !_isSwing)
        {
            Debug.Log("ドン");
            _isSwing = true;
            tree.TreeLife -= 1;
            _timer = 1;
            //if(tree.TreeLife < 1)
            //{
            //    Destroy(tree.gameObject);
            //    PointManager.Instance._hitItems.Remove(tree.gameObject);
            //}
        }
    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}
