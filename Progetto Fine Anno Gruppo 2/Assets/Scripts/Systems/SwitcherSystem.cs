using System;
using System.Collections.Generic;
using UnityEngine;

public static class SwitcherSystem
{
   
    public static void SwitchMessageType(EMessageType messageType, Action gameOver = null, Action powerUp = null, Action moduleDocked = null, Action moduleDestroyed = null, Action noBattery = null, Action playerDeath = null)
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
            case EMessageType.PlayerDeath:
                playerDeath.Invoke();
                break;
        }
    }

    public static void SwitchDirection(EDirection direction, Action up = null, Action down = null, Action right = null, Action left = null)
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
