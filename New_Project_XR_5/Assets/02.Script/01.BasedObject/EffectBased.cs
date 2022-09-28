using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBased : MonoBehaviour
{
    [System.Serializable]
    public struct EffectInfo
    {
        public float fYpos;//ĳ���� �� ����Ʈ �߻� ��ġ���� y�� �󸶳� �ű���
        public float fXpos;//���� ���� x
        public float fFirstDelay;//����
        public float fTime;//����Ʈ ���ӽð� ���� 5�� �θ� �ɵ�
        public bool bIsRotation;//ĳ������ ȸ�����͸�ŭ ����Ʈ�� ������ ����

        public GameObject EffectObj;//����Ʈ ������
        
    };
    public List<EffectInfo> CEffect;

    protected List<Queue<GameObject>> C_EffectPool;//��ųʸ� ������ƮǮ �̱��� ��ũ��Ʈ�� ���� ����

    public virtual void Start()
    {
        C_EffectPool = new List<Queue<GameObject>>();

        for (int i = 0; i < CEffect.Count; i++)
        {
            Queue<GameObject> _queue = new Queue<GameObject>();

            if (CEffect[i].EffectObj.GetComponent<ParticleSystem>().loop)
            {
                CEffect[i].EffectObj.SetActive(false);
            }
            else
            {
                GameObject obj = Instantiate(CEffect[i].EffectObj);

                obj.transform.parent = transform;
                obj.SetActive(false);
                _queue.Enqueue(obj);
            }

            C_EffectPool.Add(_queue);
        }
    }

    public virtual void OnEffect(int i, Transform _transform)//�ݺ��Ǵ� ���� ����Ʈ Ű�� ��
    {
        CEffect[i].EffectObj.SetActive(true);
    }
    public virtual void OfffEffect(int i)//�ݺ��Ǵ� ���� ����Ʈ ���� ��
    {
        CEffect[i].EffectObj.SetActive(false);
    }

    public virtual IEnumerator TargetEffect(int i, Transform _transform)
    {
        if (C_EffectPool[i].Count == 0)
        {
            GameObject obj2 = Instantiate(CEffect[i].EffectObj);
            obj2.transform.parent = _transform;
            C_EffectPool[i].Enqueue(obj2);
        }

        GameObject obj;
        obj = C_EffectPool[i].Dequeue();


        yield return new WaitForSeconds(CEffect[i].fFirstDelay);

        obj.SetActive(true);
        obj.transform.position = new Vector3
            (_transform.position.x + CEffect[i].fXpos,
            _transform.position.y + CEffect[i].fYpos,
            _transform.position.z);

        if (CEffect[i].bIsRotation)
            obj.transform.rotation = _transform.rotation;

        obj.GetComponent<ParticleSystem>().Play();


        yield return new WaitForSeconds(CEffect[i].fTime);

        //yield return new WaitForSeconds(obj.GetComponent<ParticleSystem>().duration);
        obj.SetActive(false);

        C_EffectPool[i].Enqueue(obj);
    }
}
