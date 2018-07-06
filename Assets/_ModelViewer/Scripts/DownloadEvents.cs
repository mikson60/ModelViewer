using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadEvents {

    public delegate void DownloadAction();
    public delegate void DownloadProgressAction(float progress);

    public static event DownloadAction OnDownloadStart;
    public static event DownloadAction OnDownloadEnd;

    public static event DownloadProgressAction OnDownloadProgressUpdate;


    public static void DownloadStart()
    {
        if (OnDownloadStart != null) OnDownloadStart();
    }
    public static void DownloadEnd()
    {
        if (OnDownloadEnd != null) OnDownloadEnd();
    }

    public static void DownloadProgressUpdate(float progress)
    {
        if (OnDownloadProgressUpdate != null) OnDownloadProgressUpdate(progress);
    }
}
