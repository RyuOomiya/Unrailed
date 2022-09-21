using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSet : MonoBehaviour
{
    [SerializeField, Header("RailSetPointの親オブジェクト")] GameObject _railSetManager;
    public GameObject RailSetManager { get => _railSetManager; }
    [Header("ハイライト表示用")] GameObject _hintRail;

    void Start()
    {
        _hintRail = transform.GetChild(0).gameObject;
    }

    void OnTriggerStay(Collider other)
    {
        SetPointSeach(other.gameObject);
    }
    /// <summary>
    /// レールを設置できるポイントを探す
    /// </summary>
    public void SetPointSeach(GameObject hitObj)
    {
        //hitObjのenumのタイプがResourceだったらhintRailは表示しない
        if (hitObj.TryGetComponent(out IPickableItem pickable))
        {
            if(pickable.Type == ItemType.Resource)
            {
                _hintRail.gameObject.SetActive(false);
            }
        }
        //上の処理を抜けてレールがあったら
        else if (hitObj.TryGetComponent(out Rail rail))
        {
            //そのレールが設置済みのレールでなければ配列に追加する
            if (!RailManager.Instance._rails.Contains(rail))
            {
                Debug.Log("呼ばれたらしい");
                RailManager.Instance._rails.Add(rail);
            }
        }
    }
}
