namespace Tron.DebugClient.Demo.Infrastructure
{
    public interface IMessageBoxDialogService
    {
        bool OpenDialog(string message, string title = null);
    }
}
