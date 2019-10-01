using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentManager : MonoBehaviour {
    public GameObject achievmentPrefab;
    public Sprite[] sprites;

    public AchievmentButton activeButton;
    public ScrollRect scrollRect;

    public GameObject achievmentMenu;
    public GameObject visualAchievment;
    public Dictionary<string, Achievment> achievments = new Dictionary<string, Achievment>();

    public Sprite unlockedSprite;
    public Text textPoints;
    private static AchievmentManager instance;
    private int fadeTime = 2;
    public static AchievmentManager Instance() {
        if (instance == null) {
            instance = GameObject.FindObjectOfType<AchievmentManager>();
        }
        return instance;
    }

    void Start() {

        PlayerPrefs.DeleteAll();
        activeButton = GameObject.Find("GeneralBtn").GetComponent<AchievmentButton>();
        CreateAchievment("General", "Press W", "Press W to unlock", 5, 0,0);
        CreateAchievment("General", "Press S", "Press S to unlock", 5, 0, 0);
        CreateAchievment("General", "All", "Press all to unlock", 10, 0, 0, new string[] { "Press W", "Press S" });
        CreateAchievment("General", "Press A", "Press A 3 time to unlock", 5, 0, 3);

        CreateAchievment("Other", "red", "Press Red to unlock", 5, 1, 0);
        CreateAchievment("Other", "yellow", "Press yel to unlock", 5, 1, 0);
        CreateAchievment("Other", "green", "Press gre to unlock", 5, 1, 0);

        foreach (GameObject achievmentList in GameObject.FindGameObjectsWithTag("AchievmentList")) {
            achievmentList.SetActive(false);
        }

        activeButton.Click();
        achievmentMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            achievmentMenu.SetActive(!achievmentMenu.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            EarnAchievment("Press W");
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            EarnAchievment("Press S");
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            EarnAchievment("Press A");
        }
    }
    public void EarnAchievment(string title) {
        if (achievments[title].EarnAchievment()) {
            // do sth
            GameObject achievment = Instantiate(visualAchievment) as GameObject;
            SetAchievmentInfo("EarnCanvas", achievment, title);
            textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            StartCoroutine(FadeAchievment(achievment));
        }
    }

    public IEnumerator HideAchievment(GameObject achievment) {
        yield return new WaitForSeconds(2);
        Destroy(achievment);
    }

    public void CreateAchievment(string parent, string title, string description, int points, int spriteIndex,int progress, string[] dependencies = null) {
        GameObject achievment = Instantiate(achievmentPrefab) as GameObject;
        Achievment newAchievment = new Achievment(title, description, points, spriteIndex, achievment, progress);
        achievments.Add(title, newAchievment);
        SetAchievmentInfo(parent, achievment, title, progress);
        if (dependencies != null) {
            foreach (string achievmentTitle in dependencies) {
                Achievment dependency = achievments[achievmentTitle];
                dependency.SetChild(title);
                newAchievment.AddDependency(achievments[achievmentTitle]);
                // dependency = press space <-- child = press w
                // new achie = press w --> press space
            }
        }
    }
    public void SetAchievmentInfo(string parent, GameObject achievment, string title, int progression = 0) {
        achievment.transform.SetParent(GameObject.Find(parent).transform);
        achievment.transform.localScale = new Vector3(1, 1, 1);
        string progress = progression > 0 ? "" + achievments[title].GetCurrentProgression() + "/" + progression.ToString() : string.Empty;
        achievment.transform.GetChild(0).GetComponent<Text>().text = title + progress;
        achievment.transform.GetChild(1).GetComponent<Text>().text = achievments[title].GetDescription();
        achievment.transform.GetChild(2).GetComponent<Text>().text = achievments[title].GetPoints().ToString();
        achievment.transform.GetChild(3).GetComponent<Image>().sprite = sprites[achievments[title].GetSpriteIndex()];

    }

    public void ChangeCategory(GameObject button) {
        AchievmentButton achievmentButton = button.GetComponent<AchievmentButton>();
        scrollRect.content = achievmentButton.archievmentList.GetComponent<RectTransform>();
        achievmentButton.Click();
        activeButton.Click();

        activeButton = achievmentButton;
    }

    private IEnumerator FadeAchievment(GameObject achievment) {
        CanvasGroup canvasGroup = achievment.GetComponent<CanvasGroup>();

        float rate = 1.0f / fadeTime;
        int startAlpha = 0;
        int endAlpha = 1;
        for (int i = 0; i < 2; i++) {
            float progress = 0.0f;
            while (progress < 1.0) {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(2);
            startAlpha = 1;
            endAlpha = 0;
        }
        Destroy(achievment);
    }
}
