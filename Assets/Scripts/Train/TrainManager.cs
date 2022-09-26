using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField, Tooltip("��Ԃ̐i�ރX�s�[�h")] float _moveSpeed = 0.02f;
    [Tooltip("��Ԃ�RIgidbody")] Rigidbody _rb;
    [SerializeField, Tooltip("��Raycast")] Transform _leftR;
    [SerializeField, Tooltip("�ERaycast")] Transform _rightR;
    [SerializeField, Tooltip("�ORaycast")] Transform _frontR;

    bool _isRotate;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        TrainRotate();
    }

    /// <summary>Train�̐i�s�p��Ray���΂�</summary>
    void TrainRaycast()
    {
        if(Raycast(_frontR))
        {

        }
    }

    /// <summary>Ray���΂�</summary>
    /// <param name="rayPos">Ray���΂�����</param>
    Vector3 Raycast(Transform rayPos)
    {
        Vector3 trainPos = transform.position;

        Vector3 rayVec = rayPos.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(trainPos, rayVec, out hit, 3))
        {
            if(hit.collider.TryGetComponent(out Rail rail) && hit.collider == RailManager.Instance._rails[RailManager.Instance._rails.Count - 1])
            {
                return hit.collider.transform.position;
              
            }
        }
        Debug.DrawRay(trainPos, rayVec);
    }
    void TrainRotate()
    {

    }
    void TrainMove()
    {
        _rb.AddForce(transform.right * _moveSpeed, ForceMode.Force);
    }
}
