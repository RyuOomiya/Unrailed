using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//電車が壊れるときの処理
public class TrainDestroy : MonoBehaviour
{
    [SerializeField] GameObject _train;
    public static bool _isGameOver = false;
   
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ground ground))
        {
            _isGameOver = true;
        }
    }
}
