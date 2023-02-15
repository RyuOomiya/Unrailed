using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    //シングルトンにするための処理
    private static RailManager _instance;
    public static RailManager Instance { get => _instance; }

    [Header("HintRail")]
    [SerializeField, Header("HintRailクラスがついてるオブジェクト")]
    GameObject _hintRailObj;
    [Tooltip("HintRailスクリプト")] HintRail _hintRailScript;

    [Header("Rail")]
    [Tooltip("リストの要素数の保持")]private int _railsCount;
    [SerializeField,Header("設置されたレール達")]
    public List<Rail> _rails = new List<Rail>();

    [Header("TrainManager")]
    [SerializeField, Tooltip("TrainManagerがついてるオブジェクト")] GameObject _train;
    [Tooltip("TrainManagerスクリプト")] TrainBase _trainBaseScript;
    
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
        //最初にレールの数を代入する
        _railsCount = _rails.Count;
        //_railSetObjからRailSetスクリプトを取り出す
        _hintRailScript = _hintRailObj.GetComponent<HintRail>();
        //_trainからTrainManagerスクリプトを取り出す
        _trainBaseScript = _train.GetComponent<TrainBase>();
    }

    void Update()
    {
        SetPointSummon();
        ForSetRail();
    }

    /// <summary>
    /// リストの最後尾のレールが変わるたびにSeach用Pointを飛ばす
    /// </summary>
    void SetPointSummon()
    {
        if(_railsCount != _rails.Count)
        {
            //上下左右にSeach用Pointを飛ばす
            _hintRailScript._railSetManager.transform.position =
                new Vector3(_rails[_rails.Count - 1].transform.position.x , 0 , _rails[_rails.Count - 1].transform.position.z);
            //リストの要素数を更新
            _railsCount = _rails.Count;
        }
    }

    /// <summary>
    /// リストの最後以外のRailはenumタイプをNotItemにする
    /// </summary>
    void ForSetRail()
    {
        foreach(var rail in _rails)
        {
            //rail == _rails[_rails.Count - 1]? rail.Set(ItemType.NotItem) : rail.Set(ItemType.Rail);
            if (rail != _rails[_rails.Count - 1])
            {
                rail.Set(ItemType.NotItem);
            }
            //最後のRailだけはenumタイプをRailにする
            if(rail == _rails[_rails.Count - 1])
            {
                rail.Set(ItemType.Rail);
            }

            if(rail == _rails[_trainBaseScript.NowRailIndex])
            {
                rail.Set(ItemType.NotItem);
            }
        }
            
    }
}
