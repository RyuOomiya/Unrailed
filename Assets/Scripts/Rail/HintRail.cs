using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRail : MonoBehaviour, IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;
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
        if (other.gameObject.TryGetComponent(out PointManager pm))
        {
            if (pm.HasObj && pm.PickedType == ItemType.Rail && _canSet)
            {
                ChangeSetActive(true);
            }

            SetPointSeach(other.gameObject);
        }
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
            if (pickable.GetType() == ItemType.NotItem)
            {
                _canSet = false;
            }   
        }
        else
        {
            _canSet = true;
        }
        if (hitObj.gameObject.name == "GoalRail")
        {
            _isGoal = true;
        }
    }

    public void Action(GameObject hitObj)
    {

    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}
