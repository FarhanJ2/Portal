using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class ShowPosHUD : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement movement;

    private PanelRenderer panelRenderer;
    private int uiVersion = -1;

    private Label posLabel;
    private Label angLabel;
    private Label velLabel;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
    }

    private void OnEnable()
    {
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        if (uiVersion == version)
            return;
        uiVersion = version;

        posLabel = root.Q<Label>("pos-label");
        angLabel = root.Q<Label>("ang-label");
        velLabel = root.Q<Label>("vel-label");
    }

    private void Update()
    {
        if (player == null || posLabel == null)
            return;

        Vector3 pos = player.position;
        Vector3 ang = player.eulerAngles;

        // PlayerMovement.Velocity is the real internal velocity vector, updated every
        // FixedUpdate — avoids the 0/spike flicker you'd get from position-delta math
        // sampled at a different rate than the controller actually moves.
        float speed = movement != null ? movement.Velocity.magnitude : 0f;

        posLabel.text = $"pos: {pos.x:F2} {pos.y:F2} {pos.z:F2}";
        angLabel.text = $"ang: {ang.x:F2} {ang.y:F2} {ang.z:F2}";
        velLabel.text = $"vel: {speed:F2}";
    }
}
