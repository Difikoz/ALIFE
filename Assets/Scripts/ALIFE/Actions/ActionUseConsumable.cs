using UnityEngine;

namespace WinterUniverse
{
    public class ActionUseConsumable : ActionBase
    {
        [SerializeField] private string _consumableType;
        //private ConsumableItemConfig _consumable;

        public override bool CanStart()
        {
            //_pawn.Inventory.GetConsumable(_consumableType, out _consumable);
            // if _consumable == null return false;
            return base.CanStart();
        }

        public override void OnStart()
        {
            base.OnStart();
            //_consumable.OnUse(_pawn);
        }
    }
}