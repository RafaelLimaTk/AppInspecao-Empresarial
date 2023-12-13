namespace Inspeção_Empresarial
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("inspection/create", typeof(Views.Inspection.CreateAndEditPage));
        }
    }
}
