using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance ?? (instance = FindObjectOfType(typeof(GameManager)) as GameManager); }
    }
    private void Awake()
    {
        instance = GameManager.Instance;
        if (instance == null) instance = this as GameManager;
        if (instance == this) DontDestroyOnLoad(this);
        else DestroyImmediate(this);
        Application.targetFrameRate = 60;
    }
}
