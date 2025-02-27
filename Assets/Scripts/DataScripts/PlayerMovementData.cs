﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    //Inspector
    public float maxSpeed;
    public float AccelerationTime;
}