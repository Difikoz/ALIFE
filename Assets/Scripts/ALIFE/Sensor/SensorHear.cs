using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class SensorHear : SensorBase
    {
        [SerializeField] private float _hearRadius = 10f;
        [SerializeField] private LayerMask _pawnMask;
        [SerializeField] private LayerMask _obstacleMask;

        private List<PawnBase> _detectedTargets = new();

        public List<PawnBase> DetectedTargets => _detectedTargets;

        public override void Detect()
        {
            _detectedTargets.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, _hearRadius, _pawnMask);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out PawnBase pawn) && TargetIsDetected(pawn))
                {
                    _detectedTargets.Add(pawn);
                }
            }
        }

        public bool TargetIsDetected(PawnBase pawn)
        {
            // check for pawn is owner
            if (_pawn == pawn)
            {
                return false;
            }
            // check for pawn is dead
            if (pawn.IsDead)
            {
                return false;
            }
            // check for head or body hided by obstacle
            if (Physics.Linecast(_pawn.Head.position, pawn.Head.position, _obstacleMask))
            {
                if (Physics.Linecast(_pawn.Head.position, pawn.Body.position, _obstacleMask))
                {
                    return false;
                }
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            if (_pawn != null && _pawn.Head != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_pawn.Head.position, _hearRadius);
            }
        }
    }
}