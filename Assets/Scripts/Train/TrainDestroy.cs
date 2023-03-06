using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//“dÔ‚ª‰ó‚ê‚é‚Æ‚«‚Ìˆ—
public class TrainDestroy : MonoBehaviour
{
    [SerializeField] GameObject _train;
    public bool _isGameOver = false;
   
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ground ground))
        {
            BreakTrain();
        }
    }

    public void BreakTrain()
    {
        _isGameOver = true;
    }
}
