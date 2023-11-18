using System.Security.Cryptography;
using amazonCloneWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace amazonCloneWebAPI.Handler;
public class RefreshTokenGenerator: IRefreshTokenGenerator{
    private readonly AmazonCloneContext _DBContext;
    public RefreshTokenGenerator(AmazonCloneContext dbContext){
        _DBContext = dbContext;
    }
    public async Task<string> GenerateToken(string username){
        var randomnumber = new byte[32];
        using(var randomnumbergenerator = RandomNumberGenerator.Create()){
            randomnumbergenerator.GetBytes(randomnumber);
            string refreshToken = Convert.ToBase64String(randomnumber);
            var user = await _DBContext.Users.FirstOrDefaultAsync(item => item.UsrUserName == username);
            var token = await _DBContext.RefreshTokens.FirstOrDefaultAsync(item => item.TknUserId == user.UsrUserId);
            if(token!=null){
                token.TknRfrshToken = refreshToken;
            }else{
                _DBContext.RefreshTokens.Add(new RefreshToken(){
                    TknUserId = user.UsrUserId,
                    TknTokenId = new Random().Next().ToString(),
                    TknRfrshToken = refreshToken,
                    TknIsActive = true
                });
            }
            await _DBContext.SaveChangesAsync();
            
            return refreshToken;

        }
    }
}