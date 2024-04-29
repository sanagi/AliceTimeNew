using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayerAbilityBase : MonoBehaviour
{
    public enum AbilityType
    {
        Ladder,
        PushBlock
    }

    public AbilityType GType;

    protected PlayerController _playerController;

    protected virtual void Initialize()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.AddAbility(GType, this);
    }

    /// <summary>
    /// ギミック側で動かしたいメソッドを登録
    /// </summary>
    public abstract void SetHijackMove();

    /// <summary>
    /// ギミック側で動かしていたメソッドを解除
    /// </summary>
    public abstract void RemoveHijackMove();
    
}
