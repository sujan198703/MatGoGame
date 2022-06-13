
[System.Serializable]
public class GameData 
{
    public byte number;
    public PAE_TYPE pae_type;
    public byte position;

    public GameData(byte number, PAE_TYPE pae_type, byte position)
    {
        this.number = number;
        this.pae_type = pae_type;
        this.position = position;
    }
}
