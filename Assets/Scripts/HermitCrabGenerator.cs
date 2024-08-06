using UnityEngine;

public class HermitCrabGenerator : MonoBehaviour
{
    public int seed = 0;

    public GameObject GooglyEye;
    public GameObject PillEye;
    public GameObject SquareEye;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed);

        ShellCreator shellCreator = new ShellCreator(128, 32);
        ClawCreator clawCreator = new ClawCreator();
        LegCreator legCreator = new LegCreator();
        EyeCreator eyeCreator = new EyeCreator(GooglyEye, PillEye, SquareEye);
        for (int i = 0; i < 20; i += 4)
        {
            for (int j = 0; j < 20; j += 4)
            {        
                float shellWidth = Random.value + 1f;
                float shellHeight = Random.value * 2 + 0.5f;
                float legExtrusion = Random.value + 1f;
                float legWidth = 0.1f + Random.value * 0.1f;
                float legHeight = Random.value + 1;
                GameObject shell = shellCreator.CreateShell(shellWidth, shellHeight);
                shell.transform.position = new Vector3(i, legHeight, j);

                Color crabColor = Color.Lerp(Color.red, Color.blue, Random.value);
                GameObject leftClaw = clawCreator.CreateLeft(0.1f + legExtrusion * Random.value / 2, crabColor);
                GameObject rightClaw = clawCreator.CreateRight(0.1f + legExtrusion * Random.value / 2, crabColor);
                leftClaw.transform.position = new Vector3(i + shellWidth / 2, legHeight, j);
                rightClaw.transform.position = new Vector3(i - shellWidth / 2, legHeight, j);

                // right legs
                float initTheta = 45;
                float thetaIncr = 15;
                for (int k = 0; k < 3; k++)
                {
                    GameObject leg = legCreator.CreateLeg(legHeight, legWidth, legExtrusion, crabColor);
                    leg.transform.position = new Vector3(i, legHeight, j);
                    leg.transform.RotateAround(leg.transform.position, Vector3.up, initTheta + k * thetaIncr);
                }
                // left legs
                initTheta = -45;
                thetaIncr = -15;
                for (int k = 0; k < 3; k++)
                {
                    GameObject leg = legCreator.CreateLeg(legHeight, legWidth, legExtrusion, crabColor);
                    leg.transform.position = new Vector3(i, legHeight, j);
                    leg.transform.RotateAround(leg.transform.position, Vector3.up, initTheta + k * thetaIncr);
                }

                float eyeStalkRotation = 30f + Random.value * 25f;
                float eyeHeight = 0.5f + (Random.value * 0.4f - 0.2f);
                float eyeballScale = 0.1f + (Random.value * 0.1f - 0.05f);
                float eyeStyleProb = Random.value;
                float eyeSeparation = 0.2f + Random.value * 0.2f;
                GameObject leftEye = eyeCreator.CreateEye(eyeHeight, eyeStyleProb, eyeballScale, eyeStalkRotation, crabColor);
                leftEye.transform.position = new Vector3(i - eyeSeparation / 2, eyeHeight * Mathf.Cos(eyeStalkRotation * Mathf.PI / 180f) + legHeight, j - shellWidth / 2 - Mathf.Sin(eyeStalkRotation * Mathf.PI / 180f) * eyeHeight);
                GameObject rightEye = eyeCreator.CreateEye(eyeHeight, eyeStyleProb, eyeballScale, eyeStalkRotation, crabColor);
                rightEye.transform.position = new Vector3(i + eyeSeparation / 2, eyeHeight * Mathf.Cos(eyeStalkRotation * Mathf.PI / 180f) + legHeight, j - shellWidth / 2 - Mathf.Sin(eyeStalkRotation * Mathf.PI / 180f) * eyeHeight);
            }
        }
    }
}
