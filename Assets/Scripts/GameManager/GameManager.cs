using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("TrainManager")]
    [SerializeField, Tooltip("TrainManagerがついてるオブジェクト")] GameObject _train;
    [Tooltip("TrainManagerスクリプト")] TrainBase _trainManagerScript;
    [SerializeField] TrainDestroy _trainDestroy;
    [SerializeField] HintRail _hintRail;
    // Start is called before the first frame update
    void Start()
    {
        //_trainからTrainManagerスクリプトを取り出す
        _trainManagerScript = _train.GetComponent<TrainBase>();
    }

    // Update is called once per frame
    void Update()
    {
        Goal();
        GameOver();

    }

    void Goal()
    {
        if (_hintRail._isGoal)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }

    void GameOver()
    {
        if(_trainDestroy._isGameOver)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
