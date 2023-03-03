using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainBase : MonoBehaviour
{
    //�f�o�b�N�p�ɕς���ꍇ������B�{����0.02f
    [SerializeField, Tooltip("��Ԃ̐i�ރX�s�[�h")]public float _moveSpeed = 2f;    
    [SerializeField, Tooltip("��Ԃ�������ł�Rail��Index")] int _nowRailIndex;
    [Tooltip("��Ԃ�������ł���Rail��Index")]public int NowRailIndex { get => _nowRailIndex; }
    [SerializeField, Tooltip("��Ԃ̉�]�̃X�s�[�h")] float _rotationSpeed = 0.2f;
    float _step;

    [SerializeField, Tooltip("��]�����ǂ���")] public bool _isRotate = false;
    [Tooltip("���ɉ�]")] bool _isRotateL = false;
    [Tooltip("�E�ɉ�]")] bool _isRotateR = false;
    
    [Tooltip("��Ԃ�Rigidbody")] Rigidbody _rb;
    [Tooltip("��Ԃ�Position")] Vector3 _trainPos;
    Quaternion _trainRot;
    
    
    [Tooltip("�x������Ƃ�����Ԃ̍�")]�@float _nextQuaternionL;
    [Tooltip("�x������Ƃ�����Ԃ̉E")] float _nextQuaternionR;
    
    //��Ԃ̑O�ƍ��E��Ray
    [Header("Raycast")]
    [SerializeField, Tooltip("��Raycast")] Transform _leftR;
    [SerializeField, Tooltip("�ERaycast")] Transform _rightR;
    [SerializeField, Tooltip("�ORaycast")] Transform _frontR;
    [Tooltip("Raycast���΂����W")] Vector3 _rayPos;

    [SerializeField] TrainDestroy _td;
    void Update()
    {
        TrainMove();

        //��]���łȂ��Ȃ�Ray���΂��Ď��̃��[���𔻒�
        if (!_isRotate)
        {
            TrainRaycast();
            _trainPos = gameObject.transform.position;
            _trainRot = gameObject.transform.rotation;

            //��Ԃ���]����������ʒu���C�����āA��Ԃ̂����̐��l���X�V����
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
        if (Physics.Raycast(_trainPos, rayVec, out hit, 1))
        {
            //�G���[�f���Ȃ��悤��_nowRailIndex�����X�g�̗v�f�� -1�܂ŗ����珈�����Ƃ΂�
            if (RailManager.Instance._rails.Count - 2 >= _nowRailIndex)
            {
                //�Ώۂ����[��&&����index�̃��[����������
                if (hit.collider.TryGetComponent(out Rail rail) &&
                rail == RailManager.Instance._rails.ElementAt(_nowRailIndex + 1))
                {
                    needRotate = true;
                    _nowRailIndex++;
                }
            }
      
        }
        Debug.DrawRay(_trainPos, rayVec.normalized * 1, Color.blue);
        return needRotate;
    }

    /// <summary>
    /// �E�ɋȂ���
    /// </summary>
    void RotateRight()
    {
        //�Ȃ���؂���������
        if (_step < 1)  //�Ȃ���؂��ĂȂ�
        {
            _moveSpeed = 0.02f;
            _isRotate = true;
            _step += _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_trainRot, Quaternion.Euler(0, _nextQuaternionR, 0), _step);

        }
        if (_step >= 1) //�Ȃ���؂���
        {
            //������
            _step = 0f;
            _trainRot = transform.rotation;
            _moveSpeed = 0.07f;
            _isRotate = false;
            _isRotateR = false;
            //_nowRailIndex++;
        }
    }

    /// <summary>
    /// ���ɋȂ���
    /// </summary>
    void RotateLeft()
    {
        if (_step < 1)  //�Ȃ���؂��ĂȂ�
        {
            _moveSpeed = 0.02f;
            _isRotate = true;
            _step += _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_trainRot, Quaternion.Euler(0, _nextQuaternionL, 0), _step);

        }
        if (_step >= 1) //�Ȃ���؂���
        {
            //������
            _step = 0f;
            _trainRot = transform.rotation;
            _moveSpeed = 0.07f;
            _isRotate = false;
            _isRotateL = false;
            //_nowRailIndex++;
        }
    }

    void TrainMove()
    {
        transform.position += transform.right * _moveSpeed * Time.deltaTime;
    }
}
