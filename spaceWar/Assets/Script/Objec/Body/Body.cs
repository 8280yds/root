using UnityEngine;
using System.Collections;

/// <summary>
/// ����Ļ��ࣨ�����������Ҫ�̳б���ʵ�֣�
/// </summary>
public class Body : BaseObject<BodyDBVO>
{
    /// <summary>
    /// �����ӵ���CD
    /// </summary>
	protected float m_fireDelay = 0.0f;
	
    /// <summary>
    /// ����������
    /// </summary>
    private AudioSource m_audioSource;
	
	//========================================================
    /// <summary>
    /// ����������
    /// </summary>
    protected AudioSource audioSource
	{
		get
		{
            if (m_audioSource == null)
			{
                m_audioSource = this.audio;
			}
            return m_audioSource;
		}
	}
	
    /// <summary>
    /// ����VO
    /// </summary>
	override public BodyDBVO vo
	{
		set
		{
			base.vo = value;
			life = value.life;
		}
	}
	
    /// <summary>
    /// ����
    /// </summary>
	override public int life
	{
		set
		{
			base.life = value;
			if(value <= 0)
			{
				Instantiate(ObjectManager.instance.getPrefab(vo.explosion), tf.position, Quaternion.identity);

                if(vo.goodsID != "")
                {
                    int num = Random.Range(0, 100);
                    string[] s = vo.goodsID.Split(new char[] { ',' });

                    for (int i = 0; i < s.Length; i++ )
                    {
                        string[] s2 = s[i].Split(new char[] { '-' });

                        if (num <= int.Parse(s2[1]))
                        {
                            GoodsDBVO goodsDBVO = GoodsDBManager.instance.getVOByID(int.Parse(s2[0]));
                            ObjectManager.instance.creatObject<GoodsDBVO, Goods>
                                (goodsDBVO.prefabName, tf.position, Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0)), goodsDBVO);
                            break;
                        }
                    }
                }

                GameData.instance.score += vo.score;
                ObjectManager.instance.destroyObject(gameObject);
			}
		}
	}
	
	//========================================================
	/// <summary>
	/// ����
	/// </summary>
	protected void fire()
	{
		if(vo.fireCD <= 0)
		{
			return;
		}
		
        if(vo.fireCD > 0)
        {
		    m_fireDelay -= Time.deltaTime;
		    if(m_fireDelay > 0)
		    {
			    return;
		    }
		    m_fireDelay += vo.fireCD;
            onFire();
        }
	}
    virtual protected void onFire()
    {

    }

    /// <summary>
    /// ��ײ
    /// </summary>
    /// <param name="other">��ײ������</param>
    virtual protected void triggerEnter(Collider other)
    {
        Shell shell = other.GetComponent<Shell>();
        if (shell != null && shell.vo.camp != vo.camp)
        {
            life -= shell.vo.power;
            return;
        }

        Body body = other.GetComponent<Body>();
        if (body != null && body.vo.camp != vo.camp)
        {
            life -= body.vo.power;
            return;
        }
    }

}

