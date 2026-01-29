public class BombSpawnerView : SpawnerView<Bomb>
{
    protected override string MakeString()
    {
        return "Бомбы\n" + base.MakeString();
    }
}
