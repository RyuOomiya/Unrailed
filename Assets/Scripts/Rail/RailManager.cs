using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RailManager : MonoBehaviour
{
    //�V���O���g���ɂ��邽�߂̏���
    private static RailManager _instance;
    public static RailManager Instance { get => _instance; }
    //RailSet�N���X���Q�Ƃ��邽�߂̏�����
    [SerializeField, Header("HintRail�N���X�����Ă�I�u�W�F�N�g")]
    GameObject _hintRailObj;
    [Tooltip("HintRail�X�N���v�g")] HintRail _hintRailScript;
    //���[���֌W��
    [Tooltip("���X�g�̗v�f���̕ێ�")]private int _railsCount;
    [SerializeField,Header("�ݒu���ꂽ���[���B")]
    public List<Rail> _rails = new List<Rail>();
    
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
        //�ŏ��Ƀ��[���̐���������
        _railsCount = _rails.Count;
        //_railSetObj����RailSet�X�N���v�g�����o��
        _hintRailScript = _hintRailObj.GetComponent<HintRail>();
    }

    void Update()
    {
        SetPointSummon();
        ForSetRail();
    }

    /// <summary>
    /// ���X�g�̍Ō���̃��[�����ς�邽�т�Seach�pPoint���΂�
    /// </summary>
    void SetPointSummon()
    {
        if(_railsCount != _rails.Count)
        {
            //�㉺���E��Seach�pPoint���΂�
            _hintRailScript._railSetManager.transform.position =
                new Vector3(_rails[_rails.Count - 1].transform.position.x , 0 , _rails[_rails.Count - 1].transform.position.z);
            //���X�g�̗v�f�����X�V
            _railsCount = _rails.Count;
        }
    }

    /// <summary>
    /// ���X�g�̍Ō�ȊO��Rail��enum�^�C�v��NotItem�ɂ���
    /// </summary>
    void ForSetRail()
    {
        foreach(var rail in _rails)
        {
            //rail == _rails[_rails.Count - 1]? rail.Set(ItemType.NotItem) : rail.Set(ItemType.Rail);
            if (rail != _rails[_rails.Count - 1])
            {
                rail.Set(ItemType.NotItem);
            }
            
            if(rail == _rails[_rails.Count - 1])
            {
                rail.Set(ItemType.Rail);
            }
        }
            
    }
}
