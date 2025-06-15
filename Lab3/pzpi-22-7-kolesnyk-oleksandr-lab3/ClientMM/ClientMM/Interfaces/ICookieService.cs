namespace ClientMM.Interfaces
{
    public interface ICookieService
    {
        Task SetCookie(string name, string value);
        string GetCookie(string name);
    }
}
