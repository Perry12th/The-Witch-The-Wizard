using UnityEngine;

public class ClownfaceEyeballScript : MonoBehaviour
{
    [SerializeField]
    private FerrisWheelScript ferrisWheelScript;
    [SerializeField]
    private PlayerSpotter spotter;
    [SerializeField]
    private float zAxisProjectionScale = -40;
    [SerializeField]
    private float xAxisEyeAngleCorrection = -90;
    private bool isPowered;

    private void Start()
    {
        ferrisWheelScript.OnPoweredUp.AddListener(OnPowerUp);
    }

    private void Update()
    {
        if (spotter.playerWithinRange && isPowered)
        {
            Vector3 playerPosition = spotter.player.gameObject.transform.position + new Vector3(0, 0, zAxisProjectionScale);
            transform.LookAt(playerPosition);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + xAxisEyeAngleCorrection, transform.eulerAngles.y, transform.eulerAngles.z);   
        }
    }

    private void OnPowerUp()
    {
        isPowered = true;
    }

}
