public interface IDataSynchronizer
{
    public void OnSync();
    public void OnDesync();
    public void OnSave();

    public void OnLoad();
}