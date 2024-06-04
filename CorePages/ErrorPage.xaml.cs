namespace LernzeitApp_Versuch2;

public partial class ErrorPage : ContentPage
{
	public ErrorPage(object ex)
	{
		InitializeComponent();
		ErrorMessage.Text = ex.ToString();
	}
}