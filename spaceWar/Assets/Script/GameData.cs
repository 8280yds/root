using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏数据存储类
/// </summary>
public class GameData
{
    private int m_score = 0;
    private int m_histroy = 0;
    private int m_level = 0;

    //===========================================
    static GameData m_instance;

    /// <summary>
    /// 获取单例
    /// </summary>
    public static GameData instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameData();
            }
            return m_instance;
        }
    }

    public GameData()
    {
        init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void init()
    {
        
    }

    //===============================================================
    /// <summary>
    /// 当前分数
    /// </summary>
    public int score
    {
        get
        {
            return m_score;
        }
        set
        {
            m_score = value;
            if (score > histroy)
            {
                histroy = score;
            }
        }
    }

    /// <summary>
    /// 历史最高分数
    /// </summary>
    public int histroy
    {
        get
        {
            return m_histroy;
        }
        set
        {
            m_histroy = value;
        }
    }

    /// <summary>
    /// 当前关卡
    /// </summary>
    public int level
    {
        get
        {
            return m_level;
        }
        set
        {
            m_level = value;
        }
    }

}