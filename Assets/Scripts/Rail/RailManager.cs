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
    [SerializeField, Header("RailSet�N���X�����Ă�I�u�W�F�N�g")]
    GameObject _railSetObj;
    [Tooltip("RailSet�X�N���v�g")] RailSet _railSetScript;
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
        _railSetScript = _railSetObj.GetComponent<RailSet>();
    }

    void Update()
    {
        SetPointSummon();
    }

    void SetPointSummon()
    {
        if(_railsCount != _rails.Count)
        {
            //�㉺���E��Seach�pPoint���΂�
            _railSetScript.transform.position = _rails[_rails.Count - 1].transform.position;
            //���X�g�̗v�f�����X�V
            _railsCount = _rails.Count;
        }

    }
    public void AddRail(Rail rail)
    {
        _rails.Add(rail);
    }

}
