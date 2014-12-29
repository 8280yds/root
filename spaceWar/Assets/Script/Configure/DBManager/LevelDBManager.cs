using UnityEngine;
using System.Collections;
using System.Xml;

/// <summary>
/// 关卡管理类
/// </summary>
public class LevelDBManager
{
    XmlDocument xmldoc;
    Hashtable hashtable;

    //===========================================
    static LevelDBManager m_instance;

    /// <summary>
    /// 获取实例
    /// </summary>
    static public LevelDBManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new LevelDBManager();
            }
            return m_instance;
        }
    }

    public LevelDBManager()
    {
        init();
    }

    void init()
    {
        xmldoc = new XmlDocument();
        xmldoc.Load(Application.dataPath + "/Script/Configure/XML/level.xml");

        hashtable = new Hashtable();
    }

    //===========================================================
    /// <summary>
    /// 根据id获取一条数据
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>LevelDBVO</returns>
    public LevelDBVO getVOByID(int id)
    {
        LevelDBVO vo = null;

        if (hashtable.ContainsKey(id))
        {
            vo = (LevelDBVO)hashtable[id];
        }
        else
        {
            XmlNode xmlNode = xmldoc.SelectSingleNode("data/item[@id='" + id + "']");

            if (xmlNode != null)
            {
                XmlElement xmlelement = (XmlElement)xmlNode;
                vo = new LevelDBVO();
                vo.id = int.Parse(xmlelement.GetAttribute("id"));
                vo.enemy = xmlelement.GetAttribute("enemy");
                vo.probability = xmlelement.GetAttribute("probability");
                vo.creatCD = xmlelement.GetAttribute("creatCD");
                vo.delay = int.Parse(xmlelement.GetAttribute("delay"));
                vo.score = int.Parse(xmlelement.GetAttribute("score"));
                vo.kills = int.Parse(xmlelement.GetAttribute("kills"));

                hashtable.Add(vo.id, vo);
            }
        }
        return vo;
    }

}
