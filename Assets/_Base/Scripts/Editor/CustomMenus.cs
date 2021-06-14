using System;
using UnityEditor;
using UnityEngine;

public static class CustomMenus {
   
    [MenuItem("Utils/Take Game Screenshot")]
    private static void GameScreenshot() {

        string screenshotPath = Application.persistentDataPath;

        if (!System.IO.Directory.Exists(screenshotPath)) {
            System.IO.Directory.CreateDirectory(screenshotPath);
        }

        string fileName = Application.productName + "-" + Application.version + "_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-sszzzUTC") + ".png";
        fileName = fileName.Replace(':', '-');
        fileName = fileName.Replace(' ', '_');
        screenshotPath += "/" + fileName;
        try {
            ScreenCapture.CaptureScreenshot(screenshotPath);
            Debug.Log($"Saved screenshot: {screenshotPath}");
        } catch(Exception e) {
            Debug.LogWarning($"Error while saving screenshot: {e.Message}");
        }
    }

}