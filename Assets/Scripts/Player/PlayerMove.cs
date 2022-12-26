using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] float _speed = 10.0f;
    [SerializeField] float _sprintSpeed = 30.0f;
    [SerializeField] float _x;
    [SerializeField] float _z;
    private float _nowSpeed;
    [Tooltip("�_�b�V���̃N�[���^�C��")] private float _coolTime;    
    Rigidbody _rb;
    Vector3 _playerPos;

    // Start is called before the first frame update
    void Start()
    {
        _playerPos = GetComponent<Transform>().position;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        _x = Input.GetAxisRaw("Horizontal") * _speed;
        _z = Input.GetAxisRaw("Vertical") * _speed;

        Vector3 diff = transform.position - _playerPos;
        //�x�N�g���̒�����0.01f���傫���ꍇ�Ƀv���C���[�̌�����ς��鏈��������
        if (diff.magnitude > 0.05f)
        {
            //�x�N�g���̏���Quaternion.LookRotation�Ɉ����n����]�ʂ��擾���v���C���[����]������
            transform.rotation = Quaternion.LookRotation(diff);
        }
        //�v���C���[�̈ʒu���X�V
        _playerPos = transform.position;
        //�X�v�����g�̃N�[���^�C��
        _coolTime -= Time.deltaTime;
        //�V�t�g���������Ƃ��ɃX�v�����g���ăN�[���^�C�������Z�b�g����
        if (Input.GetKeyDown(KeyCode.LeftShift) && _coolTime < 0)
        {
            _nowSpeed = _speed;
            _speed = _sprintSpeed;
            Invoke(nameof(CoolTime), 0.1f);
            _coolTime = 1.0f;
        }
    }

    void CoolTime()
    {
        _speed = _nowSpeed;
    }
    
    void FixedUpdate()
    {
        var speedVec = new Vector3(_x , 0 , _z);
        _rb.velocity = speedVec.normalized * _speed;
    }
}
