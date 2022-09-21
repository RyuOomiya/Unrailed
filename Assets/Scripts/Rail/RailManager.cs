using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RailManager : MonoBehaviour
{
    //シングルトンにするための処理
    private static RailManager _instance;
    public static RailManager Instance { get => _instance; }
    //RailSetクラスを参照するための処理↓
    [SerializeField, Header("RailSetクラスがついてるオブジェクト")]
    GameObject _railSetObj;
    [Tooltip("RailSetスクリプト")] RailSet _railSetScript;
    //レール関係↓
    [Tooltip("リストの要素数の保持")]private int _railsCount;
    [SerializeField,Header("設置されたレール達")]
    public List<Rail> _rails = new List<Rail>();
    
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
        _railSetScript = _railSetObj.GetComponent<RailSet>();
    }

    void Update()
    {
        SetPointSummon();
    }

    void SetPointSummon()
    {
        if(_railsCount != _rails.Count)
        {
            //上下左右にSeach用Pointを飛ばす
            _railSetScript.transform.position = _rails[_rails.Count - 1].transform.position;
            //リストの要素数を更新
            _railsCount = _rails.Count;
        }

    }
    public void AddRail(Rail rail)
    {
        _rails.Add(rail);
    }

}
