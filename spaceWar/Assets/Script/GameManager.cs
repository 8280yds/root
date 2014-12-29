using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏管理类
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 右上角的飞机列表
    /// </summary>
    private ArrayList playerLifeList;

    float creatCD = 0;
    LevelDBVO levelVO;
    string[] enemyIdArr;
    string[] probabilityArr;
    string[] creatCDArr;

    //===========================================
    /// <summary>
    /// 获取实例
    /// </summary>
    static public GameManager instance;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 30;
        GameData.instance.score = 0;
        GameData.instance.level = 1;

        BodyDBVO bodyDBVO = BodyDBManager.instance.getVOByID(1001);
        GameObject player = ObjectManager.instance.creatObject<BodyDBVO, Player>(bodyDBVO.prefabName, bodyDBVO);

        /*
        GoodsDBVO goodsDBVO = GoodsDBManager.instance.getVOByID(1100);
        ObjectManager.instance.creatObject<GoodsDBVO, Goods>(goodsDBVO.prefabName, new Vector3(-1,0,-1), Quaternion.Euler(new Vector3(0,-50,0)), goodsDBVO);

        BodyDBVO enemyDBVO = BodyDBManager.instance.getVOByID(2001);
        ObjectManager.instance.creatObject<BodyDBVO, Enemy>(enemyDBVO.prefabName, new Vector3(1, 0, 4), Quaternion.Euler(new Vector3(0, 0, 0)), enemyDBVO);

        BodyDBVO enemyDBVO2 = BodyDBManager.instance.getVOByID(2002);
        ObjectManager.instance.creatObject<BodyDBVO, Enemy>(enemyDBVO2.prefabName, new Vector3(-1, 0, 3), Quaternion.Euler(new Vector3(0, 0, 0)), enemyDBVO2);

        BodyDBVO enemyDBVO3 = BodyDBManager.instance.getVOByID(2003);
        ObjectManager.instance.creatObject<BodyDBVO, Enemy>(enemyDBVO3.prefabName, new Vector3(-3, 0, 3), Quaternion.Euler(new Vector3(0, 0, 0)), enemyDBVO3);

        BodyDBVO enemyDBVO4 = BodyDBManager.instance.getVOByID(2004);
        ObjectManager.instance.creatObject<BodyDBVO, Enemy>(enemyDBVO4.prefabName, new Vector3(3, 0, 4), Quaternion.Euler(new Vector3(0, 0, 0)), enemyDBVO4);

        BodyDBVO enemyDBVO5 = BodyDBManager.instance.getVOByID(2005);
        ObjectManager.instance.creatObject<BodyDBVO, Enemy>(enemyDBVO5.prefabName, new Vector3(4, 0, 4), Quaternion.Euler(new Vector3(0, 0, 0)), enemyDBVO5);
        */

        levelVO = LevelDBManager.instance.getVOByID(5);
        enemyIdArr = levelVO.enemy.Split(new char[] { ',' });
        probabilityArr = levelVO.probability.Split(new char[] { ',' });
        creatCDArr = levelVO.creatCD.Split(new char[] { ',' });
    }

    //===============================================================
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;
        }

        creatCD -= Time.deltaTime;
        if (creatCD <= 0)
        {
            creatCD += Random.Range(float.Parse(creatCDArr[0]), float.Parse(creatCDArr[1]));
            Debug.Log("creatCD:" + creatCD);

            int num = Random.Range(0, 100);
            Debug.Log("num:" + num);

            int len = enemyIdArr.Length;
            int sum = 0;

            for (int i = 0; i < len; i++ )
            {
                sum += int.Parse(probabilityArr[i]);
                if (num < sum)
                {
                    BodyDBVO enemyDBVO = BodyDBManager.instance.getVOByID(int.Parse(enemyIdArr[i]));
                    Vector3 v3 = new Vector3(Random.Range(-GameConstant.STAGE_WIDTH * 0.7f, GameConstant.STAGE_WIDTH * 0.7f), 0, GameConstant.STAGE_HEIGHT * 1.2f);
                    Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(-30, 30), 0));
                    ObjectManager.instance.creatObject<BodyDBVO, Enemy>(enemyDBVO.prefabName, v3, rotation, enemyDBVO);

                    Debug.Log("x:" + v3.x);
                    Debug.Log("rotation.y:" + rotation.eulerAngles.y);
                    break;
                }
            }
        }
    }

    void OnGUI()
    {
        if (Time.timeScale == 0)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 60, 100, 30), "继续游戏"))
            {
                Time.timeScale = 1;
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 30), "退出游戏"))
            {
                Application.Quit();
            }
        }

        Transform playerTF = ObjectManager.instance.playerTF;
        if (!playerTF)
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect((Screen.width - 200) >> 1, (Screen.height - 250) >> 1, 400, 80), "游戏失败");

            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "再来一次"))
            {
                Time.timeScale = 1;
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
        GUI.skin.label.fontSize = 14;
        GUI.Label(new Rect(5, 5, 200, 20), "score:" + GameData.instance.score);
        GUI.Label(new Rect(5, 25, 200, 20), "histroy:" + GameData.instance.histroy);
    }

    /// <summary>
    /// 更新右上角飞机数量
    /// </summary>
    /// <param name="value">需要显示的飞机数量</param>
    public void changePlayerLife(int value)
    {
        clearPlayerLeft();

        for (int i = 0; i < value - 1; i++)
        {
            GameObject gameObj = GameObject.Instantiate(ObjectManager.instance.getPrefab(GameConstant.PLAYER_LIFE_PREFAB_NAME)) as GameObject;
            gameObj.transform.position = new Vector3(GameConstant.STAGE_WIDTH - gameObj.renderer.bounds.size.x * i * 1.2f, 0,
                GameConstant.STAGE_HEIGHT - gameObj.renderer.bounds.size.z * 0.6f);
            playerLifeList.Add(gameObj);
        }
    }

    /// <summary>
    /// 清除右上角的飞机
    /// </summary>
    void clearPlayerLeft()
    {
        if (playerLifeList == null)
        {
            playerLifeList = new ArrayList();
        }
        else
        {
            int len = playerLifeList.Count;
            for (int i = len-1; i >= 0; i-- )
            {
                GameObject.Destroy(playerLifeList[i] as GameObject);
                playerLifeList.RemoveAt(i);
            }
        }
    }

}