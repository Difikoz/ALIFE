using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Movement", menuName = "Winter Universe/Pawn/New Movement")]
    public class PawnMovementConfig : ScriptableObject
    {
        [SerializeField, Range(0.5f, 4f)] private float _acceleration = 2f;
        [SerializeField, Range(0.5f, 4f)] private float _deceleration = 4f;
        [SerializeField, Range(0.5f, 2f)] private float _rotateSpeed = 0.5f;

        public float Acceleration => _acceleration;
        public float Deceleration => _deceleration;
        public float RotateSpeed => _rotateSpeed;
    }
}