using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentButton : MonoBehaviour {
    public GameObject archievmentList;
    public Sprite neutral, highlight;
    private Image sprite;
    void Awake() {
        sprite = GetComponent<Image>();
    }
    public void Click() {
        if (sprite.sprite == neutral) {
            sprite.sprite = highlight;
            archievmentList.SetActive(true);
        } else {
            sprite.sprite = neutral;
            archievmentList.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
