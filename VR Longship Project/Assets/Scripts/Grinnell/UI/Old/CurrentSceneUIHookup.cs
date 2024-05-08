using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneUIHookup : MonoBehaviour
{

    public void Hookup()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch(sceneName)
        {
            case "Tutorial":
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).GetComponent<VRUIButton>().enabled = false;
                break;

            case "1 to 10 ship building":
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<VRUIButton>().enabled = true;
                Transform currentSceneUI = transform.GetChild(0).GetChild(0);

                ShipBuildingTable shipBuildingTable = (ShipBuildingTable) GameObject.FindObjectOfType(typeof(ShipBuildingTable));
                PresentationScreen presentationScreen = (PresentationScreen) GameObject.FindObjectOfType(typeof(PresentationScreen));

                VRUISlider shipHeightSlider = currentSceneUI.GetChild(0).GetComponent<VRUISlider>();
                shipHeightSlider.onValueChanged.AddListener(shipBuildingTable.SetHeight);

                VRUIButton infoButton = currentSceneUI.GetChild(1).GetComponent<VRUIButton>();
                VRUIButton videoButton = currentSceneUI.GetChild(2).GetComponent<VRUIButton>();
                infoButton.onClick.AddListener(presentationScreen.ShowDescription);
                videoButton.onClick.AddListener(presentationScreen.PlayVideo);
                break;

            default:
            break;
        }
    }
}
