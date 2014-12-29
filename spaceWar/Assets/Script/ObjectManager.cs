using UnityEngine;
using System.Collections;

/// <summary>
/// 物体管理类
/// </summary>
public class ObjectManager
{
    /// <summary>
    /// 存储物体本体的哈希表
    /// </summary>
	Hashtable m_objectHash;
    /// <summary>
    /// 存数材质的哈希表
    /// </summary>
    Hashtable m_materialHash;
    /// <summary>
    /// 存储声音的哈希表
    /// </summary>
    Hashtable m_musicHash;

    /// <summary>
    /// 主角的transform
    /// </summary>
    private Transform m_playerTF;
    /// <summary>
    /// 敌人列表
    /// </summary>
    private Hashtable m_enemyHash;

	//===========================================
	static ObjectManager m_instance;
	
    /// <summary>
    /// 获取单例
    /// </summary>
	static public ObjectManager instance
	{
		get
		{
			if(m_instance == null)
			{
                m_instance = new ObjectManager();
			}
			return m_instance;
		}
	}
	
	public ObjectManager()
	{
		init();
	}
	
    /// <summary>
    /// 初始化
    /// </summary>
	void init()
	{
		m_objectHash = new Hashtable();
        m_materialHash = new Hashtable();
        m_musicHash = new Hashtable();
        m_enemyHash = new Hashtable();
	}

    //===========================================================
    /// <summary>
    /// 主角的transform
    /// </summary>
    public Transform playerTF
    {
        get
        {
            return m_playerTF;
        }
        set
        {
            m_playerTF = value;
        }
    }

    /// <summary>
    /// 敌人列表
    /// </summary>
    public Hashtable enemyHash
    {
        get
        {
            return m_enemyHash;
        }
    }
	
	//===========================================================
    /// <summary>
    /// 创建物体的克隆体
    /// </summary>
    /// <typeparam name="TVO">物体的数据VO</typeparam>
    /// <typeparam name="TObject">物体类名</typeparam>
    /// <param name="prefabName">物体的prefab名称</param>
    /// <param name="position">物体出现的位置</param>
    /// <param name="rotation">物体的旋转角度</param>
    /// <param name="vo">物体的数据VO</param>
    /// <returns>物体克隆的GameObject</returns>
    public GameObject creatObject<TVO, TObject>(string prefabName, Vector3 position, Quaternion rotation, TVO vo)
        where TObject : BaseObject<TVO>
	{
        GameObject gameObj = GameObject.Instantiate(getPrefab(prefabName), position, rotation) as GameObject;
		if(gameObj != null && vo != null)
		{
            TObject obj = gameObj.GetComponent<TObject>();
			if(obj != null)
			{
				obj.vo = vo;

                if (obj is Body)
                {
                    if (obj is Player)
                    {
                        playerTF = gameObj.transform;
                    }
                    else
                    {
                        addEnemyToHash(gameObj, obj as Body);
                    }
                }
			}
		}
		return gameObj;
	}

    /// <summary>
    /// 创建物体的克隆体
    /// </summary>
    /// <typeparam name="TVO">物体的数据VO</typeparam>
    /// <typeparam name="TObject">物体类名</typeparam>
    /// <param name="prefabName">物体的prefab名称</param>
    /// <param name="vo">物体的数据VO</param>
    /// <returns>物体克隆的GameObject</returns>
    public GameObject creatObject<TVO, TObject>(string prefabName, TVO vo)
    where TObject : BaseObject<TVO>
    {
        GameObject gameObj = GameObject.Instantiate(getPrefab(prefabName)) as GameObject;
        if (gameObj != null && vo != null)
        {
            TObject obj = gameObj.GetComponent<TObject>();
            if (obj != null)
            {
                obj.vo = vo;

                if (obj is Body)
                {
                    if (obj is Player)
                    {
                        playerTF = gameObj.transform;
                    }
                    else
                    {
                        addEnemyToHash(gameObj, obj as Body);
                    }
                }
            }
        }
        return gameObj;
    }

    /// <summary>
    /// 消除物体
    /// </summary>
    /// <param name="gameObj">物体</param>
    public void destroyObject(GameObject gameObj)
    {
        Body body = gameObj.GetComponent<Body>();
        if (body != null)
        {
            if (body is Player)
            {
                playerTF = null;
            }
            else
            {
                removeEnemyFromHash(gameObj);
            }
        }
        GameObject.Destroy(gameObj);
    }
	
    /// <summary>
    /// 获取物体的prefab本体
    /// </summary>
    /// <param name="prefabName">物体的prefab名称</param>
    /// <returns>物体本体的GameObject</returns>
	public GameObject getPrefab(string prefabName)
	{
		if(!m_objectHash.ContainsKey(prefabName))
		{
			m_objectHash[prefabName] = Resources.Load("Prefab/"+prefabName);
		}
        if (!(m_objectHash[prefabName] is GameObject))
        {
            Debug.Log("【提示】Load失败，未能成功加载Prefab/" + prefabName);
        }
        return m_objectHash[prefabName] as GameObject;
	}

    /// <summary>
    /// 获取材质
    /// </summary>
    /// <param name="materialName">材质文件的名称</param>
    /// <returns>材质</returns>
    public Material getMaterial(string materialName)
    {
        if (!m_materialHash.ContainsKey(materialName))
        {
            m_materialHash[materialName] = Resources.Load("Material/" + materialName);
        }
        if (!(m_materialHash[materialName] is Material))
        {
            Debug.Log("【提示】Load失败，未能成功加载Material/" + materialName);
        }
        return m_materialHash[materialName] as Material;
    }

    /// <summary>
    /// 获取声音
    /// </summary>
    /// <param name="musicName">声音文件的名称</param>
    /// <returns>声音的AudioClip</returns>
    public AudioClip getMusic(string musicName)
    {
        if (!m_musicHash.ContainsKey(musicName))
        {
            m_musicHash[musicName] = Resources.Load("Music/" + musicName);
        }
        if (!(m_musicHash[musicName] is AudioClip))
        {
            Debug.Log("【提示】Load失败，未能成功加载Music/" + musicName);
        }
        return m_musicHash[musicName] as AudioClip;
    }

    //=================================================================================
    /// <summary>
    /// 添加一个敌人到列表
    /// </summary>
    public void addEnemyToHash(GameObject gameObj, Body body)
    {
        m_enemyHash.Add(gameObj, body);
    }

    /// <summary>
    /// 从列表移除一个敌人
    /// </summary>
    /// <param name="gameObj"></param>
    public void removeEnemyFromHash(GameObject gameObj)
    {
        m_enemyHash.Remove(gameObj);
    }

    /// <summary>
    /// 摧毁所有敌人
    /// </summary>
    public void killsAllEnemy()
    {
        Body[] bodyArr = new Body[m_enemyHash.Count];
        m_enemyHash.Values.CopyTo(bodyArr,0);

        foreach (Body body in bodyArr)
        {
            body.life = 0;
        }
    }

}

