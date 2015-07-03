namespace OmniKits
{
    public interface IDataPulse<T> : IWaitData<T>
    {
        T Data { set; }
    }
}
