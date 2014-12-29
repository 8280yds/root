using UnityEngine;
using System.Collections;

public class BaseObject<TVO> : MonoBehaviour
{
	private Transform m_tf;
	private TVO m_vo;
	private int m_life;
	
	//========================================================
	/// <summary>
    /// 物体的Transform
	/// </summary>
	protected Transform tf
	{
		get
		{
            if (m_tf == null)
			{
                m_tf = this.transform;
			}
            return m_tf;
		}
	}
	
    /// <summary>
    /// 数据VO
    /// </summary>
	virtual public TVO vo
	{
		get
		{
			return m_vo;
		}
		set
		{
			m_vo = value;
		}
	}
	
    /// <summary>
    /// 生命值
    /// </summary>
	virtual public int life
	{
		get
		{
			return m_life;
		}
		set
		{
			m_life = value;
		}
	}
	
	//========================================================
	/// <summary>
	/// 移动
	/// </summary>
	virtual protected void move()
	{
        if (tf.position.x > GameConstant.STAGE_WIDTH + tf.renderer.bounds.size.x / 2 || tf.position.x < -GameConstant.STAGE_WIDTH - tf.renderer.bounds.size.x / 2 ||
            tf.position.z > GameConstant.STAGE_HEIGHT * 1.5f || tf.position.z < -GameConstant.STAGE_HEIGHT - tf.renderer.bounds.size.z / 2)
        {
            ObjectManager.instance.destroyObject(gameObject);
        }
	}
	
}
