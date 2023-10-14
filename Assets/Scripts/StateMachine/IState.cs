using UnityEngine;

public interface IState 
{
    public void OnStateStart();
    public void OnStateEnd();
    public void StateFixedUpdate();
    public void StateUpdate();
}
