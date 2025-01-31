using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Winter Universe/Inventory/Item/New Weapon")]
    public class ItemWeaponConfig : ItemBaseConfig
    {
        [SerializeField] private AnimatorOverrideController _controller;
        [SerializeField] private float _damage = 1f;
        [SerializeField] private float _range = 100f;
        [SerializeField] private float _fireRate = 300f;
        [SerializeField] private int _magSize = 10;
        [SerializeField] private int _bulletsPerShot = 1;

        public AnimatorOverrideController Controller => _controller;
        public float Damage => _damage;
        public float Range => _range;
        public float FireRate => _fireRate;
        public int MagSize => _magSize;
        public int BulletsPerShot => _bulletsPerShot;
    }
}