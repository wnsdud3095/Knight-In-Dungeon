using UnityEngine;
using System.Collections;
using Fusion;

public class GameManager : Singleton<GameManager>
{
    public BulletPoolManager BulletPool { get; set; }

    public NetworkRunner NowRunner { get; set; }
    public NetworkObjectManager NetworkObjectManager { get; set; }

    private GameEventType m_game_state;
    public GameEventType GameState
    {
        get { return m_game_state; }
        private set { m_game_state = value; }
    }

    private ItemInventory m_item_inventory;
    public ItemInventory Inventory
    {
        get { return m_item_inventory; }
        private set { m_item_inventory = value; }
    }

    private EquipmentInventory m_equipment_inventory;
    public EquipmentInventory Equipment
    {
        get { return m_equipment_inventory; }
        private set { m_equipment_inventory = value; }
    }

    private CalculatedStat m_calculated_stat;
    public CalculatedStat CalculatedStat
    {
        get { return m_calculated_stat; }
        set { m_calculated_stat = value; }
    }

    private PlayerCtrl m_player_ctrl;
    public PlayerCtrl Player
    {
        get { return m_player_ctrl; }
        set { m_player_ctrl = value; }
    }

    public PlayerCtrl Player1 { get; set; }
    public PlayerCtrl Player2 { get; set; }

    private StageManager m_stage_manager;
    public StageManager StageManager
    {
        get { return m_stage_manager; }
        set { m_stage_manager = value; }
    }

    private Finisher m_finisher;
    public Finisher Finisher
    {
        get { return m_finisher; }
        private set { m_finisher = value; }
    }

    private bool m_can_init = true;

    private new void Awake()
    {
        base.Awake();

        GameEventBus.Subscribe(GameEventType.None, None);
        GameEventBus.Subscribe(GameEventType.Loading, Loading);
        GameEventBus.Subscribe(GameEventType.Waiting, Waiting);

        GameEventBus.Publish(GameEventType.None);
    }

    private void PlayEnemies()
    {
        // GameObject[] enemies = ObjectManager.Instance.GetAllObjects(ObjectType.Enemy);
        // foreach(GameObject enemy in enemies)
        // {
        //     EnemyCtrl enemy_ctrl = enemy.GetComponent<EnemyCtrl>();
            
        //     if(enemy_ctrl)
        //     {
        //         enemy.GetComponent<Animator>().speed = 1f;
        //     }
        // }
    }

    private void StopEnemies()
    {
        // GameObject[] enemies = ObjectManager.Instance.GetAllObjects(ObjectType.Enemy);
        // foreach(GameObject enemy in enemies)
        // {
        //     EnemyCtrl enemy_ctrl = enemy.GetComponent<EnemyCtrl>();
            
        //     if(enemy_ctrl)
        //     {
        //         enemy_ctrl.Rigidbody.linearVelocity = Vector2.zero;
        //         enemy.GetComponent<Animator>().speed = 0f;
        //     }
        // }        
    }

    private void InitEnemies()
    {
        // GameObject[] enemies = ObjectManager.Instance.GetAllObjects(ObjectType.Enemy);
        // foreach(GameObject enemy in enemies)
        // {
        //     var enemy_ctrl = enemy.GetComponent<EnemyCtrl>();
            
        //     if(enemy_ctrl)
        //     {
        //         Destroy(enemy_ctrl);
        //     }
        // }     
    }

    public IEnumerator InitPlayers()
    {
        yield return new WaitForSeconds(1f);
        PlayerCtrl[] all_players = FindObjectsByType<PlayerCtrl>(FindObjectsSortMode.None);

        NetworkCallBack runner = NowRunner.GetComponent<NetworkCallBack>();

        foreach (PlayerRef playerRef in runner.PlayerRefs)
        {
            foreach (PlayerCtrl player in all_players)
            {
                var authority = player.GetComponent<NetworkObject>().InputAuthority;

                if (playerRef == authority)
                {
                    if (playerRef.PlayerId == 1)
                    {
                        Player1 = player;
                        Debug.Log($" Player1로 등록됨: {playerRef}");
                    }
                    else if (playerRef.PlayerId == 2)
                    {
                        Player2 = player;
                        Debug.Log($" Player2로 등록됨: {playerRef}");
                    }
                }
            }
        }
    }

