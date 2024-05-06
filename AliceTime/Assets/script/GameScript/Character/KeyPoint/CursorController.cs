using System;
using UnityEngine;
using System.Collections;
using Rewired;
using System.Collections.Generic;
using InputSupport;
using R3;
using UnityEngine.Serialization;

public class CursorController : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 TargetPosition
    {
        set
        {
            targetPosition = new Vector3(value.x, value.y, GameDefine.Z_POS_0);
        }
    }

    // キャッシュ
    private Transform _transform;
    [SerializeField]
    private CursorParam _cursorParameter;
    private AliceInputManager _aliceInputManager;
    private Camera _uiCamera;
    
    bool _alreadyPoint = false;
    
    void Awake()
    {
        _transform = transform;
        TargetPosition = _transform.position;

        _aliceInputManager = AliceInputManager.Instance;
        _uiCamera = CameraManager.Instance.GetUiCamera();
    }

    void Update()
    {
        ContollerMove();
        
        Move();
        
        _aliceInputManager.Point.
            //ボタンが押されて
            Where(x => x && !_alreadyPoint)
            .Subscribe(_ =>
            {
                var tempPos = CameraManager.Instance.GetUiPos(Input.mousePosition);
                EffectManager.Instance.PlayEffect(EffectId.TouchHit, tempPos, Quaternion.identity, FadeManager.Instance.FadeCanvas.transform);      //エフェクト発行
                _alreadyPoint = true;
            });
        _aliceInputManager.PointRelease.Where(x=>x).Subscribe(_ =>
        {
            _alreadyPoint = false;
        });
    }

    /// <summary>
    /// 画面外制限
    /// </summary>
    private void InScreen()
    {
        var viewport = _uiCamera.WorldToViewportPoint(targetPosition);
        if (viewport.x < 0f)
        {
            targetPosition.x = _uiCamera.ViewportToWorldPoint(Vector3.zero).x;
        }
        if (viewport.x > 1.0f)
        {
            targetPosition.x = _uiCamera.ViewportToWorldPoint(Vector3.right).x;
        }
        if (viewport.y < 0f)
        {
            targetPosition.y = _uiCamera.ViewportToWorldPoint(Vector3.zero).y;
        }
        if (viewport.y > 1.0f)
        {
            targetPosition.y = _uiCamera.ViewportToWorldPoint(Vector3.up).y;
        }

        targetPosition.z = GameDefine.Z_POS_0;
    }
    
    private void ContollerMove()
    {
        var horizontal = _aliceInputManager.KeyPointMoveDirection.CurrentValue.x;
        var vertical = _aliceInputManager.KeyPointMoveDirection.CurrentValue.y;

        //入力があればtargetPos調整
        if (_aliceInputManager.CurrentInput == AliceInputManager.InputType.KeyMouse)
        {
            targetPosition = _aliceInputManager.GetMouseScreenPos();
        }
        else
        {
            targetPosition = _transform.position + new Vector3(horizontal * _cursorParameter.MinSpeed, vertical * _cursorParameter.MinSpeed, 0);
        }
        InScreen(); //補正
    }
    
    private void Move()
    {
        //通常アニメーション中かどうか
        var distance = Vector3.Distance(_transform.position, targetPosition);
        distance = distance > _cursorParameter.MaxSpeed ? _cursorParameter.MaxSpeed : distance < _cursorParameter.MinSpeed ? _cursorParameter.MinSpeed : distance;

        var time = (_cursorParameter.AccelSpeed / 100f) * (distance / _cursorParameter.Inertia);
        if (_aliceInputManager.CurrentInput == AliceInputManager.InputType.Controller)
        {
            _transform.position = Vector3.Lerp(_transform.position, targetPosition, time);
        }
        else
        {
            _transform.position = targetPosition;
        }
    }

    public void SetTurnAnimation( float time)
    {
        
    }

    public void SetParent(Transform a_parent)
    {
        _transform.parent = a_parent;
    }
}