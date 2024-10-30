namespace PSAM.Services.IServices
{
    public interface IAuthService
    {
        public int? GetUserIdFromToken();
        public int? GetUserRoleFromToken();
    }
}