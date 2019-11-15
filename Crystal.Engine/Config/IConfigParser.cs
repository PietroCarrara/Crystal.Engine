namespace Crystal.Engine.Config
{
    public interface IConfigParser
    {
        CrystalConfig FromText(string text);
        CrystalConfig FromFile(string path);
    }
}