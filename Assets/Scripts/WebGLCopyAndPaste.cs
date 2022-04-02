using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class WebGLCopyAndPasteAPI
{

#if UNITY_WEBGL

    [DllImport("__Internal")]
    private static extern void initWebGLCopyAndPaste(StringCallback cutCopyCallback, StringCallback pasteCallback);
    [DllImport("__Internal")]
    private static extern void passCopyToBrowser(string str);
    [DllImport("__Internal")]
    private static extern void buttonCopyToBrowser(string str);

    delegate void StringCallback( string content );


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void Init()
    {
        if ( !Application.isEditor )
        {
            initWebGLCopyAndPaste(GetClipboard, ReceivePaste );
        }
    }

    private static void SendKey(string baseKey)
      {
        string appleKey = "%" + baseKey;
        string naturalKey = "^" + baseKey;

        var currentObj = EventSystem.current.currentSelectedGameObject;
        if (currentObj == null) {
          return;
        }
        {
          var input = currentObj.GetComponent<UnityEngine.UI.InputField>();
          if (input != null) {
            // I don't know what's going on here. The code in InputField
            // is looking for ctrl-c but that fails on Mac Chrome/Firefox
            input.ProcessEvent(Event.KeyboardEvent(naturalKey));
            input.ProcessEvent(Event.KeyboardEvent(appleKey));
            // so let's hope one of these is basically a noop
            return;
          }
        }
#if WEBGL_COPY_AND_PASTE_SUPPORT_TEXTMESH_PRO
        {
          var input = currentObj.GetComponent<TMPro.TMP_InputField>();
          if (input != null) {
            // I don't know what's going on here. The code in InputField
            // is looking for ctrl-c but that fails on Mac Chrome/Firefox
            // so let's hope one of these is basically a noop
            input.ProcessEvent(Event.KeyboardEvent(naturalKey));
            input.ProcessEvent(Event.KeyboardEvent(appleKey));
            return;
          }
        }
#endif
      }

      [AOT.MonoPInvokeCallback( typeof(StringCallback) )]
      private static void GetClipboard(string key)
      {
        SendKey(key);
        passCopyToBrowser(GUIUtility.systemCopyBuffer);
      }

      [AOT.MonoPInvokeCallback( typeof(StringCallback) )]
      private static void ReceivePaste(string str)
      {
        GUIUtility.systemCopyBuffer = str;
      }

      [AOT.MonoPInvokeCallback( typeof(StringCallback) )]
      public static void GetCopyClipboard(string str)
      {
        buttonCopyToBrowser(str);
      }

#endif

}
