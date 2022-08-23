using System.Windows.Input;
using DevExpress.Xpf;
using DevExpress.Mvvm;

namespace AquilaErpWpfApp3.Utillity
{
    public class InfoViewModel : BindableBase {
        AboutInfo aboutInfo;
        ICommand showHelpCommand;
        ICommand showGettingStartedCommand;
        //ICommand showContactUsCommand;

        public AboutInfo AboutInfo {
            get {
                if(aboutInfo == null) {
                    aboutInfo = new AboutInfo();
                    //DevExpress.Internal.UserData data = DevExpress.Utils.About.Utility.GetInfoEx();
                    //aboutInfo.LicenseState = data.IsExpired ? LicenseState.TrialExpired : data.IsTrial ? LicenseState.Trial : LicenseState.Licensed;
                    //aboutInfo.ProductName = "(주) 대길 통상";
                    //aboutInfo.ProductPlatform = "ERP";
                    //aboutInfo.RegistrationCode = WpfSerialNumberProvider.GetSerial();
                    //aboutInfo.Version = AssemblyInfo.Version;
                }
                return aboutInfo;
            }
        }
        public void ShowHelp() {
            DevExpress.Xpf.Core.DocumentPresenter.OpenLink("http://www.iegkr.com/");
        }
        public void ShowGettingStarted() {
            DevExpress.Xpf.Core.DocumentPresenter.OpenLink("http://www.google.com");
        }
        //public void ShowContactUs()
        //{
        //    DevExpress.Xpf.Core.DocumentPresenter.OpenLink("http://devexpress.com/Support/Center");
        //}
        public ICommand ShowHelpCommand {
            get {
                if(showHelpCommand == null)
                    showHelpCommand = new DelegateCommand(ShowHelp, false);
                return showHelpCommand;
            }
        }
        public ICommand ShowGettingStartedCommand {
            get {
                if(showGettingStartedCommand == null)
                    showGettingStartedCommand = new DelegateCommand(ShowGettingStarted, false);
                return showGettingStartedCommand;
            }
        }
        //public ICommand ShowContactUsCommand {
        //    get {
        //        if(showContactUsCommand == null)
        //            showContactUsCommand = new DelegateCommand(ShowContactUs, false);
        //        return showContactUsCommand;
        //    }
        //}
    }
}
