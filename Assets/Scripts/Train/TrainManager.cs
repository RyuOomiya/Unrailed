using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [Tooltip("��Ԃ̐i�ރX�s�[�h")]float _moveSpeed = 10.0f;
    [Tooltip("��Ԃ�RIgidbody")]Rigidbody _rb;
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
