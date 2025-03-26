[System.Flags]
public enum ItemType
{
    NONE = 0,
    
    HELMET = 1 << 0,
    ARMOR = 1 << 1,
    WEAPON = 1 << 2,
    BELT = 1 << 3,
    SHOES = 1 << 4,
}