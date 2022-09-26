using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [Tooltip("列車の進むスピード")]float _moveSpeed = 10.0f;
    [Tooltip("列車のRIgidbody")]Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        _rb.AddForce(transform.right * _moveSpeed);
    }
}
