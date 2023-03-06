using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] GameObject _train;
    float _trainX;
    // Update is called once per frame
    void Update()
    {
        _trainX = _train.transform.position.x;
        transform.position = new Vector3(_trainX, transform.position.y, transform.position.z);
    }
}
