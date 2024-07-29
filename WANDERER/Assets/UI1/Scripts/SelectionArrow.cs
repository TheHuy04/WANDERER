using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private RectTransform rect;
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (options == null || options.Length == 0)
        {
            Debug.LogError("Options are not set in the inspector.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0 && SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(changeSound);
        }

        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if (currentPosition >= options.Length)
        {
            currentPosition = 0;
        }

        if (rect != null && options != null && options.Length > 0)
        {
            rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
        }
        else
        {
            Debug.LogError("Rect or options array is not set correctly.");
        }
    }

    private void Interact()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(interactSound);
        }

        if (options != null && options.Length > 0 && options[currentPosition] != null)
        {
            Button button = options[currentPosition].GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
            else
            {
                Debug.LogError("No Button component found on the current option.");
            }
        }
        else
        {
            Debug.LogError("Options are not set or current position is invalid.");
        }
    }
}
