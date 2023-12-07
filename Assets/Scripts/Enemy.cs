namespace MiniTowerDefence
{
    public class Enemy : Soldier
    {
        protected override PathWay GetPathway()
        {
            return PathWay.Forward;
        }
    }
}