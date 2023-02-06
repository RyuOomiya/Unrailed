using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainBase : MonoBehaviour
{
    //デバック用に変える場合がある。本来は0.02f
    [SerializeField, Tooltip("列車の進むスピード")]public float _moveSpeed = 2f;    
    [SerializeField, Tooltip("列車が今踏んでるRailのIndex")] int _nowRailIndex;
    [Tooltip("列車が今踏んでいるRailのIndex")]public int NowRailIndex { get => _nowRailIndex; }
    [SerializeField, Tooltip("列車の回転のスピード")] float _rotationSpeed = 0.2f;
    float _step;
    [Tooltip("左に回転")] bool _isRotateL = false;
    [Tooltip("右に回転")] bool _isRotateR = false;
    
    [Tooltip("列車のRigidbody")] Rigidbody _rb;
    [Tooltip("列車のPosition")] Vector3 _trainPos;
    Quaternion _trainRot;
    [SerializeField, Tooltip("回転中かどうか")]public static bool _isRotate = false;
    
    float _nextQuaternionL;
    float _nextQuaternionR;
    
    //列車の前と左右にRay
    [Header("Raycast")]
    [SerializeField, Tooltip("左Raycast")] Transform _leftR;
    [SerializeField, Tooltip("右Raycast")] Transform _rightR;
    [SerializeField, Tooltip("前Raycast")] Transform _frontR;
    [Tooltip("Raycastを飛ばす座標")] Vector3 _rayPos;

    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        TrainMove();

        if (!_isRotate)
        {
            TrainRaycast();
            _trainPos = gameObject.transform.position;
            _trainRot = gameObject.transform.rotation;
            if (_trainRot == Quaternion.Euler(0, 0, 0))
            {
                _trainPos.z = RailManager.Instance._rails[NowRailIndex - 1].transform.position.z;
                _nextQuaternionL = -90;
                _nextQuaternionR = 90;
            }
            else if (_trainRot == Quaternion.Euler(0, -90, 0))
            {
                _trainPos.x = RailManager.Instance._rails[NowRailIndex - 1].transform.position.x;
                _nextQuaternionL = -180;
                _nextQuaternionR = 0;
            }
            else if (_trainRot == Quaternion.Euler(0, 90, 0))
            {
                _trainPos.x = RailManager.Instance._rails[NowRailIndex - 1].transform.position.x;
                _nextQuaternionR = 180;
                _nextQuaternionL = 0;
            }
            else if (_trainRot == Quaternion.Euler(0, 180, 0) || _trainRot == Quaternion.Euler(0, -180, 0))
            {
                _trainPos.z = RailManager.Instance._rails[NowRailIndex - 1].transform.position.z;

                _nextQuaternionL = 90;
                _nextQuaternionR = -90;
            }
            gameObject.transform.position = _trainPos;
        }

        if (_isRotateL)
        {
            RotateLeft();
        }
        if (_isRotateR)
        {
            RotateRight();
        }
    }

    /// <summary>Trainの進行用にRayを飛ばす</summary>
    void TrainRaycast()
    {

        if (Raycast(_frontR))
        {

        }
        if (Raycast(_rightR))
        {
            _isRotateR = true;
        }
        if (Raycast(_leftR))
        {
            _isRotateL = true;
        }
    }

    /// <summary>Rayを飛ばす</summary>
    /// <param name="rayPos">Rayを飛ばす場所</param>
    bool Raycast(Transform rayPos)
    {
        bool needRotate = false;
        Vector3 rayVec = rayPos.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(_trainPos, rayVec, out hit, 3))
        {
            //エラー吐かないように_nowRailIndexがリストの要素数 -1まで来たら処理をとばす
            if (RailManager.Instance._rails.Count - 2 >= _nowRailIndex)
            {
                if (hit.collider.TryGetComponent(out Rail rail) &&
                rail == RailManager.Instance._rails.ElementAt(_nowRailIndex + 1))
                {
                    needRotate = true;
                    _nowRailIndex++;
                }
            }
        }
        Debug.DrawRay(_trainPos, rayVec.normalized * 3, Color.blue);
        return needRotate;
    }
    void RotateRight()
    {
        if (_step < 1)
        {
            _moveSpeed = 75000f;
            _isRotate = true;
            _step += _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_trainRot, Quaternion.Euler(0, _nextQuaternionR, 0), _step);

        }
        if (_step >= 1)
        {
            _step = 0f;
            _trainRot = transform.rotation;
            _moveSpeed = 300000f;
            _isRotate = false;
            _isRotateR = false;

        }
    }

    void RotateLeft()
    {
        if (_step < 1)
        {
            _moveSpeed = 75000f;
            _isRotate = true;
            _step += _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_trainRot, Quaternion.Euler(0, _nextQuaternionL, 0), _step);

        }
        if (_step >= 1)
        {
            _step = 0f;
            _trainRot = transform.rotation;
            _moveSpeed = 300000f;
            _isRotate = false;
            _isRotateL = false;

        }

    }
    void TrainMove()
    {
        _rb.AddForce(transform.right * _moveSpeed * Time.deltaTime, ForceMode.Force);
    }
}
