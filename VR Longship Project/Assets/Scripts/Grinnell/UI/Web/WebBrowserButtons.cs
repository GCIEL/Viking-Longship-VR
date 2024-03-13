using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

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
    }

    private void OpenBrowser()
    {
        foreach(Transform child in transform) child.gameObject.SetActive(true);
    }

    public async void LoadURL(string url)
    {
        Debug.Log(url);
        if(!transform.GetChild(0).gameObject.activeSelf) OpenBrowser();
        await webBrowser.WaitUntilInitialized();
        webBrowser.WebView.LoadUrl(url);
    }


}
