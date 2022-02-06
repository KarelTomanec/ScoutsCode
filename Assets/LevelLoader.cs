using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;
    public Text progresText;
    public Text finalText;

    public void LoadLevel(int scene)
    {
        StartCoroutine(LoadAsync(scene));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progres = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progres;
            progresText.text = System.Math.Round(progres, 2) * 100f + "%";

            if (progres >= 0.9f)
            {
                finalText.gameObject.SetActive(true);
            }

            yield return null;
        }
    }
}
