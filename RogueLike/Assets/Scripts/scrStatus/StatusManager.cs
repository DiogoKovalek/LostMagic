using System.Collections;
using UnityEngine;

public class StatusManager : MonoBehaviour {
    public IStatusAplicate statsEntity;

    //Burning =========================
    public GameObject iconBurning;
    public int TimeForBurning = 3;
    public int damageBurning = 1;

    private Coroutine coroutineBurning = null;
    private int countTimeInBurning = 0;
    //=================================

    //Stunned =========================
    public GameObject iconStuned;
    public int TimeForStuned = 1;

    private Coroutine coroutineStuned = null;
    private int countTimeInStunned = 0;
    //=================================

    //Wet =============================
    //Quando estiver molhado, nao tera nenhum efeito negativo
    private bool isWet = false;
    //=================================
    void Start() {
        statsEntity = GetComponent<IStatusAplicate>();
    }

    public void AplicateStatus(Element element) {
        switch (element) {
            case Element.Fire:
                if (!isWet) aplicateBurning();
                break;
            case Element.Earth:
                if (!isWet) aplicateStunned();
                break;
            case Element.Water:
                aplicateWet();
                break;
        }
    }
    #region Burning
    private void aplicateBurning() {
        statsEntity.managerStatus(Status.Burning, true);
        if (coroutineBurning == null) {
            coroutineBurning = StartCoroutine(Burning(TimeForBurning));
        }
        else {
            countTimeInBurning = 0;
        }
    }
    private IEnumerator Burning(int timeBurning) {
        while (countTimeInBurning < timeBurning) {
            statsEntity.damageBurning(damageBurning);
            countTimeInBurning++;
            yield return new WaitForSeconds(1f);
        }
        statsEntity.managerStatus(Status.Burning, false);
        coroutineBurning = null;
        countTimeInBurning = 0;
    }
    #endregion

    #region Strunned
    private void aplicateStunned() {
        statsEntity.managerStatus(Status.Stunned, true);
        if (coroutineStuned == null) {
            coroutineStuned = StartCoroutine(Stunned(TimeForStuned));
        }
        else {
            countTimeInStunned = 0;
        }

    }
    private IEnumerator Stunned(int timeStuned) {
        Debug.Log("Inicio de Stunned");
        statsEntity.Stunned(true);
        while (countTimeInStunned < timeStuned) {
            countTimeInStunned++;
            yield return new WaitForSeconds(1f);
        }
        statsEntity.Stunned(false);
        statsEntity.managerStatus(Status.Stunned, false);
        coroutineStuned = null;
        countTimeInStunned = 0;
    }
    #endregion

    #region Wet
    private void aplicateWet() {
        isWet = true;
        statsEntity.managerStatus(Status.Wet, true);
        if (coroutineBurning != null) {
            statsEntity.managerStatus(Status.Burning, false);
            StopCoroutine(coroutineBurning);
            coroutineBurning = null;
        }
        if (coroutineStuned != null) {
            statsEntity.managerStatus(Status.Stunned, false);
            StopCoroutine(coroutineStuned);
            coroutineStuned = null;
        }
    }
    public void removeWet() {
        isWet = false;
        statsEntity.managerStatus(Status.Wet, false);
    }
    #endregion
}

public enum Status {
    Burning,
    Stunned,
    Wet
}
