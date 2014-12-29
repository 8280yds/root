using UnityEngine;
using System.Collections;

/// <summary>
/// 通用型物品类
/// </summary>
public class Goods : BaseObject<GoodsDBVO>
{
    /// <summary>
    /// 物品持续的时间
    /// </summary>
    private long tick;

    /// <summary>
    /// 物品类型的枚举
    /// </summary>
    public enum GoodsType
    {
        A = 1,
        B = 2,
        C = 3,
        S = 4,

        P = 100,
        O = 200,
    }

    /// <summary>
    /// 数据VO
    /// </summary>
    override public GoodsDBVO vo
    {
        set
        {
            base.vo = value;

            tf.renderer.material = ObjectManager.instance.getMaterial(vo.materialName);
            tick = System.DateTime.Now.Ticks;
        }
    }

    //========================================================

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
        tf.Translate(new Vector3(0, 0, -vo.speed * Time.deltaTime));

        if (System.DateTime.Now.Ticks - tick < vo.liftTime*1000*10000)
        {
            Vector3 v3 = tf.rotation.eulerAngles;
            if (tf.position.x > GameConstant.STAGE_WIDTH || tf.position.x < -GameConstant.STAGE_WIDTH)
            {
                v3.y = -v3.y;
            }
            if (tf.position.z > GameConstant.STAGE_HEIGHT || tf.position.z < -GameConstant.STAGE_HEIGHT)
            {
                v3.y = 180 - v3.y;
            }
            tf.rotation = Quaternion.Euler(v3);
        }
	}
	
	void OnTriggerEnter(Collider other)
	{
        if (other.transform == ObjectManager.instance.playerTF)
		{
            GameData.instance.score += vo.score;
			ObjectManager.instance.destroyObject(gameObject);
		}
	}

}

