using UnityEngine;
using System.Collections;

public class Player : Body
{
    /// <summary>
    /// 火力类型
    /// </summary>
    private Goods.GoodsType m_shellType = Goods.GoodsType.A;
	
	//========================================================
	/// <summary>
	/// 生命
	/// </summary>
	override public int life
	{
		set
		{
			base.life = value;

            GameManager.instance.changePlayerLife(value);
		}
	}

    /// <summary>
    /// 火力类型
    /// </summary>
    public Goods.GoodsType shellType
    {
        get
        {
            return m_shellType;
        }
        set
        {
            m_shellType = value;
        }
    }
	
	//========================================================
	void Update ()
	{
		move();
		fire();
	}
	
    /// <summary>
    /// 移动
    /// </summary>
	override protected void move()
	{
		float dx = 0;
		float dz = 0;
		float d = vo.speed * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.UpArrow))
		{
			dz = d; 
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			dz -= d; 
		}	
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			dx -= d; 
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			dx += d; 
		}

        Vector3 v3 = tf.position + new Vector3(dx, 0, dz);
        if (v3.x > GameConstant.STAGE_WIDTH - tf.renderer.bounds.size.x/2)
        {
            v3.x = GameConstant.STAGE_WIDTH - tf.renderer.bounds.size.x/2;
        }
        else if (v3.x < -GameConstant.STAGE_WIDTH + tf.renderer.bounds.size.x/2)
        {
            v3.x = -GameConstant.STAGE_WIDTH + tf.renderer.bounds.size.x/2;
        }
        if (v3.z > GameConstant.STAGE_HEIGHT - tf.renderer.bounds.size.z/2)
        {
            v3.z = GameConstant.STAGE_HEIGHT - tf.renderer.bounds.size.z/2;
        }
        else if (v3.z < -GameConstant.STAGE_HEIGHT + tf.renderer.bounds.size.z/2)
        {
            v3.z = -GameConstant.STAGE_HEIGHT + tf.renderer.bounds.size.z/2;
        }
        tf.position = v3;
	}

    /// <summary>
    /// 发射子弹
    /// </summary>
    override protected void onFire()
	{
        base.onFire();
		
		if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
		{
            audioSource.PlayOneShot(ObjectManager.instance.getMusic(vo.fireSound));
            switch (shellType)
			{
				case Goods.GoodsType.A:
					OnFireTypeA();
					break;
				case Goods.GoodsType.B:
					OnFireTypeB();
					break;
				case Goods.GoodsType.C:
					OnFireTypeC();
					break;
				case Goods.GoodsType.S:
					OnFireTypeS();
					break;
				default:
					OnFireTypeA();
					break;
			}
		}
	}

    /// <summary>
    /// 火力A的实现
    /// </summary>
	void OnFireTypeA()
	{
		Vector3 position = tf.position;
        ShellDBVO shellDBVO = ShellDBManager.instance.getVOByID(vo.shellId);
        ObjectManager.instance.creatObject<ShellDBVO, Shell>(shellDBVO.prefabName, new Vector3(position.x, position.y, position.z + 0.5f), tf.rotation, shellDBVO);
	}

    /// <summary>
    /// 火力B的实现
    /// </summary>
	void OnFireTypeB()
	{
		Vector3 position = tf.position;
        ShellDBVO shellDBVO = ShellDBManager.instance.getVOByID(vo.shellId);
        ObjectManager.instance.creatObject<ShellDBVO, Shell>(shellDBVO.prefabName, new Vector3(position.x - 0.2f, position.y, position.z + 0.5f), tf.rotation, shellDBVO);
        ObjectManager.instance.creatObject<ShellDBVO, Shell>(shellDBVO.prefabName, new Vector3(position.x + 0.2f, position.y, position.z + 0.5f), tf.rotation, shellDBVO);
	}

    /// <summary>
    /// 火力C的实现
    /// </summary>
	void OnFireTypeC()
	{
		OnFireTypeA();
		OnFireTypeB();
		Vector3 position = tf.position;
        ShellDBVO shellDBVO = ShellDBManager.instance.getVOByID(vo.shellId);
        ObjectManager.instance.creatObject<ShellDBVO, Shell>(shellDBVO.prefabName, new Vector3(position.x - 0.4f, position.y, position.z + 0.5f), tf.rotation, shellDBVO);
        ObjectManager.instance.creatObject<ShellDBVO, Shell>(shellDBVO.prefabName, new Vector3(position.x + 0.4f, position.y, position.z + 0.5f), tf.rotation, shellDBVO);
	}

    /// <summary>
    /// 火力S的实现
    /// </summary>
	void OnFireTypeS()
	{
		OnFireTypeA();
		AddOnFireTypeRector(120);
		AddOnFireTypeRector(140);
		AddOnFireTypeRector(160);
		AddOnFireTypeRector(200);
		AddOnFireTypeRector(220);
		AddOnFireTypeRector(240);
	}
	void AddOnFireTypeRector(int angle)
	{
		Quaternion rotation = new Quaternion();
		rotation.eulerAngles = new Vector3(0, angle, 0);
        ShellDBVO shellDBVO = ShellDBManager.instance.getVOByID(vo.shellId);
        ObjectManager.instance.creatObject<ShellDBVO, Shell>(shellDBVO.prefabName, tf.position, rotation, shellDBVO);
	}

    void OnTriggerEnter(Collider other)
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

        Goods goods = other.GetComponent<Goods>();
        if (goods)
        {
            audioSource.PlayOneShot(ObjectManager.instance.getMusic(goods.vo.musicName));
            Goods.GoodsType type = (Goods.GoodsType)goods.vo.type;

            switch(type)
            {
                case Goods.GoodsType.P:
                    life++;
                    break;
                default:
                    shellType = (Goods.GoodsType)goods.vo.type;
                    break;
            }
        }
    }
}

