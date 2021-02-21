using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightsController : MonoBehaviour
{
    private Button light;
    private LightStatesEnum currentState;
    private const int DIAG_LAYER = 9;
    public LightStatesEnum GetCurrentState { get { return currentState; } }
    private void Start()
    {
        EventManager.DisableInteractability += DisableInteractive;
        EventManager.EnableInteractability += MakeInteractive;
        light = GetComponent<Button>();
        currentState = LightStatesEnum.Alive;
        light.onClick.AddListener(SelectedState);
    }

    private void SelectedState()
    {
        ChangeColorTo(Color.yellow);
        currentState = LightStatesEnum.Selected;
        EventManager.Instance.ButtonClickedEvent(gameObject.transform);
        StartCoroutine(KillLight());
    }

    private void ChangeColorTo(Color color)
    {
        ColorBlock c = light.colors;
        c.normalColor = color;
        c.disabledColor = color;
        light.colors = c;
    }

    private IEnumerator KillLight()
    {
        currentState = LightStatesEnum.Dead;
        yield return new WaitForEndOfFrame();
        CallDisableEvent();
        yield return new WaitForSeconds(1);
        ChangeColorTo(Color.red);
        EventManager.DisableInteractability -= DisableInteractive;
        EventManager.EnableInteractability -= MakeInteractive;
        yield return new WaitForEndOfFrame();
        CallEnableEvent();
    }

    private void CallEnableEvent()
    {
        EventManager.Instance.EnableEvent();
    }

    private void CallDisableEvent()
    {
        EventManager.Instance.DisableEvent();
    }

    private void MakeInteractive()
    {
        if(currentState != LightStatesEnum.Dead)
            light.interactable = true;
    }

    private void DisableInteractive()
    {
        light.interactable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer.Equals(DIAG_LAYER) && (currentState == LightStatesEnum.Alive))
        {
            SetLightsProperties();
        }
    }

    private void SetLightsProperties()
    {
        ChangeColorTo(Color.green);
        StartCoroutine(KillLight());
    }

    private void OnDestroy()
    {
        EventManager.DisableInteractability -= DisableInteractive;
        EventManager.EnableInteractability -= MakeInteractive;
    }
}
