using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class SensorVision : SensorBase
    {
        [SerializeField] private float _viewDistance = 100f;
        [SerializeField] private float _viewAngle = 90f;
        [SerializeField] private LayerMask _damageableMask;
        [SerializeField] private LayerMask _obstacleMask;

        private List<PawnBase> _detectedTargets = new();

        public List<PawnBase> DetectedTargets => _detectedTargets;

        public override void Detect()
        {
            _detectedTargets.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, _viewDistance, _damageableMask);
            foreach (Collider collider in colliders)
            {
                PawnBase pawn = collider.GetComponentInParent<PawnBase>();
                if (pawn != null && !_detectedTargets.Contains(pawn) && TargetIsDetected(pawn, collider))
                {
                    _detectedTargets.Add(pawn);
                }
            }
        }

        public bool TargetIsDetected(PawnBase pawn, Collider collider = null)
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
            if (collider != null)
            {
                // check for collider in view angle
                if (Vector3.Angle(_pawn.Head.forward, (collider.transform.position - _pawn.Head.position).normalized) > _viewAngle / 2f)
                {
                    return false;
                }
                // check for collider hided by obstacle
                if (Physics.Linecast(_pawn.Head.position, collider.transform.position, _obstacleMask))
                {
                    return false;
                }
            }
            else
            {
                // check for head or body in view angle
                if (Vector3.Angle(_pawn.Head.forward, (pawn.Head.position - _pawn.Head.position).normalized) > _viewAngle / 2f)
                {
                    if (Vector3.Angle(_pawn.Head.forward, (pawn.Body.position - _pawn.Head.position).normalized) > _viewAngle / 2f)
                    {
                        return false;
                    }
                }
                // check for head or body hided by obstacle
                if (Physics.Linecast(_pawn.Head.position, pawn.Head.position, _obstacleMask))
                {
                    if (Physics.Linecast(_pawn.Head.position, pawn.Body.position, _obstacleMask))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            if (_pawn != null && _pawn.Head != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_pawn.Head.position, _viewDistance);
            }
        }
    }
}