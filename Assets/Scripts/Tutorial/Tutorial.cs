using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    public event System.Action OnTutorialFinish; // Event to trigger when tutorial is finish  
    public event System.Action  OnTextChange; // Event to change our tutorial text
    public bool finished; // bool to show if tutorial is finish
    public string description; // description of tutorial

    public abstract void Execute(); // abstract method that must be overriden by derrived classes , it works like start method but we can delayed this method to execute when we want
    public virtual void TutorialCompleted() // Method to be called by derrived classes when tutorial has finished
    {
        finished = true;
        if (OnTutorialFinish != null)
            OnTutorialFinish();
    }

    public virtual void ChangeText(string text) // same but with tutorial text
    {
        description = text;

        if (OnTextChange != null)
            OnTextChange();
    }
}
