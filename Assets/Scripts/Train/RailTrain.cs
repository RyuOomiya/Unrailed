using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RailTrain : MonoBehaviour
{
    [SerializeField] GameObject _rail;
    //�f�o�b�N�p�ɕς���ꍇ������B�{����0.02f
    [SerializeField, Tooltip("��Ԃ̐i�ރX�s�[�h")] public float _moveSpeed = 2f;
    [SerializeField, Tooltip("Train��������ł�Rail��Index")] int _nowRailIndex;
    public int NowRailIndex { get => _nowRailIndex; }
    [SerializeField, Tooltip("��Ԃ̉�]�̃X�s�[�h")] float _rotationSpeed = 0.2f;
    float _step;
    [Tooltip("���ɉ�]")] bool _isRotateL = false;
    [Tooltip("�E�ɉ�]")] bool _isRotateR = false;

    [Tooltip("��Ԃ�RIgidbody")] Rigidbody _rb;
    [Tooltip("Raycast�̃X�^�[�g���W")] Vector3 _trainPos;
    [SerializeField, Tooltip("��]�����ǂ���")] bool _isRotate = false;
    Quaternion _currentAngle;
    Quaternion _trainRot;
    float _nextQuaternionL;
    float _nextQuaternionR;

    [Header("Raycast")]
    [SerializeField, Tooltip("��Raycast")] Transform _leftR;
    [SerializeField, Tooltip("�ERaycast")] Transform _rightR;
    [SerializeField, Tooltip("�ORaycast")] Transform _frontR;
    [Tooltip("Raycast���΂����W")] Vector3 _rayPos;


    void Start()
    {
        _currentAngle = transform.rotation;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log(_nowRailIndex);
        InstantiateRail();
        TrainMove();
        if(!TrainManager._isRotate && FreightTrain._isRotate)
        {
            _moveSpeed = 50000f;
        }
        
        if (TrainManager._isRotate)
        {
            _moveSpeed = 70000f;
        }
        if (!TrainManager._isRotate && !FreightTrain._isRotate)
        {
            _moveSpeed = 200000f;
        }
        if (!_isRotate)
        {
            TrainRaycast();
            _trainPos = gameObject.transform.position;
            _trainRot = gameObject.transform.rotation;
            if (_trainRot == Quaternion.Euler(0, 0, 0))
            {
                if (NowRailIndex > 0)
                {
                    _trainPos.z = RailManager.Instance._rails[NowRailIndex - 1].transform.position.z;
                }
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

    /// <summary>Train�̐i�s�p��Ray���΂�</summary>
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

    /// <summary>Ray���΂�</summary>
    /// <param name="rayPos">Ray���΂��ꏊ</param>
    bool Raycast(Transform rayPos)
    {
        bool needRotate = false;
        Vector3 rayVec = rayPos.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(_trainPos, rayVec, out hit, 3))
        {
            //�G���[�f���Ȃ��悤��_nowRailIndex�����X�g�̗v�f�� -1�܂ŗ����珈�����Ƃ΂�
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
            transform.rotation = Quaternion.Slerp(_currentAngle, Quaternion.Euler(0, _nextQuaternionR, 0), _step);

        }
        if (_step >= 1)
        {
            _step = 0f;
            _currentAngle = transform.rotation;
            _moveSpeed = 200000f;
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
            transform.rotation = Quaternion.Slerp(_currentAngle, Quaternion.Euler(0, _nextQuaternionL, 0), _step);

        }
        if (_step >= 1)
        {
            _step = 0f;
            _currentAngle = transform.rotation;
            _moveSpeed = 200000f;
            _isRotate = false;
            _isRotateL = false;

        }

    }
    void TrainMove()
    {
        _rb.AddForce(transform.right * _moveSpeed * Time.deltaTime, ForceMode.Force);
    }

    void InstantiateRail()
    {
        if (FreightTrain.Instance._instanceRail)
        {
            Instantiate
                (_rail, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z),
                Quaternion.identity);
            FreightTrain.Instance._instanceRail = false;
        }

    }
}
