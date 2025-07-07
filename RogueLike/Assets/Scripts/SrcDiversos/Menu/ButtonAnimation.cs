using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Image>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Image>().enabled = false;
    }
}
