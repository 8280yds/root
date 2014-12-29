using UnityEngine;
using System.Collections;
using System.Xml;

/// <summary>
/// 子弹数据表管理类
/// </summary>
public class ShellDBManager
{
	XmlDocument xmldoc;
	Hashtable hashtable;
	
	//===========================================
	static ShellDBManager m_instance;
	
    /// <summary>
    /// 获取实例
    /// </summary>
	static public ShellDBManager instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = new ShellDBManager();
			}
			return m_instance;
		}
	}
	
	public ShellDBManager()
	{
		init();
	}
	
    /// <summary>
    /// 初始化
    /// </summary>
	void init()
	{
		xmldoc = new XmlDocument();
        xmldoc.Load(Application.dataPath + "/Script/Configure/XML/shell.xml");
		
		hashtable = new Hashtable();
	}
	
	//===========================================================
	/// <summary>
	/// 根据id获取一条数据
	/// </summary>
	/// <param name="id">id</param>
    /// <returns>ShellDBVO</returns>
	public ShellDBVO getVOByID(int id)
	{
		ShellDBVO vo = null;
		
		if(hashtable.ContainsKey(id))
		{
            vo = (ShellDBVO)hashtable[id];
		}else{
			XmlNode xmlNode = xmldoc.SelectSingleNode("data/item[@id='"+id+"']");
			
			if(xmlNode != null)
			{
				XmlElement xmlelement = (XmlElement)xmlNode;
				vo = new ShellDBVO();
				vo.id = int.Parse(xmlelement.GetAttribute("id"));
				vo.prefabName = xmlelement.GetAttribute("prefabName");
				vo.speed = float.Parse(xmlelement.GetAttribute("speed"));
				vo.power = int.Parse(xmlelement.GetAttribute("power"));
				vo.camp = byte.Parse(xmlelement.GetAttribute("camp"));
				vo.life = int.Parse(xmlelement.GetAttribute("life"));
                vo.isToward = bool.Parse(xmlelement.GetAttribute("isToward"));
                vo.isTrack = bool.Parse(xmlelement.GetAttribute("isTrack"));
				vo.canSmash = bool.Parse(xmlelement.GetAttribute("canSmash"));

                hashtable.Add(vo.id, vo);
			}
		}
		return vo;
	}
	
}

