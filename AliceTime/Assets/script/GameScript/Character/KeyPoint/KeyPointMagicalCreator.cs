using System;
using UnityEngine;
using System.Collections;
using Rewired;
using KamioriInput;
using System.Collections.Generic;
using InputSupport;
using R3;

public class KeyPointMagicalCreator
{
    public GameObject CreatePastMagicalObj;
    public GameObject CreatePastMagicalObjGhost;
    
    private GameObject _createMagicalCircle;
    private GameObject _createMagicalCircleGhost;
    public void CreatePastMagicalCircle(Vector3 pos)
    {
        //既に作ってたらそっちを先に消す
        DestroyPastMagicalCircle();
        
        //ゴーストがあれば消す
        DestroyPastMagicalCircleGhost();
        
        if (CreatePastMagicalObj != null)
        {
            _createMagicalCircle = GameObject.Instantiate(CreatePastMagicalObj, pos, Quaternion.identity);
        }
    }

    public void CreatePastMagicalCircleGhost(Vector3 pos, Transform keyPointTransform)
    {
        //既に実態の魔法陣作ってたら消す
        DestroyPastMagicalCircle();
        
        if (CreatePastMagicalObjGhost != null)
        {
            _createMagicalCircleGhost = GameObject.Instantiate(CreatePastMagicalObjGhost, pos, Quaternion.identity);
        }
    }

    public void DestroyPastMagicalCircle()
    {
        if (_createMagicalCircle != null)
        {
            GameObject.Destroy(_createMagicalCircle);
        }
    }
    
    public void DestroyPastMagicalCircleGhost()
    {
        if (_createMagicalCircleGhost != null)
        {
            GameObject.Destroy(_createMagicalCircleGhost);
        }
    }
}