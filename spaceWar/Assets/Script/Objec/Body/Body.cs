using UnityEngine;
using System.Collections;

/// <summary>
/// 物体的基类（具体的物体需要继承本类实现）
/// </summary>
public class Body : BaseObject<BodyDBVO>
{
    /// <summary>
    /// 发射子弹的CD
    /// </summary>
	protected float m_fireDelay = 0.0f;
	
    /// <summary>
    /// 声音播放器
    /// </summary>
    private AudioSource m_audioSource;
	
	//========================================================
    /// <summary>
    /// 声音播放器
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
    /// 数据VO
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
    /// 生命
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
	/// 开火
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
    /// 碰撞
    /// </summary>
    /// <param name="other">碰撞的物体</param>
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

