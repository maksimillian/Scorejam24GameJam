using System;
using UnityEngine;

public interface IFsmState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    Type ShouldExit();
}

public class FlyToPlayerState : IFsmState
{
    protected readonly EnemyBehavior _enemyBehavior = null;
    protected float minimumHealthToStop;

    public FlyToPlayerState(EnemyBehavior behavior)
    {
        _enemyBehavior = behavior;
        minimumHealthToStop = behavior.maxHealth * 0.2f;
    }

    public void OnEnter()
    {
        Debug.Log("FlyToPlayerState Entered");
    }

    public void OnUpdate()
    {
        FlyToPlayer();
        _enemyBehavior.ShootIfPossible();
    }

    private void FlyToPlayer()
    {
        Vector2 targetPos = MainShip.Instance.transform.position;
        Vector2 direction = (targetPos - (Vector2)_enemyBehavior.transform.position).normalized;

        _enemyBehavior.MoveInTheDirection(direction);
        _enemyBehavior.RotateInTheDirectionOf(direction);
    }

    public void OnExit()
    {
        Debug.Log("End FlyToPlayer");
    }

    public virtual Type ShouldExit()
    {
        if (_enemyBehavior.health < 0)
        {
            return typeof(DestroyState);
        }
        if (_enemyBehavior.distanceToPlayer < _enemyBehavior.requiredDistanceToFeelComfortable)
        {
            return typeof(StopAndShootState);
        }
        if (_enemyBehavior.health < minimumHealthToStop)
        {
            return typeof(FlyAwayState);
        }

        return null;
    }
}

public class StopAndShootState : IFsmState
{
    protected readonly EnemyBehavior _enemyBehavior = null;
    protected float minimumHealthToStop;

    public StopAndShootState(EnemyBehavior behavior)
    {
        _enemyBehavior = behavior;
        minimumHealthToStop = behavior.maxHealth * 0.2f;
    }

    public void OnEnter()
    {
        Debug.Log("Start rotating around the player");
    }

    public void OnUpdate()
    {
        Vector2 targetPos = MainShip.Instance.transform.position;
        Vector2 direction = (targetPos - (Vector2)_enemyBehavior.transform.position).normalized;
        
        _enemyBehavior.ShootIfPossible();
        _enemyBehavior.RotateInTheDirectionOf(direction);
    }

    public void OnExit()
    {
        Debug.Log("End RotateAroundPlayer");
    }

    public virtual Type ShouldExit()
    {
        if (_enemyBehavior.health < 0)
        {
            return typeof(DestroyState);
        }
        if (_enemyBehavior.distanceToPlayer > _enemyBehavior.requiredDistanceToFeelComfortable)
        {
            return typeof(FlyToPlayerState);
        }
        if (_enemyBehavior.distanceToPlayer < _enemyBehavior.requiredDistanceToFeelComfortable*0.5f)
        {
            return typeof(FlyAwayState);
        }
        if (_enemyBehavior.health < minimumHealthToStop)
        {
            return typeof(FlyAwayState);
        }
        return null;
    }
}

public class FlyAwayState : IFsmState
{
    protected readonly EnemyBehavior _enemyBehavior = null;
    protected float minimumHealthToContinue;

    public FlyAwayState(EnemyBehavior behavior)
    {
        _enemyBehavior = behavior;
        minimumHealthToContinue = behavior.maxHealth * 0.8f;
    }
    public void OnEnter()
    {
        Debug.Log("Start to FlyAway");
    }

    public void OnUpdate()
    {
        Vector2 targetPos = MainShip.Instance.transform.position;
        Vector2 direction = (targetPos - (Vector2)_enemyBehavior.transform.position).normalized;
        
        _enemyBehavior.MoveInTheDirection(-direction);
        
        _enemyBehavior.ShootIfPossible();
        _enemyBehavior.RotateInTheDirectionOf(direction);
    }

    public void OnExit()
    {
        Debug.Log("End FlyAway");
    }

    public virtual Type ShouldExit()
    {
        if (_enemyBehavior.health < 0)
        {
            return typeof(DestroyState);
        }
        if (_enemyBehavior.health >= minimumHealthToContinue && _enemyBehavior.distanceToPlayer >= _enemyBehavior.requiredDistanceToFeelComfortable)
        {
            return typeof(FlyToPlayerState);
        }

        return null;
    }
}

public class DestroyState : IFsmState
{
    protected readonly EnemyBehavior _enemyBehavior = null;

    public DestroyState(EnemyBehavior behavior)
    {
        _enemyBehavior = behavior;
    }
    public void OnEnter()
    {
        Debug.Log("Start to Destroy");
        Explode();
    }

    public void OnUpdate()
    {
    }

    public virtual void Explode()
    {
        _enemyBehavior.SelfDestroy();
    }

    public void OnExit()
    {
        Debug.Log("End life");
    }

    public Type ShouldExit()
    {
        return null;
    }
}