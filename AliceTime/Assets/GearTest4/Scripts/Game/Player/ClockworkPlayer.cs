using UnityEngine;

namespace Alice
{
    /// <summary>
    /// 時計挙動 プレーヤー
    /// </summary>
    public class ClockworkPlayer : MonoBehaviour
    {
        /// <summary>アニメ</summary>
        [SerializeField]
        private Animator _playerAnimator = null;

        /// <summary>経路探索AI</summary>
        [SerializeField]
        private UnityEngine.AI.NavMeshAgent _playerAgent = null;
        private Transform _targetNode = null;



        /// <summary>
        /// 更新処理
        /// </summary>
        private void Update()
        {
            float moveSpeed = 0;
            if (null != _targetNode)
            {
                var subPos = (_targetNode.position - transform.position);
                if (subPos.magnitude < 0.5f)
                {
                    _targetNode = null;
                    _playerAgent.ResetPath();
                }
                else
                {
                    _playerAgent.SetDestination(_targetNode.position);
                }
                moveSpeed = subPos.magnitude;
            }
            _playerAnimator.SetFloat("MoveSpeed", moveSpeed);


            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down * 1.1f, out var tempHit))
            {
                transform.SetParent(tempHit.transform, true);
            }
        }

        /// <summary>
        /// NavMeshで追従するターゲットを指定
        /// </summary>
        public void SetNavTarget(Transform tempNode)
        {
            _targetNode = tempNode;
        }
    }
}