using UnityEngine;
using UnityEditor;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト設定用　常に移動を加えるスクリプト のInspector
    /// </summary>
    [CustomEditor(typeof(ClockworkAddPosition))]
    public class ClockworkAddPositionEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Inspector表示用
        /// </summary>
        public override void OnInspectorGUI()
        {
            //元のスクリプトを取得
            ClockworkAddPosition targetScript = target as ClockworkAddPosition;
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
