using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIInvoker : MonoBehaviour
{
    [SerializeField] private Button _lbButton;
    [SerializeField] private Button _settingsButton;

    public static event Action OnOpenSettings;
    public static event Action OnOpenLeaderboard;

    void Start()
    {
        Assert.IsNotNull( _lbButton );
        Assert.IsNotNull( _settingsButton );

        _lbButton.onClick.AddListener(delegate{ OnOpenLeaderboard?.Invoke(); });
        _settingsButton.onClick.AddListener(delegate{ OnOpenSettings?.Invoke(); });
    }
}
