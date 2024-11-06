namespace OnTopReplica.AntiBrowserOcclusion {

  using Windows.Win32;
  using Windows.Win32.Foundation;
  using Windows.Win32.UI.Accessibility;

  public static class AntiOcclusionTracker {

    #region Fields

    private const uint EVENT_OBJECT_HIDE = 0x8003;
    private const uint EVENT_OBJECT_SHOW = 0x8002;
    private static readonly WINEVENTPROC PfnWinEventProc = WinEventProc;
    private static UnhookWinEventSafeHandle UnhookWinEventSafeHandle;
    private static bool showing_thumbnails_;

    #endregion Fields

    #region Methods

    public static void PerformAntiOcclusion() {
      PInvoke.NotifyWinEvent(EVENT_OBJECT_SHOW, FakeMultitaskingViewFrame.GetHandle(), 0, 0);
    }

    public static void Start() {
      Stop();

      PerformAntiOcclusion();

      UnhookWinEventSafeHandle =
        PInvoke.SetWinEventHook(
          EVENT_OBJECT_SHOW,
          EVENT_OBJECT_HIDE,
          PInvoke.GetModuleHandle(default(string)),
          PfnWinEventProc,
          0,
          0,
          0);
    }

    public static void Stop() {
      if(UnhookWinEventSafeHandle != null) {
        UnhookWinEventSafeHandle.Dispose();
      }

      UnhookWinEventSafeHandle = null;
    }

    private static void WinEventProc(
      HWINEVENTHOOK hWinEventHook,
      uint @event,
      HWND hwnd,
      int idObject,
      int idChild,
      uint idEventThread,
      uint dwmsEventTime) {
      // https://chromium.googlesource.com/chromium/src/+/refs/heads/main/ui/aura/native_window_occlusion_tracker_win.cc
      //
      // [...]
      //
      // No need to calculate occlusion if a zero HWND generated the event. This
      // happens if there is no window associated with the event, e.g., mouse move
      // events.
      ////if (!hwnd)
      ////  return;

      if(hwnd.IsNull) {
        return;
      }

      // We only care about events for window objects. In particular, we don't care
      // about OBJID_CARET, which is spammy.
      ////if (id_object != OBJID_WINDOW)
      ////  return;

      const int OBJID_WINDOW = 0;
      if(idObject != OBJID_WINDOW) {
        return;
      }

      // We generally ignore events for popup windows, except for when the taskbar
      // is hidden or when the popup is a Chrome Widget or Windows Taskbar, in
      // which case we recalculate occlusion.
      ////bool calculate_occlusion = true;
      ////if (::GetWindowLong(hwnd, GWL_STYLE) & WS_POPUP)
      ////{
      ////  std::wstring hwnd_class_name = gfx::GetClassName(hwnd);
      ////  calculate_occlusion = hwnd_class_name.starts_with(L"Chrome_WidgetWin_") ||
      ////                        hwnd_class_name == L"Shell_TrayWnd";

      ; // Nop, or is here something to do?

      // Detect if either the alt tab view or the task list thumbnail is being
      // shown. If so, mark all non-hidden windows as occluded, and remember that
      // we're in the showing_thumbnails state. This lasts until we get told that
      // either the alt tab view or task list thumbnail are hidden.
      ////if (event == EVENT_OBJECT_SHOW) {
      ////  // Avoid getting the hwnd's class name, and recomputing occlusion, if not
      ////  // needed.
      ////  if (showing_thumbnails_)
      ////    return;
      ////  std::string hwnd_class_name = base::WideToUTF8(gfx::GetClassName(hwnd));
      ////  if ((hwnd_class_name == "MultitaskingViewFrame" ||
      ////       hwnd_class_name == "TaskListThumbnailWnd"))
      ////  {
      ////    showing_thumbnails_ = true;
      ////    ui_thread_task_runner_->PostTask(
      ////        FROM_HERE, base::BindOnce(update_occlusion_state_callback_,
      ////                                  root_window_hwnds_occlusion_state_,
      ////                                  showing_thumbnails_));
      ////  }
      ////  return;

      if(@event == EVENT_OBJECT_SHOW) {
        if(showing_thumbnails_) {
          return;
        }

        // We ignore ourselves...
        if(hwnd == FakeMultitaskingViewFrame.GetHandle()) {
          return;
        }

        var hwnd_class_name = getClassName(hwnd);
        if(hwnd_class_name == "MultitaskingViewFrame" ||
           hwnd_class_name == "TaskListThumbnailWnd") {
          showing_thumbnails_ = true;
        }

        return;
      }

      ////} else if (event == EVENT_OBJECT_HIDE) {
      ////  // Avoid getting the hwnd's class name, and recomputing occlusion, if not
      ////  // needed.
      ////  if (!showing_thumbnails_)
      ////    return;
      ////  std::string hwnd_class_name = base::WideToUTF8(gfx::GetClassName(hwnd));
      ////  if (hwnd_class_name == "MultitaskingViewFrame" ||
      ////      hwnd_class_name == "TaskListThumbnailWnd")
      ////  {
      ////    showing_thumbnails_ = false;
      ////    // Let occlusion calculation fix occlusion state, even though hwnd might
      ////    // be a popup window.
      ////    calculate_occlusion = true;
      ////  }
      ////  else
      ////  {
      ////    return;
      ////  }
      ////}

      if(@event == EVENT_OBJECT_HIDE) {
        if(!showing_thumbnails_) {
          return;
        }

        var hwnd_class_name = getClassName(hwnd);
        if(hwnd_class_name == "MultitaskingViewFrame" ||
           hwnd_class_name == "TaskListThumbnailWnd") {
          showing_thumbnails_ = false;
          PerformAntiOcclusion();
        }

        return;
      }

      static unsafe string getClassName(HWND hwnd) {
        // The maximum length for <b>lpszClassName</b> is 256
        const int bufferLen = 257;
        fixed(char* bufferptr = new char[bufferLen]) {
          PWSTR lpClassName = bufferptr;
          _ = PInvoke.GetClassName(hwnd, lpClassName, bufferLen);
          return lpClassName.ToString();
        }
      }
    }

    #endregion Methods
  }
}
