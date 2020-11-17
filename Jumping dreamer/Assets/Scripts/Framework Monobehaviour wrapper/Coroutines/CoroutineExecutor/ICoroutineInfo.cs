﻿using System;
using System.Collections;


public interface ICoroutineInfo
{
    IEnumerator Enumerator { get; }
    bool IsExecuting { get; }

    Action OnCoroutineAlreadyStarted { get; set; }
    Action OnStopCoroutine { get; set; }
    Action OnCoroutineIsAlreadyStopped { get; set; }
}
