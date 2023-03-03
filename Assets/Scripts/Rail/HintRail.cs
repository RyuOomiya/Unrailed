using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRail : MonoBehaviour, IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
    [SerializeField]bool _canSet = true;
    public bool CanSet  {get => _canSet;}
    [SerializeField, Header("RailSetPoint�̐e�I�u�W�F�N�g")]public GameObject _railSetManager;
    MeshRenderer _railRenderer;
    [SerializeField , Tooltip("�S�[�������H")]public static bool _isGoal;
   
    private void Start()
    {
        _railRenderer = GetComponent<MeshRenderer>();
        _railRenderer.enabled = false;
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

            SetPointSeach(other.gameObject);
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

    public void ChangeSetActive(bool enabled)
    {
        //�v���C���[���G��Ă���Ƃ��͕\�����āA���ꂽ��\�����Ȃ�
        gameObject.GetComponent<MeshRenderer>().enabled = enabled;
    }


    /// <summary>
    /// ���[����ݒu�ł���|�C���g��T��
    /// </summary>
    public void SetPointSeach(GameObject hitObj)
    {
        //hitObj��enum�̃^�C�v��NotItem��������hintRail�͕\�����Ȃ�
        if (hitObj.TryGetComponent(out IPickableItem pickable))
        {
            if (pickable.GetType() == ItemType.NotItem)
            {
                _canSet = false;
            }   
        }
        else
        {
            _canSet = true;
        }
        if (hitObj.gameObject.name == "GoalRail")
        {
            _isGoal = true;
        }
    }

    public void Action(GameObject hitObj)
    {

    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}
