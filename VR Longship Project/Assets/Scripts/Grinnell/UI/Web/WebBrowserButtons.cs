using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

/*
This class just implements a few buttons for the web beowser inside the Info panel
*/
public class WebBrowserButtons : MonoBehaviour
{
    private CanvasWebViewPrefab webBrowser;
    void Start()
    {
        webBrowser = transform.GetChild(0).GetComponent<CanvasWebViewPrefab>();
    }

    public void Return()
    {
        webBrowser.WebView.GoBack();
    }

    public void CloseBrowser()
    {
        foreach(Transform child in transform) child.gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }

    private void OpenBrowser()
    {
        foreach(Transform child in transform) child.gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }

    public async void LoadURL(string url)
    {
        Debug.Log(url);
        if(!transform.GetChild(0).gameObject.activeSelf) OpenBrowser();
        await webBrowser.WaitUntilInitialized();
        GetComponent<BoxCollider>().enabled = true;
        webBrowser.WebView.LoadUrl(url);
    }


}
