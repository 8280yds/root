using UnityEngine;
using System.Collections;
using System.Xml;

/// <summary>
/// ��Ʒ���ݱ������
/// </summary>
public class GoodsDBManager
{
	XmlDocument xmldoc;
	Hashtable hashtable;
	
	//===========================================
	static GoodsDBManager m_instance;
	
    /// <summary>
    /// ��ȡʵ��
    /// </summary>
	static public GoodsDBManager instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = new GoodsDBManager();
			}
			return m_instance;
		}
	}

    public GoodsDBManager()
	{
		init();
	}
	
    /// <summary>
    /// ��ʼ��
    /// </summary>
	void init()
	{
		xmldoc = new XmlDocument();
        xmldoc.Load(Application.dataPath + "/Script/Configure/XML/goods.xml");
		
		hashtable = new Hashtable();
	}
	
	//===========================================================
	/// <summary>
	/// ����id��ȡһ������
	/// </summary>
	/// <param name="id">id</param>
    /// <returns>GoodsDBVO</returns>
	public GoodsDBVO getVOByID(int id)
	{
		GoodsDBVO vo = null;

        if(hashtable.ContainsKey(id))
		{
            vo = (GoodsDBVO)hashtable[id];
		}else{
			XmlNode xmlNode = xmldoc.SelectSingleNode("data/item[@id='"+id+"']");
			
			if(xmlNode != null)
			{
				XmlElement xmlelement = (XmlElement)xmlNode;
				vo = new GoodsDBVO();
				vo.id = int.Parse(xmlelement.GetAttribute("id"));
				vo.prefabName = xmlelement.GetAttribute("prefabName");
				vo.speed = float.Parse(xmlelement.GetAttribute("speed"));
				vo.liftTime = float.Parse(xmlelement.GetAttribute("liftTime"));
				vo.score = int.Parse(xmlelement.GetAttribute("score"));
				vo.type = int.Parse(xmlelement.GetAttribute("type"));
                vo.materialName = xmlelement.GetAttribute("materialName");
                vo.musicName = xmlelement.GetAttribute("musicName");

                hashtable.Add(vo.id, vo);
			}
		}
		return vo;
	}
	
}