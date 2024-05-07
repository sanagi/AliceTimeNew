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
