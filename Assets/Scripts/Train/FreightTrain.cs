using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FreightTrain : MonoBehaviour
{
    [Header("myCollider")]
    [SerializeField] GameObject _woodsSetCollider;
    [SerializeField] GameObject _stonesSetCollider;

    //シングルトンにするための処理
    private static FreightTrain _instance;
    public static FreightTrain Instance { get => _instance; }

    [SerializeField, Header("設置された木材達")]
    public List<GameObject> _woods = new List<GameObject>();
    [SerializeField, Header("設置された石材達")]
    public List<GameObject> _stones = new List<GameObject>();
    [Tooltip("Rail作ってー")] public bool _instanceRail = false;

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

    void Update()
    {
        ListCheck();
        if(_woods.Count > 0)
        {
            foreach(GameObject w in _woods)
            {
                w.transform.position = _woodsSetCollider.transform.position;
            }
        }
        if(_stones.Count > 0)
        {
            foreach(GameObject s in _stones)
            {
                s.transform.position = _stonesSetCollider.transform.position;  
            }
        }
        
    }

    /// <summary>
    /// 二つのリストをチェックしてRail作れるか確認する
    /// </summary>
    void ListCheck()
    {
        GameObject _destroyWood;
        GameObject _destroyStone;
        if (_woods.Count > 0 && _stones.Count > 0)
        {
            _destroyWood = _woods[0];
            _destroyStone = _stones[0];
            PointManager.Instance._hitItems.Remove(_woods[0]);
            PointManager.Instance._hitItems.Remove(_stones[0]);
            _woods.Remove(_woods[0]);
            _stones.Remove(_stones[0]);
            Destroy(_destroyWood);
            Destroy(_destroyStone);
            _instanceRail = true;
        }
    }
}








