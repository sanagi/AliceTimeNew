using System.Collections;
using UnityEngine;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト用スクリプト　常に移動を加えるスクリプト
    /// </summary>
    [DisallowMultipleComponent]
    public class ClockworkAddPosition : ClockworkCurveBase
    {
        /// <summary>
        /// 設定されている動作を開始する
        /// </summary>
        protected override void PlayCurve()
        {
            //処理を開始
            StartCoroutine(UpdateOffset());
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        IEnumerator UpdateOffset()
        {
            //アニメーションカーブの設定がある？
            if ((null == AnimationCurve) || (_curveSecond <= 0))
            {
                //中断
                Debug.LogError("カーブ設定がおかしいものがあります (" + name + ")\n" + _curveEffect);
                yield break;
            }

            //現在の値を取得
            Vector3 positionOrigin = transform.localPosition;
            Vector3 positionOffset;
            //経過時間
            float progressSec = (_curveStart * _curveSecond);
            //毎フレーム加える
            do
            {
                //停止処理の確認
                if (IsPauseUpdate())
                {
                    //停止中
                    yield return null;
                    continue;
                }

                //経過時間を記録
                if (globalFlag)
                {
                    progressSec = GlobalProgressSec;
                }
                else
                {
                    progressSec += GetDeltaTime();
                }
                //オフセット量
                positionOffset = (_curveEffect * GetCurrentCurveValue(AnimationCurve, progressSec, _curveSecond));
                //反映
                transform.localPosition = (positionOrigin + positionOffset);
                //待ち
                yield return null;
            }
            while (IsLoop(progressSec, _curveSecond));

            //終了処理
            Finish();
        }

#if UNITY_EDITOR
        /// <summary>
        /// エディタ用 カーブ初期化
        /// </summary>
        public override void InitializeCurve()
        {
            _curveEffect = new Vector3(0, 1, 0);
            AnimationCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 0, 1, 0));
        }
#endif
    }
}
