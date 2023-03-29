using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BridgeChecker
{

    public bool BridgeWorks(Stick stick, Platform platform)
    {

        float platformSizeX = platform.GetSizeX();
        float stickLength = stick.GetLength();

        float platformLeftDistance = platform.transform.position.x - platformSizeX / 2 - stick.transform.position.x;
        float platformRightDistance = platformLeftDistance + platformSizeX;


        Debug.Log($"StickLengt = {stickLength}, platform sizeX = {platformSizeX}, left = {platformLeftDistance}, right = {platformRightDistance}");

        if (platformLeftDistance <= stickLength && stickLength <= platformRightDistance)
            return true;

        return false;
    }



}
