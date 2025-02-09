using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Targeting", menuName = "Winter Universe/Pawn/New Targeting")]
    public class PawnTargetingConfig : ScriptableObject
    {
        [SerializeField] private float _hearRadius = 10f;
        [SerializeField] private float _viewDistance = 100f;
        [SerializeField] private float _viewAngle = 120f;
        [SerializeField] private float _targetLostDistance = 100f;

        public float HearRadius => _hearRadius;
        public float ViewDistance => _viewDistance;
        public float ViewAngle => _viewAngle;
        public float TargetLostDistance => _targetLostDistance;
    }
}