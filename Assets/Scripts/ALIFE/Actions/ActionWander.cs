using UnityEngine;

namespace WinterUniverse
{
    public class ActionWander : ActionBase
    {
        [SerializeField] private float _radius = 10f;

        public override void OnStart()
        {
            base.OnStart();
            _pawn.SetDestinationInRange(_radius);
        }
    }
}