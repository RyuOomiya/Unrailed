using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRail : MonoBehaviour
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
    [SerializeField] bool _canSet = true;
    public bool CanSet { get => _canSet; }
    [SerializeField, Header("RailSetPoint�̐e�I�u�W�F�N�g")] public GameObject _railSetManager;
    MeshRenderer _railRenderer;
    Vector3 _overLapPos;
    [SerializeField, Tooltip("�S�[�������H")] public bool _isGoal;
    [SerializeField] PointManager _pointManager;
    [SerializeField] Rail rail;
    bool _isHitNotGround = false;

    private void Start()
    {
        _railRenderer = GetComponent<MeshRenderer>();
        _railRenderer.enabled = false;
        _overLapPos = this.transform.position;
    }
    void OnTriggerEnter(Collider other)
    {
        //�v���C���[���A�C�e���������ĂāA���̃A�C�e�������[����������n�C���C�g�I��
        if (other.gameObject.TryGetComponent(out PointManager pm))
        {
            if (pm.HasObj && pm.PickedType == ItemType.Rail && _canSet)
            {
                ChangeSetActive(true);
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        //���ꂽ�Ƃ��n�C���C�g�I�t
        if (other.gameObject.TryGetComponent(out PointManager pm))
        {
            if (pm.HasObj && pm.PickedType == ItemType.Rail)
            {
                ChangeSetActive(false);
            }
        }
    }

    void Update()
    {
        SetPointSeach();
    }
    public void ChangeSetActive(bool enabled)
    {
        //�v���C���[���G��Ă���Ƃ��͕\�����āA���ꂽ��\�����Ȃ�
        gameObject.GetComponent<MeshRenderer>().enabled = enabled;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(_canSet)
        Gizmos.DrawSphere(new Vector3(_overLapPos.x, _overLapPos.y + 0.5f, _overLapPos.z), 0.3f);
    }
    /// <summary>
    /// ���[����ݒu�ł���|�C���g��T��
    /// </summary>
    public void SetPointSeach()
    {
        _isHitNotGround = false;
        _overLapPos = transform.position;
        foreach(Collider hitObj in Physics.OverlapSphere(new Vector3(_overLapPos.x, _overLapPos.y + 0.5f, _overLapPos.z),0.3f))
        {
            if (!hitObj.name.Contains("Ground"))
            {
                if (hitObj.TryGetComponent(out PlayerMove pm)
                || hitObj.TryGetComponent(out TrainBase tb)
                || (hitObj.TryGetComponent(out IPickableItem pickable)
                            && pickable.GetType() == ItemType.NotItem))
                {
                    Debug.Log("q");
                    _canSet = false;
                    _isHitNotGround = true;
                }
            }
            else
            {
                if(_isHitNotGround)
                {

                }
                else
                {
                    _canSet = true;
                }
            }

            if (hitObj.TryGetComponent(out Rail rail)
                && !_pointManager._isHave
                    && !RailManager.Instance._rails.Contains(rail))
            {
                //���̃��[�����ݒu�ς݂̃��[���łȂ���Δz��ɒǉ�����
                Debug.Log("�Ă΂ꂽ�炵��");
                RailManager.Instance._rails.Add(rail);
                rail._railColor.material = rail._installedRailMaterial; //�ݒu�������[���̃}�e���A���𒣂�ւ���
                ChangeSetActive(false);
                rail._isMove = true;
            }

            //�S�[����������
            if (hitObj.gameObject.name == "GoalRail")
            {
                _isGoal = true;
            }
        }
        

    }

    //public void Action(GameObject hitObj)
    //{

    //}

    //ItemType IPickableItem.GetType()
    //{
    //    return _type;
    //}
}
