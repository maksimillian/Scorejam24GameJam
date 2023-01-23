using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFsm
{
    public IFsmState CurrentState => _currentState;
    private IFsmState _currentState = null;
    private IFsmState _initState = null;

    private Dictionary<Type, IFsmState> _fsmStates = new Dictionary<Type, IFsmState>();

    public EnemyFsm(IFsmState initState, IFsmState[] allStates)
    {   
        _initState = initState;
            
        foreach( var s in allStates )
        {
            _fsmStates.Add(s.GetType(), s);
        }
    }

    public void Start()
    {
        _currentState = _initState;
        _currentState.OnEnter();
    }

    public void UpdateTick()
    {
        if (_currentState != null)
        {
            var next = _currentState.ShouldExit();
            if ( next != null && next != _currentState.GetType() )
            {
                _currentState.OnExit();
                _currentState = _fsmStates[next];
                _currentState.OnEnter();
            }

            _currentState.OnUpdate();
        }
    }
}
