﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject Player;
    public GameObject Centre;

    public readonly float CentreRadius = 9f;
}
