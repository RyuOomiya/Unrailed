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
    [Tooltip("ダッシュのクールタイム")] private float _coolTime;    
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
        //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる
        if (diff.magnitude > 0.05f)
        {
            //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
            transform.rotation = Quaternion.LookRotation(diff);
        }
        //プレイヤーの位置を更新
        _playerPos = transform.position;
        //スプリントのクールタイム
        _coolTime -= Time.deltaTime;
        //シフトを押したときにスプリントしてクールタイムをリセットする
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
