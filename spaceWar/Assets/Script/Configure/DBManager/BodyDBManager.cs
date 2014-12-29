using UnityEngine;
using System.Collections;
using System.Xml;

/// <summary>
/// 物体数据表管理类
/// </summary>
public class BodyDBManager
{
	XmlDocument xmldoc;
	Hashtable hashtable;
	
	//===========================================
	static BodyDBManager m_instance;
	
    /// <summary>
    /// 获取实例
    /// </summary>
	static public BodyDBManager instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = new BodyDBManager();
			}
			return m_instance;
		}
	}
	
	public BodyDBManager()
	{
		init();
	}
	
    /// <summary>
    /// 初始化
    /// </summary>
	void init()
	{
		xmldoc = new XmlDocument();
		xmldoc.Load(Application.dataPath + "/Script/Configure/XML/body.xml");
		
		hashtable = new Hashtable();
	}
	
	//===========================================================
	/// <summary>
	/// 根据id获取一条数据
	/// </summary>
	/// <param name="id">id</param>
    /// <returns>BodyDBVO</returns>
	public BodyDBVO getVOByID(int id)
	{
		BodyDBVO vo = null;

        if(hashtable.ContainsKey(id))
		{
            vo = (BodyDBVO)hashtable[id];
		}else{
			XmlNode xmlNode = xmldoc.SelectSingleNode("data/item[@id='"+id+"']");
			
			if(xmlNode != null)
			{
				XmlElement xmlelement = (XmlElement)xmlNode;
				vo = new BodyDBVO();
				vo.id = int.Parse(xmlelement.GetAttribute("id"));
				vo.prefabName = xmlelement.GetAttribute("prefabName");
                vo.fireSound = xmlelement.GetAttribute("fireSound");
                vo.explosion = xmlelement.GetAttribute("explosion");
				vo.speed = float.Parse(xmlelement.GetAttribute("speed"));
				vo.power = int.Parse(xmlelement.GetAttribute("power"));
				vo.camp = byte.Parse(xmlelement.GetAttribute("camp"));
				vo.life = int.Parse(xmlelement.GetAttribute("life"));
				vo.score = int.Parse(xmlelement.GetAttribute("score"));
				vo.fireCD = float.Parse(xmlelement.GetAttribute("fireCD"));
				vo.shellId = int.Parse(xmlelement.GetAttribute("shellId"));
                vo.isToward = bool.Parse(xmlelement.GetAttribute("isToward"));
                vo.isTrack = bool.Parse(xmlelement.GetAttribute("isTrack"));
				vo.goodsID = xmlelement.GetAttribute("goodsID");

                hashtable.Add(vo.id, vo);
			}
		}
		return vo;
	}
	
}
