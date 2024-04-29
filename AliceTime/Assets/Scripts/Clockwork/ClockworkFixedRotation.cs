using UnityEngine;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト用スクリプト　角度を固定するスクリプト
    /// </summary>
    public class ClockworkFixedRotation : MonoBehaviour
    {
        /// <summary>反映する角度</summary>
        private Quaternion _setQuaternion = Quaternion.identity;

        /// <summary>
        /// 開始時
        /// </summary>
        private void Start()
        {
            //まずは覚える
            _setQuaternion = transform.rotation;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void LateUpdate()
        {
            //入れ直し続ける
            transform.rotation = _setQuaternion;
        }
    }
}
