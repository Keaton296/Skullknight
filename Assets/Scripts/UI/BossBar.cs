using System;
using System.Collections;
using System.Collections.Generic;
using Skullknight.Core;
using UnityEngine;
using TMPro;
public class BossBar : MonoBehaviour
{
    public StatBar statBar;
    public TMP_Text title;
    private IDamageable entity;
    private void OnEnable()
    {
        entity = GameManager.Instance.CurrentBoss.GetComponent<IDamageable>();
        float ratio = (float) entity.Health / entity.MaxHealth;
        if (entity != null)
        {
            statBar.SetValue(ratio);
            entity.OnHealthChanged.AddListener(OnValueChange);
        }
    }

    void Awake()
    {
        GameManager.Instance.OnStateChange.AddListener(OnGameStateChange);
    }

    private void OnGameStateChange(GameManager.EGameManagerState newState)
    {
        switch (newState)
        {
            case GameManager.EGameManagerState.Playing:
                this.enabled = true;
                break;
            case GameManager.EGameManagerState.EscapeMenu:
                break;
            case GameManager.EGameManagerState.Cutscene:
                this.enabled = false;
                break;
            case GameManager.EGameManagerState.Gameover:
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        entity?.OnHealthChanged.RemoveListener(OnValueChange);
    }

    private void OnValueChange(int newValue)
    {
        statBar.OnValueChange((float)newValue / entity.MaxHealth);
    }
}
