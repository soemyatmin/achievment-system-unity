using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievment {
    private string name;
    private string description;
    private bool unlocked;
    private int points;
    private int spriteIndex;
    private GameObject achievmentRef;

    private List<Achievment> dependencies = new List<Achievment>();
    private string child;

    private int currentProgression;
    private int maxProgression;

   
    public Achievment(string name, string description, int points, int spriteIndex, GameObject achievmentRef, int maxProgression) {
        this.name = name;
        this.description = description;
        this.unlocked = false;
        this.points = points;
        this.spriteIndex = spriteIndex;
        this.achievmentRef = achievmentRef;
        this.maxProgression = maxProgression;
        LoadAchievment();
    }

    public void AddDependency(Achievment dependency) {
        dependencies.Add(dependency);
    }

    public bool EarnAchievment() {
        if (!unlocked && !dependencies.Exists(x=>x.unlocked == false) && CheckProgress()) {
            unlocked = true;
            achievmentRef.GetComponent<Image>().sprite = AchievmentManager.Instance().unlockedSprite;
            SaveAchievment(true);

            if (child != null) {
                AchievmentManager.Instance().EarnAchievment(child);
            }

            return true;
        }
        return false;
    }

    public void SaveAchievment(bool value) {
        unlocked = value;
        int tmpPoints = PlayerPrefs.GetInt("Points",0);
        PlayerPrefs.SetInt("Points", tmpPoints + points);
        PlayerPrefs.SetInt("Progression" + name, currentProgression);
        PlayerPrefs.SetInt(name, value ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void LoadAchievment() {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true: false ;
        if (unlocked) {
            AchievmentManager.Instance().textPoints.text = "point: " + PlayerPrefs.GetInt("Points");
            currentProgression = PlayerPrefs.GetInt("Progression" + name);
            achievmentRef.GetComponent<Image>().sprite = AchievmentManager.Instance().unlockedSprite;
        }
    }

    public bool CheckProgress() {
        currentProgression++;

        achievmentRef.transform.GetChild(0).GetComponent<Text>().text = name + "" + currentProgression + "/" + maxProgression;
        SaveAchievment(false);
        if (maxProgression == 0) {
            return true;
        }
        if (currentProgression >= maxProgression) {
            return true;
        }
        return false;

    }

    public string GetChild() {
        return child;
    }

    public void SetChild(string value) {
        child = value;
    }

    public string GetDescription() {
        return description;
    }
    public void SetDescription(string value) {
        description = value;
    }

    public bool GetUnlocked() {
        return unlocked;
    }

    public void SetUnlocked(bool value) {
        unlocked = value;
    }

    public int GetPoints() {
        return points;
    }

    public void SetPoints(int value) {
        points = value;
    }

    public int GetSpriteIndex() {
        return spriteIndex;
    }

    public void SetSpriteIndex(int value) {
        spriteIndex = value;
    }

    public GameObject GetAchievmentRef() {
        return achievmentRef;
    }

    public void SetAchievmentRef(GameObject value) {
        achievmentRef = value;
    }

    public int GetCurrentProgression() {
        return currentProgression;
    }

    public void SetCurrentProgression(int value) {
        currentProgression = value;
    }

    public int GetMaxProgression() {
        return maxProgression;
    }

    public void SetMaxProgression(int value) {
        maxProgression = value;
    }
}
