using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public CameraControls cameraControls;

    public void GithubButton()
    {
        Application.OpenURL("https://github.com/jacobpalin");
    }

    public void LinkedINButton()
    {
        Application.OpenURL("https://www.linkedin.com/in/jacobpalin/");
    }

    public void YoutubeButton()
    {
        Application.OpenURL("https://www.youtube.com/@Jacob-Palin");
    }

    public void DiskbotsButton()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=yODe6TaV0Jw&list=PLP1VMbm9_UkTMyVC5MtyND_v31EyWM8gX");
    }

    public void NextIslandButton()
    {
        cameraControls.CycleToIsland(cameraControls.currentIslandIndex + 1);
    }
    public void LastIslandButton()
    {
        cameraControls.CycleToIsland(cameraControls.currentIslandIndex - 1);
    }
}