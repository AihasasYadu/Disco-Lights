using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightsController : MonoBehaviour
{
    private Button light;
    private AlongDiagonalEnum currentDiag;
    private LightStatesEnum currentState;
    public LightStatesEnum GetCurrentState { get { return currentState; } }
    public AlongDiagonalEnum GetDiagonalDirection { get { return currentDiag; } }
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
        currentDiag = AlongDiagonalEnum.Both;
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
        CallDisableEvent();
        yield return new WaitForSeconds(2);
        ChangeColorTo(Color.red);
        currentState = LightStatesEnum.Dead;
        EventManager.DisableInteractability -= DisableInteractive;
        EventManager.EnableInteractability -= MakeInteractive;
        CallEnableEvent();
    }

    private void CallDisableEvent()
    {
        EventManager.Instance.DisableEvent();
    }

    private void CallEnableEvent()
    {
        EventManager.Instance.EnableEvent();
    }

    private void MakeInteractive()
    {
        light.interactable = true;
    }

    private void DisableInteractive()
    {
        light.interactable = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        StartCoroutine(CheckStateInTrigger(collision));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StartCoroutine(CheckStateInCollision(collision));
    }

    private IEnumerator CheckStateInTrigger(Collider2D collision)
    {
        
        while (currentState == LightStatesEnum.Alive)
        {
            LightsController temp = new LightsController();
            if (collision.gameObject.GetComponent<LightsController>() != null && collision.GetType() == typeof(BoxCollider2D))
            {
                temp = collision.gameObject.GetComponent<LightsController>();
            }
            else
            {
                break;
            }

            if (temp.GetCurrentState == LightStatesEnum.Selected || temp.GetCurrentState == LightStatesEnum.Reachable)
            {
                if (temp.GetDiagonalDirection == AlongDiagonalEnum.Left || temp.GetDiagonalDirection == AlongDiagonalEnum.Both)
                {
                    SetLightsProperties(LightStatesEnum.Reachable, AlongDiagonalEnum.Left);
                }
            }
            yield return null;
        }
    }
    
    private IEnumerator CheckStateInCollision(Collision2D collision)
    {
        
        while (currentState == LightStatesEnum.Alive)
        {
            LightsController temp = new LightsController();
            if (collision.gameObject.GetComponent<LightsController>() != null && collision.GetType() == typeof(CircleCollider2D))
            {
                temp = collision.gameObject.GetComponent<LightsController>();
            }
            else
            {
                break;
            }

            if (temp.GetCurrentState == LightStatesEnum.Selected || temp.GetCurrentState == LightStatesEnum.Reachable)
            {
                if (temp.GetDiagonalDirection == AlongDiagonalEnum.Right || temp.GetDiagonalDirection == AlongDiagonalEnum.Both)
                {
                    SetLightsProperties(LightStatesEnum.Reachable, AlongDiagonalEnum.Right);
                }
            }
            yield return null;
        }
    }

    private void SetLightsProperties(LightStatesEnum curr, AlongDiagonalEnum diag)
    {
        currentDiag = diag;
        currentState = curr;
        ChangeColorTo(Color.green);
        StartCoroutine(KillLight());
    }
}
