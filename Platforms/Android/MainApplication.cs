using Android.App;
using Android.Runtime;
using LernzeitApp_Versuch2.CorePages;

namespace LernzeitApp_Versuch2
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
