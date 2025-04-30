using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehavior 
{
    public void StartBehavior(Enemy enemy);
    public void UpdateBehavior(Enemy enemy);
}
