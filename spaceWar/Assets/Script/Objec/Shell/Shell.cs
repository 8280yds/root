using UnityEngine;
using System.Collections;

/// <summary>
/// 通用型子弹类
/// </summary>
public class Shell : BaseObject<ShellDBVO>
{
    /// <summary>
    /// 数据VO
    /// </summary>
	override public ShellDBVO vo
	{
		set
		{
			base.vo = value;
			life = value.life;
		}
	}
	
    /// <summary>
    /// 生命值
    /// </summary>
	override public int life
	{
		set
		{
			base.life = value;
			if(value <= 0)
			{
				ObjectManager.instance.destroyObject(this.gameObject);
			}
		}
	}
	
	//========================================================
    void Start()
    {
        if (vo.isToward)
        {
            Transform playerTF = ObjectManager.instance.playerTF;
            if (playerTF != null)
            {
                tf.rotation = Quaternion.LookRotation(tf.position - playerTF.position);
            }
        }
    }

    void Update()
    {
        move();
    }

    /// <summary>
    /// 移动
    /// </summary>
	override protected void move()
	{
        base.move();

        Transform playerTF = ObjectManager.instance.playerTF;
        if (vo.isTrack && playerTF != null)
        {
            tf.rotation = Quaternion.LookRotation(tf.position - playerTF.position);
        }
        tf.Translate(new Vector3(0, 0, -vo.speed * Time.deltaTime));
	}
	
	void OnTriggerEnter(Collider other)
	{
		Shell shell = other.GetComponent<Shell>();
		if(shell!=null && shell.vo.camp!=vo.camp && vo.canSmash && shell.vo.canSmash)
		{
			life -= shell.vo.power;
			return;
		}
		
		Body body = other.GetComponent<Body>();
		if(body!=null && body.vo.camp!=vo.camp)
		{
			life = 0;
			return;
		}
	}
}
