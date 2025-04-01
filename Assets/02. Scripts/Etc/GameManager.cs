using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerCtrl Player { get; set; }
    public BulletPoolManager BulletPool { get; set; }

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

    private new void Awake()
    {
        base.Awake();

        GameEventBus.Subscribe(GameEventType.None, None);
        GameEventBus.Subscribe(GameEventType.Loading, Loading);
        GameEventBus.Subscribe(GameEventType.Waiting, Waiting);
        GameEventBus.Subscribe(GameEventType.Playing, Playing);

        GameEventBus.Publish(GameEventType.None);
    }

    public void None()
    {
        GameState = GameEventType.None;
    }
    
    public void Loading()
    {
        GameState = GameEventType.Loading;
    }

    public void Waiting()
    {
        GameState = GameEventType.Waiting;

        Inventory = GameObject.Find("Inventory Manager").GetComponent<ItemInventory>();
        Inventory.Initialize();

        Equipment = m_item_inventory.GetComponent<EquipmentInventory>();
        Equipment.Initialize();
    }

    public void Playing()
    {
        GameState = GameEventType.Playing;
        Player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        BulletPool = GameObject.Find("Bullet Pool Manager").GetComponent<BulletPoolManager>();
    }

    public void Setting()
    {

    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            if(GameState == GameEventType.Waiting && DataManager.Instance.Data is not null)
            {
                Inventory?.SaveSlotData();
                Equipment?.SaveSlotData();
                DataManager.Instance.SaveUserData(DataManager.Instance.Data);
            }
        }
    }

    private void OnApplicationQuit()
    {
        if(GameState == GameEventType.Waiting && DataManager.Instance.Data is not null)
        {

            Inventory?.SaveSlotData();
            Equipment?.SaveSlotData();
            DataManager.Instance.SaveUserData(DataManager.Instance.Data);
        }
    }
}
