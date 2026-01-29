public class CubeSpawnerView : SpawnerView<Cube>
{
    protected override string MakeString()
    {
        return "Кубы\n" + base.MakeString();
    }
}
