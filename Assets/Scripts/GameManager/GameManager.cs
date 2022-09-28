using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("TrainManager")]
    [SerializeField, Tooltip("TrainManagerがついてるオブジェクト")] GameObject _train;
    [Tooltip("TrainManagerスクリプト")] TrainManager _trainManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        //_trainからTrainManagerスクリプトを取り出す
        _trainManagerScript = _train.GetComponent<TrainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Goal();
    }

    void Goal()
    {
        if (HintRail._isGoal)
        {
            _trainManagerScript._moveSpeed = 5000000f;
        }
    }
}
