using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapSystem;
public class GameManager : MonoBehaviour
{

    static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance ?? (instance = FindObjectOfType<GameManager>()); }
    }

    public float gameDeltaTime;
    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(this);
    }

    private void Update()
    {
        gameDeltaTime = Time.deltaTime;
    }
}
