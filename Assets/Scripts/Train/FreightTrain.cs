using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FreightTrain : MonoBehaviour
{


    //�f�o�b�N�p�ɕς���ꍇ������B�{����200000f
    [SerializeField, Tooltip("��Ԃ̐i�ރX�s�[�h")] public float _moveSpeed = 2f;
    [SerializeField, Tooltip("Train��������ł�Rail��Index")] int _nowRailIndex;
    public int NowRailIndex { get => _nowRailIndex; }
    [SerializeField, Tooltip("��Ԃ̉�]�̃X�s�[�h")] float _rotationSpeed = 0.2f;
    float _step;
    [Tooltip("���ɉ�]")] bool _isRotateL = false;
    [Tooltip("�E�ɉ�]")] bool _isRotateR = false;

    [Tooltip("��Ԃ�RIgidbody")] Rigidbody _rb;
    [Tooltip("Raycast�̃X�^�[�g���W")] Vector3 _trainPos;
    [SerializeField, Tooltip("��]�����ǂ���")]public static bool _isRotate = false;
    Quaternion _currentAngle;
    Quaternion _trainRot;
    float _nextQuaternionL;
    float _nextQuaternionR;

    [Header("Raycast")]
    [SerializeField, Tooltip("��Raycast")] Transform _leftR;
    [SerializeField, Tooltip("�ERaycast")] Transform _rightR;
    [SerializeField, Tooltip("�ORaycast")] Transform _frontR;
    [Tooltip("Raycast���΂����W")] Vector3 _rayPos;

    [Header("myCollider")]
    [SerializeField] GameObject _woodsSetCollider;
    [SerializeField] GameObject _stonesSetCollider;

    //�V���O���g���ɂ��邽�߂̏���
    private static FreightTrain _instance;
    public static FreightTrain Instance { get => _instance; }

    [SerializeField, Header("�ݒu���ꂽ�؍ޒB")]
    public List<GameObject> _woods = new List<GameObject>();
    [SerializeField, Header("�ݒu���ꂽ�΍ޒB")]
    public List<GameObject> _stones = new List<GameObject>();
    [Tooltip("Rail����ā[")] public bool _instanceRail = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _currentAngle = transform.rotation;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ListCheck();
        TrainMove();
        if(_woods.Count > 0)
        {
            foreach(GameObject w in _woods)
            {
                w.transform.position = _woodsSetCollider.transform.position;
            }
        }
        if(_stones.Count > 0)
        {
            foreach(GameObject s in _stones)
            {
                s.transform.position = _stonesSetCollider.transform.position;  
            }
        }
        //�ق��̗�Ԃ���]���̏���  
        if (TrainManager._isRotate)
        {
            _moveSpeed = 70000f;
        }
        if(!TrainManager._isRotate)
        {
            _moveSpeed = 200000f;
        }
        //���g����]���ĂȂ��Ƃ�
        if (!_isRotate)
        {
            TrainRaycast();
            _trainPos = gameObject.transform.position;
            _trainRot = gameObject.transform.rotation;
            //���g�̍���Rotation�ɂ���Ēl��ς���
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
            //��]��ɍ��W���C������
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

    /// <summary>
    /// ��̃��X�g���`�F�b�N����Rail���邩�m�F����
    /// </summary>
    void ListCheck()
    {
        GameObject _destroyWood;
        GameObject _destroyStone;
        if (_woods.Count > 0 && _stones.Count > 0)
        {
            _destroyWood = _woods[0];
            _destroyStone = _stones[0];
            _woods.Remove(_woods[0]);
            _stones.Remove(_stones[0]);
            //PointManager.Instance._hitItems.Remove(_woods[0]);
            //PointManager.Instance._hitItems.Remove(_stones[0]);
            Destroy(_destroyWood);
            Destroy(_destroyStone);
            _instanceRail = true;
        }
    }
}








