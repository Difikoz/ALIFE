using UnityEngine;

namespace WinterUniverse
{
    public abstract class SensorBase : MonoBehaviour
    {
        protected PawnBase _pawn;

        public virtual void Initialize()
        {
            _pawn = GetComponentInParent<PawnBase>();
        }

        public abstract void Detect();
    }
}