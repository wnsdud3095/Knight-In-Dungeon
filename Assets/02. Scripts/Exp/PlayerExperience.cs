using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int experience = 0; // 현재 경험치

    public void GainExperience(int amount)
    {
        experience += amount;
    }
}
