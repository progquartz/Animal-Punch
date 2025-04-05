using JetBrains.Annotations;
using UnityEngine;

public interface IActorPattern
{
    public void Init(Enemy enemy);

    public void EnterPattern();
    public void ExitPattern();
    public void ActPattern();
}
