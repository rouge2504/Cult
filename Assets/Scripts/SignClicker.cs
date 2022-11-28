using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignClicker : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) )
        {
            Transform objectHit = hit.transform;

            if (Input.GetMouseButtonDown(0) && objectHit.name == GameConstants.SIGN_GO)
            {
                GameplayManager.instance.UpdateClicks();
            }
            // Do something with the object that was hit by the raycast.
        }
    }
}
