using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieFlyToPlayerState : FlyToPlayerState
{
    public CookieFlyToPlayerState(EnemyBehavior behavior) : base(behavior)
    {
    }

    public override Type ShouldExit()
    {
        if (_enemyBehavior.health < 0)
        {
            return typeof(CookieDestroyState);
        }
        if (_enemyBehavior.distanceToPlayer < _enemyBehavior.requiredDistanceToFeelComfortable)
        {
            return typeof(CookieStopAndShootState);
        }
        if (_enemyBehavior.health < minimumHealthToStop)
        {
            return typeof(CookieFlyAwayState);
        }
        return null;
    }
}

public class CookieStopAndShootState : StopAndShootState
{
    public CookieStopAndShootState(EnemyBehavior behavior) : base(behavior)
    {
    }
    
    public override Type ShouldExit()
    {
        if (_enemyBehavior.health < 0)
        {
            return typeof(CookieDestroyState);
        }
        if (_enemyBehavior.distanceToPlayer > _enemyBehavior.requiredDistanceToFeelComfortable)
        {
            return typeof(CookieFlyToPlayerState);
        }
        if (_enemyBehavior.distanceToPlayer < _enemyBehavior.requiredDistanceToFeelComfortable*0.5f)
        {
            return typeof(CookieFlyAwayState);
        }
        if (_enemyBehavior.health < minimumHealthToStop)
        {
            return typeof(CookieFlyAwayState);
        }
        return null;
    }
}

public class CookieFlyAwayState : FlyAwayState
{
    public CookieFlyAwayState(EnemyBehavior behavior) : base(behavior)
    {
    }
    
    public override Type ShouldExit()
    {
        if (_enemyBehavior.health < 0)
        {
            return typeof(CookieDestroyState);
        }
        if (_enemyBehavior.health >= minimumHealthToContinue && _enemyBehavior.distanceToPlayer >= _enemyBehavior.requiredDistanceToFeelComfortable)
        {
            return typeof(CookieFlyToPlayerState);
        }

        return null;
    }
}

public class CookieDestroyState : DestroyState
{
    public CookieDestroyState(EnemyBehavior behavior) : base(behavior)
    {
    }
}

