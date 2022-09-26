using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField, Tooltip("列車の進むスピード")] float _moveSpeed = 0.02f;
    [Tooltip("列車のRIgidbody")] Rigidbody _rb;
    [SerializeField, Tooltip("左Raycast")] Transform _leftR;
    [SerializeField, Tooltip("右Raycast")] Transform _rightR;
    [SerializeField, Tooltip("前Raycast")] Transform _frontR;

    bool _isRotate;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        TrainRotate();
    }

    /// <summary>Trainの進行用にRayを飛ばす</summary>
    void TrainRaycast()
    {
        if(Raycast(_frontR))
        {

        }
    }

    /// <summary>Rayを飛ばす</summary>
    /// <param name="rayPos">Rayを飛ばす向き</param>
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
