using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Player: MonoBehaviour {

    [SerializeField] private GameObject automata;
    [SerializeField] private GameObject rightController;

    [SerializeField] private InputActionReference nextStepAction;
    [SerializeField] private InputActionReference togglePauseAction;
    [SerializeField] private InputActionReference toggleCellAction;

    private bool isPaused = false;

    void Update() {
        if (togglePauseAction.action.WasPressedThisFrame()) {
            isPaused = !isPaused;
            automata.GetComponent<Automata>().setPaused(isPaused);
        }

        if (nextStepAction.action.WasPressedThisFrame()) {
            automata.GetComponent<Automata>().nextIteration();
        }

        if (toggleCellAction.action.WasPressedThisFrame()) {
            automata.GetComponent<Automata>().switchCell(rightController.transform.position);
        }
    }

}