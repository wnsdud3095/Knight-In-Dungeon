using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public void Button_Level()
    {
        DataManager.Instance.Data.m_user_level++;
    }

    public void Button_Money()
    {
        DataManager.Instance.Data.m_user_money += 1000;
    }
}
