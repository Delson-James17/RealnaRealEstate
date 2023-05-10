namespace Real_Estate.Services
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}