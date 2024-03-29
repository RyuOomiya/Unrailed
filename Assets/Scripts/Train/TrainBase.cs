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

    [SerializeField, Tooltip("回転中かどうか")] public bool _isRotate = false;
    [Tooltip("左に回転")] bool _isRotateL = false;
    [Tooltip("右に回転")] bool _isRotateR = false;
    
    [Tooltip("列車のRigidbody")] Rigidbody _rb;
    [Tooltip("列車のPosition")] Vector3 _trainPos;
    Quaternion _trainRot;
    
    
    [Tooltip("Ｙ軸を基準とした列車の左")]　float _nextQuaternionL;
    [Tooltip("Ｙ軸を基準とした列車の右")] float _nextQuaternionR;
    
    //列車の前と左右にRay
    [Header("Raycast")]
    [SerializeField, Tooltip("左Raycast")] Transform _leftR;
    [SerializeField, Tooltip("右Raycast")] Transform _rightR;
    [SerializeField, Tooltip("前Raycast")] Transform _frontR;
    [Tooltip("Raycastを飛ばす座標")] Vector3 _rayPos;

    [SerializeField] TrainDestroy _td;
    void Update()
    {
        TrainMove();

        //回転中でないならRayを飛ばして次のレールを判定
        if (!_isRotate)
        {
            TrainRaycast();
            _trainPos = gameObject.transform.position;
            _trainRot = gameObject.transform.rotation;

            //列車が回転しきったら位置を修正して、列車のｙ軸の数値を更新する
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
        if (Physics.Raycast(_trainPos, rayVec, out hit, 1))
        {
            //エラー吐かないように_nowRailIndexがリストの要素数 -1まで来たら処理をとばす
            if (RailManager.Instance._rails.Count - 2 >= _nowRailIndex)
            {
                //対象がレール&&次のindexのレールだったら
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
    /// 右に曲がる
    /// </summary>
    void RotateRight()
    {
        //曲がり切ったか判定
        if (_step < 1)  //曲がり切ってない
        {
            _moveSpeed = 0.02f;
            _isRotate = true;
            _step += _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_trainRot, Quaternion.Euler(0, _nextQuaternionR, 0), _step);

        }
        if (_step >= 1) //曲がり切った
        {
            //初期化
            _step = 0f;
            _trainRot = transform.rotation;
            _moveSpeed = 0.07f;
            _isRotate = false;
            _isRotateR = false;
            //_nowRailIndex++;
        }
    }

    /// <summary>
    /// 左に曲がる
    /// </summary>
    void RotateLeft()
    {
        if (_step < 1)  //曲がり切ってない
        {
            _moveSpeed = 0.02f;
            _isRotate = true;
            _step += _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_trainRot, Quaternion.Euler(0, _nextQuaternionL, 0), _step);

        }
        if (_step >= 1) //曲がり切った
        {
            //初期化
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
