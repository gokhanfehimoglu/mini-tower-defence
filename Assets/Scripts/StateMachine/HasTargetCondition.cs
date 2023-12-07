using UnityEngine;

namespace MiniTowerDefence.StateMachine
{
    public class HasTargetCondition : Condition
    {
        [SerializeField]
        private TurretTargetGetter turretTargetGetter;

        [SerializeField]
        private bool shouldHaveTarget;

        public override bool IsMet() => turretTargetGetter.HasTarget == shouldHaveTarget;
    }
}