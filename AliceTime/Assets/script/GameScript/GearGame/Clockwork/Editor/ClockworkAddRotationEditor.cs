using UnityEngine;
using UnityEditor;

namespace Alice
{
    /// <summary>
    /// 背景アーティスト設定用　常に回転を加えるスクリプト のInspector
    /// </summary>
    [CustomEditor(typeof(ClockworkAddRotation))]
    public class ClockworkAddRotationEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Inspector表示用
        /// </summary>
        public override void OnInspectorGUI()
        {
            //元のスクリプトを取得
            ClockworkAddRotation targetScript = target as ClockworkAddRotation;
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

        /// <summary>
        /// デバッグ用の再生ボタン
        /// </summary>
        public static void SetReplayButton(ClockworkCurveBase targetScript)
        {
            //スクリプトがなければ無視
            if ((null == targetScript) || (!EditorApplication.isPlaying))
            {
                return;
            }

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("停止"))
                {
                    targetScript.StopCurve();
                }
                if (GUILayout.Button("リプレイ"))
                {
                    targetScript.ReplayCurve();
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// カーブのプリセットボタンを表示する
        /// </summary>
        public static AnimationCurve SetAnimationCurveButtons()
        {
            //ボタンが押されたらカーブが入る
            AnimationCurve curve = null;

            GUILayout.BeginHorizontal();

            //アニメーションを変更する
            if (GUILayout.Button("― フラット"))
            {
                //直線(編集用に点が打ってあるプリセット)
                curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(0.25f, 0, 0, 0), new Keyframe(0.5f, 0, 0, 0), new Keyframe(0.75f, 0, 0, 0), new Keyframe(1, 0, 0, 0));
            }
            if (GUILayout.Button("／ 上り"))
            {
                //上り斜線
                curve = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(1, 1, 1, 0));
            }
            if (GUILayout.Button("＼ 下り"))
            {
                //下り斜線
                curve = new AnimationCurve(new Keyframe(0, 1, 0, -1), new Keyframe(1, 0, -1, 0));
            }
            if (GUILayout.Button("┌┐矩形波"))
            {
                //矩形波
                curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(0, 1, 0, 0), new Keyframe(0.5f, 1, 0, 0), new Keyframe(0.5f, 0, 0, 0), new Keyframe(1, 0, 0, 0));
            }
            if (GUILayout.Button("～ サイン波"))
            {
                //Sin波
                curve = new AnimationCurve(new Keyframe(0, 0, 0, 10.5f), new Keyframe(1, 0, 10.5f, 0));
            }
            if (GUILayout.Button("∩ 放物線"))
            {
                //放物線
                curve = new AnimationCurve(new Keyframe(0, 0, 0, 4), new Keyframe(1, 0, -4, 0));
            }
            if (GUILayout.Button("Ｌ クリック"))
            {
                //最初にちょっと跳ねて後フラット
                curve = new AnimationCurve(new Keyframe(0, 0, 0, 80), new Keyframe(0.05f, 0, -80, 0), new Keyframe(0.05f, 0, 0, 0), new Keyframe(1, 0, 0, 0));
            }

            GUILayout.EndHorizontal();

            //カーブを返す
            return curve;
        }
    }
}
