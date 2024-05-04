using UnityEngine;
using UnityEditor;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト設定用　周期的なスケールの変更をするスクリプト のInspector
    /// </summary>
    [CustomEditor(typeof(ClockworkSetScale))]
    public class ClockworkSetScaleEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Inspector表示用
        /// </summary>
        public override void OnInspectorGUI()
        {
            //元のスクリプトを取得
            ClockworkSetScale targetScript = target as ClockworkSetScale;
            if (null == targetScript.AnimationCurve)
            {
                //初期化
                targetScript.InitializeCurve();
            }
            //リプレイボタン
            ClockworkAddRotationEditor.SetReplayButton(targetScript);

            //親のGUI表示をそのままやる
            base.OnInspectorGUI();

            //カーブのプリセット
            {
                //アニメーションカーブのプリセットボタンを表示
                AnimationCurve next = ClockworkAddRotationEditor.SetAnimationCurveButtons();
                if (null != next)
                {
                    //プリセットボタンが押された
                    targetScript.AnimationCurve = next;
                }
            }
        }
    }
}
