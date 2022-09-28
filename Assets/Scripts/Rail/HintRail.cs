using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRail : MonoBehaviour, IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;
    public ItemType Type { get => _type; }
    [SerializeField]bool _canSet = true;
    public bool CanSet  {get => _canSet;}
    [SerializeField, Header("RailSetPointの親オブジェクト")]public GameObject _railSetManager;
    MeshRenderer _railRenderer;
    [SerializeField , Tooltip("ゴールした？")]public static bool _isGoal;
   
    private void Start()
    {
        _railRenderer = GetComponent<MeshRenderer>();
        _railRenderer.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        //プレイヤーがアイテムを持ってて、そのアイテムがレールだったらハイライトオン
        if(other.gameObject.TryGetComponent(out PointManager pm))
        {
            if (pm.HasObj && pm.PickedType == ItemType.Rail && _canSet)
            {
                ChangeSetActive(true);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        SetPointSeach(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        //離れたときハイライトオフ
        if (other.gameObject.TryGetComponent(out PointManager pm))
        {
            if (pm.HasObj && pm.PickedType == ItemType.Rail)
            {
                ChangeSetActive(false);
            }
        }
    }

    public void ChangeSetActive(bool enabled)
    {
        //プレイヤーが触れているときは表示して、離れたら表示しない
        gameObject.GetComponent<MeshRenderer>().enabled = enabled;
    }


    /// <summary>
    /// レールを設置できるポイントを探す
    /// </summary>
    public void SetPointSeach(GameObject hitObj)
    {
        //hitObjのenumのタイプがNotItemだったらhintRailは表示しない
        if (hitObj.TryGetComponent(out IPickableItem pickable))
        {
            if (pickable.Type == ItemType.NotItem)
            {
                _canSet = false;
            }   
        }
        else
        {
            _canSet = true;
        }
        //レールがあったら
        if (hitObj.TryGetComponent(out Rail rail))
        {
            //そのレールが設置済みのレールでなければ配列に追加する
            if (!RailManager.Instance._rails.Contains(rail))
            {
                Debug.Log("呼ばれたらしい");
                RailManager.Instance._rails.Add(rail);
            }
        }
        if(hitObj.gameObject.name == "GoalRail")
        {
            _isGoal = true;
        }
    }

    public void Action(GameObject hitObj)
    {

    }
}
