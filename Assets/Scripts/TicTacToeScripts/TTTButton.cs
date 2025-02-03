using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TTTButton : MonoBehaviour
{
    public Image buttonImage;       // UI Image component
    public Sprite activeSprite;     // Sprite for active state
    public Sprite disabledSprite;   // Sprite for disabled state
    public Sprite xSprite;          // Sprite for Player X
    public Sprite oSprite;          // Sprite for Player O

    public UnityEvent<int> OnButtonClickedEvent;  // Event triggered when the button is clicked

    private int buttonIndex;        // Index of this button on the board
    private bool isInteractable = true;  // Track if the button is clickable

    private void Start()
    {
        ResetButton();
    }

    public void Initialize(int index)
    {
        buttonIndex = index;
    }

    public void OnButtonClicked()
    {
        if (isInteractable) // Only trigger if the button is interactable
        {
            OnButtonClickedEvent?.Invoke(buttonIndex);
        }
    }

    public void UpdateState(string player)
    {
        buttonImage.sprite = player == "X" ? xSprite : oSprite;
        DisableInteraction(); // Disable the button after updating its state
    }

    public void ResetButton()
    {
        buttonImage.sprite = activeSprite;
        EnableInteraction();
    }

    public void EnableInteraction()
    {
        isInteractable = true;
        buttonImage.raycastTarget = true; // Allow the button to be clicked
    }

    public void DisableInteraction()
    {
        isInteractable = false;
        buttonImage.sprite = disabledSprite; // Optionally change to disabled sprite
    }
}
