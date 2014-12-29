using UnityEngine;
using System.Collections;

public class Enemy : Body
{
	
	//=======================================================
	void Start ()
	{
	    if(vo.isToward)
        {
            Transform playerTF = ObjectManager.instance.playerTF;
            if (playerTF != null)
            {
                tf.rotation = Quaternion.LookRotation(tf.position - playerTF.position);
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		move();
		fire();
	}

    /// <summary>
    /// ÒÆ¶¯
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

    /// <summary>
    /// ·¢Éä×Óµ¯
    /// </summary>
    override protected void onFire()
	{
        base.onFire();

        ShellDBVO shellDBVO = ShellDBManager.instance.getVOByID(vo.shellId);
        ObjectManager.instance.creatObject<ShellDBVO, Shell>(shellDBVO.prefabName, tf.position, tf.rotation, shellDBVO);
        audioSource.PlayOneShot(ObjectManager.instance.getMusic(vo.fireSound));
	}

    void OnTriggerEnter(Collider other)
    {
        triggerEnter(other);
    }

}

