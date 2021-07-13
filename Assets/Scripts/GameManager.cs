using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance ?? (instance = FindObjectOfType(typeof(GameManager)) as GameManager); }
    }

    public float gameDeltaTime;
    private void Awake()
    {
        instance = GameManager.Instance;
        if (instance == null) instance = this as GameManager;
        if (instance == this) DontDestroyOnLoad(this);
        else DestroyImmediate(this);
    }

    private void Update()
    {
        gameDeltaTime = Time.deltaTime;
    }
}
