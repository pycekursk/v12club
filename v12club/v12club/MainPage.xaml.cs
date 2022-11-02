using Xamarin.Essentials;
using Xamarin.Forms;

namespace v12club
{
  public partial class MainPage : Xamarin.Forms.TabbedPage
  {
    public MainPage()
    {
      InitializeComponent();
      App.IsBusy = true;
    }
  }
}
