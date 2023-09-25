using Unity.VisualScripting;
using UnityEngine;

public class Player: MonoBehaviour {

    [SerializeField] private GameObject automata;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private GameObject pointer;
    [SerializeField] private float buildDistance = 10f;

    private RaycastHit hit;
    private bool isPaused = false;


    private void HandleMovement() {
        Vector3 inputDir = new Vector3(0, 0, 0) {
            z = Input.GetAxis("Vertical"),
            x = Input.GetAxis("Horizontal")
        };
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    public void TeleportTo(Vector3 location) {
        transform.position = location;
    }

    public void LookAt(Vector3 direction) {
        // Transform cameraTransform = Camera.main.transform;
        transform.LookAt(direction);
    }
    
    private void UpdatePointerPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 20)) {
            pointer.transform.position = hit.transform.position;
        } else {
            pointer.transform.position = ray.origin + ray.direction * buildDistance;
        }
    }
    
    private void Update() {
        HandleMovement();

        UpdatePointerPosition();

        if (Input.GetKeyUp(KeyCode.Space)) {
            isPaused = !isPaused;
            automata.GetComponent<Automata>().setPaused(isPaused);
        }

        if (Input.GetKeyUp(KeyCode.N)) {
            automata.GetComponent<Automata>().nextIteration();
        }

        if (Input.GetMouseButtonUp(0)) {
            UpdatePointerPosition();
            automata.GetComponent<Automata>().switchCell(pointer.transform.position);
        }
    }

}