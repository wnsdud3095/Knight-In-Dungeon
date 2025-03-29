[System.Flags]
public enum EvolutionType
{
    NONE            = 0,

    HP              = 1 << 0,
    ATK             = 1 << 1,
    HP_REGEN        = 1 << 2,
}