namespace amazonCloneWebAPI.Handler;
public interface IRefreshTokenGenerator{
    Task<string> GenerateToken(string userName);

}