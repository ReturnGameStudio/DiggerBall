using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Manager<LevelManager>
{

    [SerializeField] private bool isMenu;
    private int _sceneCount;
    private int _mainScenesCount = 1;

    public int currentLevel
    {
        get
        {
            if (PlayerPrefs.HasKey("level"))
            {
                return PlayerPrefs.GetInt("level");
            }
            else
            {
                PlayerPrefs.SetInt("level", 1);
                return 1;
            }
        }
        set
        {
            PlayerPrefs.SetInt("level", value);
        }
    }

    void Start()
    {

        _sceneCount = SceneManager.sceneCountInBuildSettings - _mainScenesCount;

        if (isMenu)
            LoadCurrentLevel();
        else
            UIManager.Instance.UpdateLevelText(currentLevel);

    }

    public void NextLevel()
    {
        currentLevel++;
        LoadCurrentLevel();
    }
    public void LoadCurrentLevel()
    {
        ChangeLevel(currentLevel);
    }


    public void ChangeLevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    private IEnumerator LoadLevel(int index)
    {
        while (index > _sceneCount )
            index = index % _sceneCount ;

        if (index == 0)
            index += _mainScenesCount;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    public int GetLevelIndex()
    {
        int level = currentLevel % _sceneCount;
        if (level == 0) level = 1;
        return level;
    }

}
