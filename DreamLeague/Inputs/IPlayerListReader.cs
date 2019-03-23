namespace DreamLeague.Inputs
{
    public interface IPlayerListReader
    {
        PlayerList Read(string filePath);
    }
}