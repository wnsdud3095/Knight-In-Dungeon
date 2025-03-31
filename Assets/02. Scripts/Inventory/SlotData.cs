[System.Serializable]
public class SlotData
{
    public int m_item_id;
    public int m_reinforcement_level;

    public SlotData(int item_id, int reinforcement_level)
    {
        m_item_id = item_id;
        m_reinforcement_level = reinforcement_level;
    }
}
