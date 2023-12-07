namespace MiniTowerDefence
{
    public class Ally : Soldier
    {
        protected override PathWay GetPathway()
        {
            return PathWay.Backward;
        }
    }
}