using UnityEngine;
using Fusion;

public class NPlayer
{
    private int m_player_id;
    public int PlayerID { get { return m_player_id;} set { m_player_id = value; } }

    private PlayerRef m_player_ref;
    public PlayerRef PlayerRef { get { return m_player_ref; } set { m_player_ref = value; } }

    private NetworkObject m_player_obj;
    public NetworkObject PlayerObject { get { return m_player_obj; } set {m_player_obj= value; } }


    public NPlayer()
    {

    }
    public NPlayer(int m_player_id,PlayerRef m_player_ref, NetworkObject m_player_obj)
    {
        this.m_player_id = m_player_id;
        this.m_player_ref = m_player_ref;
        this.m_player_obj = m_player_obj;
    }
}
