using Microsoft.Maui.Platform;

namespace Inspeção_Empresarial;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        CustomHandler();

        MainPage = new AppShell();
    }

    private void CustomHandler()
    {
        EntryBorder();
        DatePickerBorder();
    }

    private static void EntryBorder()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Border", (handler, view) =>
        {
#if ANDROID
            //Android
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif IOS || MACCATALYST
                //IOS e MAC
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
            //Windows
            handler.PlatformView.BorderThickness = new Thickness(0).ToPlatform();
#endif
        });
    }

    private static void DatePickerBorder()
    {
        Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("Border", (handler, view) =>
        {
#if ANDROID
            //Android
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif WINDOWS
            //Windows
            handler.PlatformView.BorderThickness = new Thickness(0).ToPlatform();
#endif
        });
    }
}
