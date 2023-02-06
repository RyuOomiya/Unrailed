using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FreightTrain : MonoBehaviour
{
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

    void Update()
    {
        ListCheck();
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
            PointManager.Instance._hitItems.Remove(_woods[0]);
            PointManager.Instance._hitItems.Remove(_stones[0]);
            _woods.Remove(_woods[0]);
            _stones.Remove(_stones[0]);
            Destroy(_destroyWood);
            Destroy(_destroyStone);
            _instanceRail = true;
        }
    }
}








