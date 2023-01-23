using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberFlyToPlayerState : FlyToPlayerState
{
    public BomberFlyToPlayerState(EnemyBehavior behavior) : base(behavior)
    {
    }

    public override Type ShouldExit()
    {
        if (_enemyBehavior.health < 0)
        {
            return typeof(BomberDestroyState);
        }
        if (_enemyBehavior.distanceToPlayer < _enemyBehavior.requiredDistanceToFeelComfortable)
        {
            return typeof(BomberExplodeState);
        }
        return null;
    }
}

public class BomberExplodeState : StopAndShootState
{
    public BomberExplodeState(EnemyBehavior behavior) : base(behavior)
    {
    }

    public override Type ShouldExit()
    {
        return typeof(BomberDestroyState);
    }
}

public class BomberDestroyState : DestroyState
{
    public BomberDestroyState(EnemyBehavior behavior) : base(behavior)
    {
    }

    public override void Explode()
    {
        Debug.Log("Explode");
        _enemyBehavior.Shoot();
        _enemyBehavior.SelfDestroy();
    }
}

