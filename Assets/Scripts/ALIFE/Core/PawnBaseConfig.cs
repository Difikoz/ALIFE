using UnityEngine;

namespace WinterUniverse
{
    public class PawnBaseConfig : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController _controller;

        public AnimatorOverrideController Controller => _controller;
    }
}