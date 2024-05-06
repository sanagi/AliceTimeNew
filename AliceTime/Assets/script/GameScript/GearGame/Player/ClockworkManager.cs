using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using R3;

namespace Alice
{
    /// <summary>
    /// 時計挙動 管理クラス
    /// </summary>
    [DisallowMultipleComponent]
    public class ClockworkManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _debugRay = null;
        [SerializeField]
        private GameObject _debugFlag = null;


        /// <summary>３D用カメラ</summary>
        [SerializeField]
        private Camera _mainCamera = null;
        /// <summary>バックドロップのキューブマップ</summary>
        [SerializeField]
        private Material _cubemapMaterial = null;
        [SerializeField]
        private ClockworkPlayer _clockworkPlayer = null;


        /// <summary>全体管理の時間</summary>
        [SerializeField]
        private bool _autoRotation = false;

        /// <summary>全体管理の時間 背景用</summary>
        private float _targetProgressSecBg = 360;

        /// <summary>全体管理の時間 現在</summary>
        private float _targetProgressSecNow = 360;
        /// <summary>全体管理の時間 ログ</summary>
        private float _targetProgressSecLog = 360;

        /// <summary>カーブ設定</summary>
        [SerializeField]
        private AnimationCurve _lastWaveCurve = null;

        [SerializeField]
        private GearGameParam _gearGameParam;
        private float _lastWaveNow = 0;
        private float _lastWaveSign = 1;

        private AliceInputManager _aliceInputManager;
        private CursorController _cursorController;

        private void Start()
        {
            _aliceInputManager = AliceInputManager.Instance;
            _aliceInputManager.SetPointAction(SetPoint);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void Update()
        {
            UpdateGear();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void SetPoint()
        {
            // 実際のマウス位置とのずれを考慮
            // RenderTextureを使っているため
            var screenPosCursor = _mainCamera.WorldToScreenPoint(_aliceInputManager.CursorPos());

            //補正位置からレイを発射
            var tempRay = _mainCamera.ScreenPointToRay(screenPosCursor);
            
            _debugRay.transform.position = (tempRay.origin + (tempRay.direction / 2));
            _debugRay.transform.rotation = Quaternion.LookRotation(tempRay.direction, Vector3.up);

            float nearestDist = 1024f;
            var hitArray = Physics.RaycastAll(tempRay);
            foreach (var tempHit in hitArray)
            {
                if (nearestDist <= tempHit.distance)
                {
                    continue;
                }
                if (tempHit.normal.y < 0.8f)
                {
                    continue;
                }

                nearestDist = tempHit.distance;
                _debugFlag.transform.position = tempHit.point;
                _debugFlag.transform.SetParent(tempHit.transform, true);
                _clockworkPlayer.SetNavTarget(_debugFlag.transform);
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void UpdateGear()
        {
            _aliceInputManager.AutoRotate.
                //ボタンが押されて
                Where(x => x)
                .ThrottleFirst(TimeSpan.FromSeconds(_gearGameParam.NextButtonDelay))
                .Subscribe(_ =>
                {
                    _autoRotation = !_autoRotation;
                    if (!_autoRotation)
                    {
                        _lastWaveSign = 1;
                        _lastWaveNow = 1;
                    }
                });
            
            _targetProgressSecBg += Time.deltaTime;

            if (_autoRotation)
            {
                _targetProgressSecNow += Time.deltaTime / 1f;
            }
            else
            {
                if (_aliceInputManager.LeftRotate.CurrentValue)
                {
                    if (_aliceInputManager.LeftRotateDown.CurrentValue)
                    {
                        _targetProgressSecNow -= .5f;
                    }
                    else
                    {
                        _targetProgressSecNow -= Time.deltaTime * 3f;
                    }
                    _lastWaveSign = -1;
                    _lastWaveNow = 1;
                }
                else
                if (_aliceInputManager.RightRotate.CurrentValue)
                {
                    if (_aliceInputManager.RightRotateDown.CurrentValue)
                    {
                        _targetProgressSecNow += .5f;
                    }
                    else
                    {
                        _targetProgressSecNow += Time.deltaTime * 3f;
                    }
                    _lastWaveSign = 1;
                    _lastWaveNow = 1;
                }
                else
                {
                    _targetProgressSecNow = (float)((int)(_targetProgressSecNow + 0.5f));
                }
            }

            _targetProgressSecLog = Mathf.Lerp(_targetProgressSecNow, _targetProgressSecLog, 0.5f);
            _cubemapMaterial.SetFloat(GameDefine._ROTATION, _targetProgressSecBg + _targetProgressSecLog);

            float tempWave = 0;
            if (_lastWaveNow > 0)
            {
                _lastWaveNow -= Time.deltaTime;
                float tempRate = 1f - Mathf.Clamp01(_lastWaveNow);
                tempWave = _lastWaveCurve.Evaluate(tempRate);
            }
            ClockworkCurveBase.GlobalProgressSec = (_targetProgressSecLog / 2f) + ((-tempWave * _lastWaveSign) / 8f);
        }
    }
}
