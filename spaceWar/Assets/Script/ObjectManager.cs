using UnityEngine;
using System.Collections;

/// <summary>
/// ���������
/// </summary>
public class ObjectManager
{
    /// <summary>
    /// �洢���屾��Ĺ�ϣ��
    /// </summary>
	Hashtable m_objectHash;
    /// <summary>
    /// �������ʵĹ�ϣ��
    /// </summary>
    Hashtable m_materialHash;
    /// <summary>
    /// �洢�����Ĺ�ϣ��
    /// </summary>
    Hashtable m_musicHash;

    /// <summary>
    /// ���ǵ�transform
    /// </summary>
    private Transform m_playerTF;
    /// <summary>
    /// �����б�
    /// </summary>
    private Hashtable m_enemyHash;

	//===========================================
	static ObjectManager m_instance;
	
    /// <summary>
    /// ��ȡ����
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
    /// ��ʼ��
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
    /// ���ǵ�transform
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
    /// �����б�
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
    /// ��������Ŀ�¡��
    /// </summary>
    /// <typeparam name="TVO">���������VO</typeparam>
    /// <typeparam name="TObject">��������</typeparam>
    /// <param name="prefabName">�����prefab����</param>
    /// <param name="position">������ֵ�λ��</param>
    /// <param name="rotation">�������ת�Ƕ�</param>
    /// <param name="vo">���������VO</param>
    /// <returns>�����¡��GameObject</returns>
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
    /// ��������Ŀ�¡��
    /// </summary>
    /// <typeparam name="TVO">���������VO</typeparam>
    /// <typeparam name="TObject">��������</typeparam>
    /// <param name="prefabName">�����prefab����</param>
    /// <param name="vo">���������VO</param>
    /// <returns>�����¡��GameObject</returns>
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
    /// ��������
    /// </summary>
    /// <param name="gameObj">����</param>
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
    /// ��ȡ�����prefab����
    /// </summary>
    /// <param name="prefabName">�����prefab����</param>
    /// <returns>���屾���GameObject</returns>
	public GameObject getPrefab(string prefabName)
	{
		if(!m_objectHash.ContainsKey(prefabName))
		{
			m_objectHash[prefabName] = Resources.Load("Prefab/"+prefabName);
		}
        if (!(m_objectHash[prefabName] is GameObject))
        {
            Debug.Log("����ʾ��Loadʧ�ܣ�δ�ܳɹ�����Prefab/" + prefabName);
        }
        return m_objectHash[prefabName] as GameObject;
	}

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="materialName">�����ļ�������</param>
    /// <returns>����</returns>
    public Material getMaterial(string materialName)
    {
        if (!m_materialHash.ContainsKey(materialName))
        {
            m_materialHash[materialName] = Resources.Load("Material/" + materialName);
        }
        if (!(m_materialHash[materialName] is Material))
        {
            Debug.Log("����ʾ��Loadʧ�ܣ�δ�ܳɹ�����Material/" + materialName);
        }
        return m_materialHash[materialName] as Material;
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="musicName">�����ļ�������</param>
    /// <returns>������AudioClip</returns>
    public AudioClip getMusic(string musicName)
    {
        if (!m_musicHash.ContainsKey(musicName))
        {
            m_musicHash[musicName] = Resources.Load("Music/" + musicName);
        }
        if (!(m_musicHash[musicName] is AudioClip))
        {
            Debug.Log("����ʾ��Loadʧ�ܣ�δ�ܳɹ�����Music/" + musicName);
        }
        return m_musicHash[musicName] as AudioClip;
    }

    //=================================================================================
    /// <summary>
    /// ���һ�����˵��б�
    /// </summary>
    public void addEnemyToHash(GameObject gameObj, Body body)
    {
        m_enemyHash.Add(gameObj, body);
    }

    /// <summary>
    /// ���б��Ƴ�һ������
    /// </summary>
    /// <param name="gameObj"></param>
    public void removeEnemyFromHash(GameObject gameObj)
    {
        m_enemyHash.Remove(gameObj);
    }

    /// <summary>
    /// �ݻ����е���
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

