using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("TrainManager")]
    [SerializeField, Tooltip("TrainManager�����Ă�I�u�W�F�N�g")] GameObject _train;
    [Tooltip("TrainManager�X�N���v�g")] TrainManager _trainManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        //_train����TrainManager�X�N���v�g�����o��
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
