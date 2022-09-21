using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSet : MonoBehaviour
{
    [SerializeField, Header("RailSetPoint�̐e�I�u�W�F�N�g")] GameObject _railSetManager;
    public GameObject RailSetManager { get => _railSetManager; }
    [Header("�n�C���C�g�\���p")] GameObject _hintRail;

    void Start()
    {
        _hintRail = transform.GetChild(0).gameObject;
    }

    void OnTriggerStay(Collider other)
    {
        SetPointSeach(other.gameObject);
    }
    /// <summary>
    /// ���[����ݒu�ł���|�C���g��T��
    /// </summary>
    public void SetPointSeach(GameObject hitObj)
    {
        //hitObj��enum�̃^�C�v��Resource��������hintRail�͕\�����Ȃ�
        if (hitObj.TryGetComponent(out IPickableItem pickable))
        {
            if(pickable.Type == ItemType.Resource)
            {
                _hintRail.gameObject.SetActive(false);
            }
        }
        //��̏����𔲂��ă��[������������
        else if (hitObj.TryGetComponent(out Rail rail))
        {
            //���̃��[�����ݒu�ς݂̃��[���łȂ���Δz��ɒǉ�����
            if (!RailManager.Instance._rails.Contains(rail))
            {
                Debug.Log("�Ă΂ꂽ�炵��");
                RailManager.Instance._rails.Add(rail);
            }
        }
    }
}
