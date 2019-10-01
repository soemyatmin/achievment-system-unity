using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fruit : MonoBehaviour {
    public string achievmentName;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        if (!EventSystem.current.IsPointerOverGameObject(-1)) {
            AchievmentManager.Instance().EarnAchievment(achievmentName);
        }
    }
}
