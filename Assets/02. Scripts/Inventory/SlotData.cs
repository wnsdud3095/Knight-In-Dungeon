[System.Serializable]
public class SlotData
{
    public int m_item_id;
    public ItemType m_item_type;
    public int m_reinforcement_level;

    public SlotData(int item_id, ItemType item_type, int reinforcement_level)
    {
        m_item_id = item_id;
        m_item_type = item_type;
        m_reinforcement_level = reinforcement_level;
    }
}
