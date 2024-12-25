using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 记录进度信息
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField]
    private RoadGenerator roadGenerator;

    public RockControl rock;

    [Tooltip("当前高度")]
    public float Height { get; private set; }

    [SerializeField]
    private TextMeshProUGUI heightText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHeightText() {
        Height = roadGenerator.RoadCount;
        
        heightText.text = Height.ToString();
    }

    
}
