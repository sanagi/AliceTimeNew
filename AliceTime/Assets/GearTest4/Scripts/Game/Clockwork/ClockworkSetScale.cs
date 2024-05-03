using System.Collections;
using UnityEngine;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト用スクリプト　周期的にスケールを変更するスクリプト
    /// </summary>
    [DisallowMultipleComponent]
    public class ClockworkSetScale : ClockworkCurveBase
    {
        /// <summary>
        /// スケールの設定方法
        /// </summary>
        public enum CalcType
        {
            Set,        //カーブの値を そのまま入れる
            Add,        //元のスケールに加算する
        }
        /// <summary>設定の種類</summary>
        [SerializeField, Tooltip("カーブの値を 直接スケールに代入するか、今の値に加算するか")] private CalcType _calcType = CalcType.Add;

        /// <summary>0以下を使用しないフラグ(falseの場合は絶対値)</summary>
        [SerializeField, Tooltip("0以下のとき、0で止めるフラグ")] private bool _clampZeroFlag = false;


        /// <summary>
        /// 設定されている動作を開始する
        /// </summary>
        protected override void PlayCurve()
        {
            //処理を開始
            StartCoroutine(UpdateOffset());
        }

        /// <summary>
        /// 回転処理
        /// </summary>
        IEnumerator UpdateOffset()
        {
            //アニメーションカーブの設定がある？
            if ((null == AnimationCurve) || (_curveSecond <= 0))
            {
                //中断
                Debug.LogError("カーブ設定がおかしいものがあります (" + name + ")");
                yield break;
            }

            //現在の値を取得
            Vector3 scaleOrigin = transform.localScale;
            float scaleNow;
            //経過時間
            float progressSec = (_curveStart * _curveSecond);
            //毎フレーム処理する
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
                //スケール取得
                scaleNow = GetCurrentCurveValue(AnimationCurve, progressSec, _curveSecond);
                //負数をどう扱う？
                if (_clampZeroFlag)
                {
                    //0以下にはならないようにする
                    scaleNow = Mathf.Max(0, scaleNow);
                }
                else
                {
                    //絶対値
                    scaleNow = Mathf.Abs(scaleNow);
                }
                //反映
                if (CalcType.Set == _calcType)
                {
                    //そのまま設定
                    transform.localScale = (_curveEffect * scaleNow);
                }
                else
                {
                    //オリジナルのスケールに加算
                    transform.localScale = (scaleOrigin + (_curveEffect * scaleNow));
                }
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
            _curveEffect = new Vector3(1, 1, 1);
            AnimationCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 0, 1, 0));
    }
#endif
    }
}
