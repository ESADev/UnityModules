using UnityEngine;

public class TESTER : MonoBehaviour
{
    public KeyCode triggerKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SFXManager.Instance.PlaySFX("Jump");
        }
    }
}
