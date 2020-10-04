﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopController : MonoBehaviour {

    public float loopDuration = 5f;
    private float loopTimeLeft;
    private Slider loopSlider;
    private Slider chargeSlider;
    private Transform shiftButton;

    private List<ILooper> subscribers = new List<ILooper>();

    private void Awake() {
        Transform UITrans = GameObject.FindGameObjectWithTag("UI").transform;
        loopSlider = UITrans.Find("LoopSlider").GetComponent<Slider>();
        chargeSlider = UITrans.Find("BreakMeter").GetComponent<Slider>();
        shiftButton = UITrans.Find("ButtonShift");
        loopSlider.gameObject.SetActive(this.isActiveAndEnabled);
        chargeSlider.gameObject.SetActive(this.isActiveAndEnabled);
        shiftButton.gameObject.SetActive(this.isActiveAndEnabled);
    }

    private void OnEnable() {
        loopTimeLeft = loopDuration;
        loopSlider.gameObject.SetActive(true);
        chargeSlider.gameObject.SetActive(true);
        shiftButton.gameObject.SetActive(true);
    }

    private void OnDisable() {
        if (loopSlider != null) {
            loopSlider.gameObject.SetActive(false);
        }
        if (chargeSlider != null) {
            chargeSlider.gameObject.SetActive(false);
        }
        if (shiftButton != null) {
            shiftButton.gameObject.SetActive(false);
        }
    }

    private void Update() {
        loopTimeLeft -= Time.deltaTime;
        loopSlider.value = GetLoopPercentage();
        if (loopTimeLeft <= 0) {
            UpdateLoopers();
            loopTimeLeft = loopDuration;
        }
    }

    public void Subscribe(ILooper iLooper) {
        subscribers.Add(iLooper);
        iLooper.SetState();
    }

    public void Unsubscribe(ILooper iLooper) {
        subscribers.Remove(iLooper);
    }

    private void UpdateLoopers() {
        foreach(ILooper iLooper in subscribers) {
            if (iLooper.IsLooping()) {
                iLooper.Loop();
            } else {
                iLooper.SetState();
            }
        }
    }

    public float GetLoopPercentage() {
        return Mathf.Clamp((loopDuration-loopTimeLeft)/loopDuration, 0, 1);
    }
}
