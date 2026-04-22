using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditorInternal.VersionControl.ListControl;
using static UnityEngine.Rendering.DebugUI.Table;

public class LevelManager : Singleton<LevelManager>
{
    readonly List<ColorType> liColorType = new List<ColorType>()
    {
        ColorType.Blue,
        ColorType.Brown,
        ColorType.Green,
        ColorType.Purple,
        ColorType.Red,
        ColorType.Yellow
    };
    public Level[] LevelPrefabs;
    public Bot botPrefab;
    public Player player;
    private List<Bot> liBot = new List<Bot>();
    private Level currentLevel;
    List<Stage> stages = new List<Stage>();
    public int CharacterAmount => currentLevel.botAmount + 1; // số lượng bot + player

    private int levelIndex;

    public Vector3 FinishPoint => currentLevel.finishPoint.position; // vị trí kết thúc

    private void Awake()
    {
        PlayerPrefs.DeleteAll(); // 🧹 Xoá sạch toàn bộ dữ liệu đã lưu
        levelIndex = 0;
        levelIndex = PlayerPrefs.GetInt("Level", 0);
    }
    private void Start()
    {
        LoadLevel(levelIndex);
        OnInit();
        UIManager.Instance.OpenUI<MainMenu>();
    }

    public void OnInit()
    {
        // vị trí xuất phát
        Vector3 index = currentLevel.startPoint.position = Vector3.back * 6;
        float space = 3f;
        Vector3 leftPoint = ((CharacterAmount / 2) - (CharacterAmount % 2) * 0.5f - 0.5f) * space * Vector3.left + index;
        List<Vector3> startPoint = new List<Vector3>();


        for (int i = 0; i < CharacterAmount; i++)
        {
            startPoint.Add(leftPoint + i * space * Vector3.right);
        }

        //khởi tạo list để random màu
        List<ColorType> liColor = Utilities.SortOrder(liColorType, CharacterAmount);
        //Set vị trí player
        int rand = UnityEngine.Random.Range(0, CharacterAmount);
        player.transform.position = startPoint[rand];
        player.transform.rotation = Quaternion.identity;

        startPoint.RemoveAt(rand);

        player.OnInit();
        //Set màu player
        player.ChangeColor(liColor[rand]);
        liColor.RemoveAt(rand);

        //Set vị trí và màu bot
        for (int i = 0; i < CharacterAmount - 1; i++)
        {
            //Bot bot = SimplePool.Spawn<Bot>(botPrefab, startPoint[i], Quaternion.identity);
            Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot, startPoint[i], Quaternion.identity);
            bot.ChangeColor(liColor[i]);
            bot.OnInit();
            liBot.Add(bot);
        }
    }

    public void LoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        if (level < LevelPrefabs.Length)
        {
            currentLevel = Instantiate(LevelPrefabs[level]);
            currentLevel.OnInit();
            stages = new List<Stage>(currentLevel.GetComponentsInChildren<Stage>());
        }
        else
        {
            Debug.LogWarning("⚠️ Level index vượt quá số lượng LevelPrefabs");

        }
    }

    public void OnStartGame()
    {

        GameManager.Instance.ChangeState(GameState.Gameplay);
        for (int i = 0; i < liBot.Count; i++)
        {
            liBot[i].ChangeState(new PatrolState());
        }
    }

    public void OnFinishGame()
    {
        for (int i = 0; i < liBot.Count; i++)
        {
            liBot[i].ChangeState(null);
            liBot[i].StopMove();

        }
    }

    public void OnReset()
    {
        SimplePool.CollectAll();
        SimplePool.ReleaseAll();
        liBot.Clear();


    }

    internal void OnRetry()
    {
        player.ChangeAnim("Idle");
        OnReset();
        LoadLevel(levelIndex);
        OnInit();
        UIManager.Instance.OpenUI<MainMenu>();
    }

    internal void OnNextLevel()
    {
        player.ChangeAnim("Idle");
        levelIndex++;
        PlayerPrefs.SetInt("Level", levelIndex);
        OnReset();
        LoadLevel(levelIndex);
        OnInit();
        UIManager.Instance.OpenUI<MainMenu>();
    }

    public Stage GetCurrentStageForBot(Bot bot)
    {
        if (stages == null || stages.Count == 0)
        {
            Debug.LogWarning("⚠️ listStage is empty in LevelManager!");
            return null;
        }

        foreach (Stage s in stages)
        {
            if (Vector3.Distance(bot.transform.position, s.transform.position) < 10f)
            {
                return s;
            }
          
        }
        return stages[0]; // fallback
    }
}
