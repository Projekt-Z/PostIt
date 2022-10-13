namespace PostIt.Web.Installers;

public interface IInstaller
{
    public void InstallServices(IServiceCollection serviceCollection, IConfiguration configuration);
}