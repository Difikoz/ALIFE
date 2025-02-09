using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Visual", menuName = "Winter Universe/Pawn/New Visual")]
    public class PawnVisualConfig : ScriptableObject
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private AnimatorOverrideController _controller;
        [SerializeField] private float _turnAnimationAngle = 45f;

        public GameObject Model => _model;
        public AnimatorOverrideController Controller => _controller;
        public float TurnAnimationAngle => _turnAnimationAngle;
    }
}