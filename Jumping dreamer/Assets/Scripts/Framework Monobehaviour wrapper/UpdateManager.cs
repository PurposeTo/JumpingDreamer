using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UpdateManager : SingletonSuperMonoBehaviour<UpdateManager>
{
    public static List<Action> AllUpdatesSuper = new List<Action>();
    public static List<Action> AllFixedUpdatesSuper = new List<Action>();
    public static List<Action> AllLateUpdatesSuper = new List<Action>();


    private void Update()
    {
        for (int i = 0; i < AllUpdatesSuper.Count; i++)
        {
            AllUpdatesSuper[i]?.Invoke();
        }
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < AllFixedUpdatesSuper.Count; i++)
        {
            AllFixedUpdatesSuper[i]?.Invoke();
        }
    }


    private void LateUpdate()
    {
        for (int i = 0; i < AllLateUpdatesSuper.Count; i++)
        {
            AllLateUpdatesSuper[i]?.Invoke();
        }
    }
}
