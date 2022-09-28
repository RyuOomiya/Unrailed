using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxe : MonoBehaviour , IPickableItem
{
    [Tooltip("���Ԋu�ŉ󂷂��߂̃^�C�}�[")] float _timer = 0;
    [Tooltip("�^�C�}�[�ƃZ�b�g�Ŏg��")] bool _isSwing;
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;

    public ItemType Type { get => _type; }
    void Update()
    {
        CoolTime();
    }

    /// <summary>
    /// ����󂷃C���^�[�o��
    /// </summary>
    void CoolTime()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _isSwing = false;
        }
    }
    public void Action(GameObject hitObj)
    {
        
        if (hitObj.TryGetComponent(out Rock rock) && !_isSwing)
        {
            Debug.Log("�h��");
            _isSwing = true;
            rock.RockLife -= 1;
            _timer = 1;
        }
    }
}

