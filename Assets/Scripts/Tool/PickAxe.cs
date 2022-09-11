using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxe : MonoBehaviour , IPickableItem
{
    [Tooltip("一定間隔で壊すためのタイマー")] float _timer = 0;
    [Tooltip("タイマーとセットで使う")] bool _isSwing;
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;

    public ItemType Type { get => _type; }
    void Update()
    {
        CoolTime();
    }

    /// <summary>
    /// 岩を壊すインターバル
    /// </summary>
    void CoolTime()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _isSwing = false;
        }
    }
    public  void Action(GameObject hitObj)
    {
        if (hitObj.TryGetComponent(out Rock rock) && !_isSwing)
        {
            Debug.Log("ドン");
            _isSwing = true;
            rock.RockLife -= 1;
            _timer = 1;
        }
    }
}

