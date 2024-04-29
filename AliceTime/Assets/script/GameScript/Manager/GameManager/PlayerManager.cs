using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 2つのplayerControllerを切り替える
/// </summary>
public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    public bool IsControllable()
    {
        if (_currentPlayer == null)
        {
            return false;
        }
        return _currentPlayer.IsControllable();
    }
    private PlayerController _tickPlayer;

    /// <summary>
    /// 現在activeなplayer
    /// </summary>
    private PlayerController _currentPlayer;
    public PlayerController CurrentPlayer => _currentPlayer;
    public void SetCurrentPlayer(PlayerController p)
    {
        _currentPlayer = p;
        //_cameraTarget.SetParent(_currentPlayer.transform);
        _cameraTarget.localPosition = Vector3.zero;
    }

    private Transform _cameraTarget;
    public Transform CameraTarget => _cameraTarget;

    private void Awake()
    {
        _cameraTarget = new GameObject("CameraTarget").transform;
    }
    private void Update()
    {
        /*if (_currentPlayer.IsControllable())
        {
            _cameraTarget.position = _currentPlayer.transform.position;
        }
        */
    }

    public void MoveCameraTarget(float duration, Ease easeType, RotateController.PlaneType planeType)
    {
        /*switch (planeType)
        {
            case RotateController.PlaneType.XY:
                _cameraTarget.SetParent(_xyPlayer.transform);
                break;
            case RotateController.PlaneType.YZ:
                _cameraTarget.SetParent(_yzPlayer.transform);
            break;
        }
        _cameraTarget.DOLocalMove(Vector3.zero, duration).SetEase(easeType).OnComplete(() =>
        {
            _cameraTarget.localPosition = Vector3.zero;
        });
        */
    }
    /*public void ChangePlayer(RotateController.PlaneType planeType)
    {
        switch (planeType)
        {
            case RotateController.PlaneType.XY:
                EnableXYPlayer();
                DisableYZPlayer();
                _currentPlayer = _xyPlayer;
                break;
            case RotateController.PlaneType.YZ:
                EnableYZPlayer();
                DisableXYPlayer();
                _currentPlayer = _yzPlayer;
                break;
        }

        //CameraManager.Instance.GamePlayCamera.ChangeTarget(_currentPlayer.transform);
    }*/

    public void WakeUpRigidBody()
    {
        if (_currentPlayer == null)
        {
            return;
        }
        //_currentPlayer.RigidbodyWakeUp();
    }
    public void EnablePhysics()
    {
        if (_currentPlayer == null)
        {
            return;
        }
        _currentPlayer.EnablePhysics();
    }
    public void DisablePhysics()
    {
        if (_currentPlayer == null)
        {
            return;
        }
        _currentPlayer.DisablePhysics();
    }

    public void EnableControllable()
    {
        if (_currentPlayer == null)
        {
            return;
        }
        _currentPlayer.EnableControllable();
    }
    public void DisableControllable()
    {
        if (_currentPlayer == null)
        {
            return;
        }
        _currentPlayer.DisableControllable();
    }
    
    /*private void EnableXYPlayer()
    {
        if (_xyPlayer == null)
        {
            return;
        }
        _xyPlayer.EnablePhysics();
        _xyPlayer.EnableControllable();
    }
    
    private void DisableXYPlayer()
    {
        if (_xyPlayer == null)
        {
            return;
        }
        _xyPlayer.DisablePhysics();
        _xyPlayer.DisableControllable();
    }
    
    private void EnableYZPlayer()
    {
        if (_yzPlayer == null)
        {
            return;
        }
        _yzPlayer.EnablePhysics();
        _yzPlayer.EnableControllable();
    }
    
    private void DisableYZPlayer()
    {
        if (_yzPlayer == null)
        {
            return;
        }
        _yzPlayer.DisablePhysics();
        _yzPlayer.DisableControllable();
    }    
    
    /// <summary>
    /// キャラクターをリスポーン
    /// </summary>
    public void Respawn()
    {
        if (_currentPlayer == null)
        {
            return;
        }
        _currentPlayer.Respawn();
    }
    */
    
    public PlayerController.STATE CurrentState { get { return _currentPlayer.CurrentState; } }
    public void SetState(PlayerController.STATE state)
    {
        if (_currentPlayer == null)
        {
            return;
        }
        _currentPlayer.SetState(state);
    }

    public void RemoveHijackMove(PlayerAbilityBase.AbilityType abilityType)
    {
        switch (abilityType)
        {
            case PlayerAbilityBase.AbilityType.Ladder:
                _currentPlayer.AbilityDictionary[abilityType].RemoveHijackMove();
                break;
        }
    }
}