    private void PlayArrows()
    {
        GameObject[] arrows = ObjectManager.Instance.GetAllObjects(ObjectType.Arrow);
        foreach(GameObject arrow_object in arrows)
        {
            Arrow arrow = arrow_object.GetComponent<Arrow>();

            if(arrow)
            {
                arrow.Resume();
            }
        }
    }

    private void StopArrows()
    {
        GameObject[] arrows = ObjectManager.Instance.GetAllObjects(ObjectType.Arrow);
        foreach(GameObject arrow_object in arrows)
        {
            Arrow arrow = arrow_object.GetComponent<Arrow>();

            if(arrow)
            {
                arrow.Stop();
            }
        }
    }

    public void None()
    {
        Debug.Log("논");
        GameState = GameEventType.None;

        SoundManager.Instance.PlayBGM("Login Background");
    }
    
    public void Loading()
    {
        Debug.Log("로딩");
        GameState = GameEventType.Loading;
    }

    public void Waiting()
    {
        Debug.Log("웨이팅");
        GameState = GameEventType.Waiting;

        SoundManager.Instance.PlayBGM("Title Background");

        m_can_init = true;

        Inventory = GameObject.Find("Inventory Manager").GetComponent<ItemInventory>();
        Inventory.Initialize();

        Equipment = m_item_inventory.GetComponent<EquipmentInventory>();
        Equipment.Initialize();

        InitEnemies();
    }

    public void Playing()
    {
        GameState = GameEventType.Playing;
        Debug.Log("플레잉");
        if(m_can_init)
        {
            m_can_init = false;

            SoundManager.Instance.PlayBGM("Game Background");
            
            //Player = GameObject.Find("Player").GetComponent<PlayerCtrl>();         
            StageManager = GameObject.Find("Stage Manager").GetComponent<StageManager>();
            if (StageManager == null) { Debug.Log("스테이지 매니저 초기화 실패"); }
            Finisher = GameObject.Find("Finish UI").GetComponent<Finisher>();
            BulletPool = GameObject.Find("Bullet Pool Manager").GetComponent<BulletPoolManager>();
        }
        else
        {
            if(!SettingManager.Instance.Data.BGM)
            {
                SoundManager.Instance.BGM.UnPause();
            }
        }

        PlayEnemies();
        PlayArrows();
        if(Player == null)
        {
            Debug.Log("애니메이터 널");
        }
        Player.Animator.speed = 1f;
    }

    public void Setting()
    {
        GameState = GameEventType.Setting;

        StopEnemies();
        StopArrows();

        Player.Animator.speed = 0f;
    }

    public void Selecting()
    {
        GameState = GameEventType.Selecting;

        StopEnemies();
        StopArrows();

        Player.Animator.speed = 0f;
    }

    public void Dead()
    {
        GameState = GameEventType.Dead;

        DataManager.Instance.Data.m_user_money += StageManager.Kill;
        DataManager.Instance.Data.m_user_exp += Mathf.FloorToInt(StageManager.OriginTimer - StageManager.GameTimer);

        StopEnemies();
        StopArrows();

        if(NowRunner.GetComponent<NetworkCallBack>().PlayerRefs.Count<2)
        {
            Finisher.OpenUI(false);
            NowRunner.Shutdown();
        }
        else
        {
            if(Player1.m_is_dead && Player2.m_is_dead)
            {
                Finisher.OpenUI(false);
                NowRunner.Shutdown();
            }
        }
    }

    public void Clear()
    {
        GameState = GameEventType.Clear;

        DataManager.Instance.Data.m_user_money += StageManager.Kill * DataManager.Instance.Data.m_current_stage;
        DataManager.Instance.Data.m_user_exp += Mathf.FloorToInt(StageManager.OriginTimer - StageManager.GameTimer);
        DataManager.Instance.Data.m_current_stage++;

        StopEnemies();
        StopArrows();

        Finisher.OpenUI(true);
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        if(GameState == GameEventType.Waiting && DataManager.Instance.Data is not null)
        {
            Inventory?.SaveSlotData();
            Equipment?.SaveSlotData();

            if(Inventory is not null && Equipment is not null)
            {
                DataManager.Instance.SaveUserData(DataManager.Instance.Data);
            }
        }
    }
}
