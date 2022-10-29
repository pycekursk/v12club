
using System;

using v12club.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace v12club.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LoginView : ContentView
  {
    public LoginView(HybridWebView webView)
    {
      InitializeComponent();

      try
      {
        this.BindingContext = new LoginViewModel(webView);
      }
      catch (Exception)
      {


      }
    }

    private async void Login_TextChanged(object sender, EventArgs e)
    {
      if (e.GetType() == typeof(TextChangedEventArgs))
      {
        await (sender as Entry).VerifyPhone((TextChangedEventArgs)e);
      }
      else
      {
        await (sender as Entry).VerifyPhone();
      }
    }

    private void Login_Focused(object sender, FocusEventArgs e)
    {
      if (sender is Entry entry && string.IsNullOrEmpty(entry.Text))
      {
        entry.Text = "+7(";
        entry.CursorPosition = entry.Text.Length;
      }
      else (sender as Entry).CursorPosition = (sender as Entry).Text.Length;
    }
  }
}