using System.Collections;
using UnityEngine;

public interface IAtributesComunique {
    float CalculateSpeed();
    int CalculateAtack(Element project);
}
public interface IManaManager {
    void expendMana(int value);
    int getTotalMana();
}
public interface IPlayer {
    void TakeDamage(int atack, Element element);
    void RecoilAttack(Vector2 posE, float force);
    void AddHP(int HPRecover);
}
