using System;
using System.Collections.Generic;
using UnityEngine;

public static class SwitcherSystem
{
   
    public static void SwitchMessageType(EMessageType messageType, Action gameOver, Action powerUp, Action moduleDocked, Action moduleDestroyed, Action noBattery)
    {
        switch (messageType)
        {
            case EMessageType.none:
                break;
            case EMessageType.GameOver:
                gameOver.Invoke();
                break;
            case EMessageType.PowerUp:
                powerUp.Invoke();
                break;
            case EMessageType.ModuleDocked:
                moduleDocked.Invoke();
                break;
            case EMessageType.ModuleDestroyed:
                moduleDestroyed.Invoke();
                break;
            case EMessageType.NoBattery:
                noBattery.Invoke();
                break;
        }
    }

    public static void SwitchDirection(EDirection direction, Action up, Action down, Action right, Action left)
    {
        switch (direction)
        {
            case EDirection.Up:
                up.Invoke();
                break;
            case EDirection.Down:
                down.Invoke();
                break;
            case EDirection.Right:
                right.Invoke();
                break;
            case EDirection.Left:
                left.Invoke();
                break;
        }
    }
}
