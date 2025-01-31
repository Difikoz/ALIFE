using UnityEngine;

namespace WinterUniverse
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private HandSlotType _handSlotType;

        private PawnBase _pawn;

        public HandSlotType HandSlotType => _handSlotType;

        public void Initialize()
        {
            _pawn = GetComponentInParent<PawnBase>();
        }
    }
}