using UnityEngine;

public interface IStatusAplicate {
    //Manager
    void managerStatus(Status status, bool aplicate);
    //Burning
    void damageBurning(int damage);
    bool isBurning();
    //Stunned
    void Stunned(bool isStuned);
}
