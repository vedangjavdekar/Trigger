using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStateHandler 
{
    public void OnGameStateEnd(GameState state);
    public void OnGameStateStart(GameState state);
}
