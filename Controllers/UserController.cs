using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using amazonCloneWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using amazonCloneWebAPI.Handler;

[ApiController]
[Route("[controller]")]

public class UserController: ControllerBase
{
    private readonly AmazonCloneContext _DBContext;
    private readonly JwtSettings _JwtSettings;

    private readonly IRefreshTokenGenerator _rfrshTknGen;

    public UserController(AmazonCloneContext dbContext, IOptions<JwtSettings> jwtsettings, IRefreshTokenGenerator rfrshTknGen){
        _DBContext = dbContext;
        _JwtSettings = jwtsettings.Value;
        _rfrshTknGen = rfrshTknGen; 
    }
    [HttpPost("Authenticate")]
    public async Task<IActionResult>Authenticate([FromBody]UserCreds userCreds){
        
        var user = await _DBContext.Users.FirstOrDefaultAsync(item => item.UsrUserName == userCreds.username &&
        item.UsrPassword == userCreds.password);
        var role = await _DBContext.Roles.FirstOrDefaultAsync(item => item.RoleRoleId == user.UsrRoleId);
        if(user == null)
            return Unauthorized();
        //Generate Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_JwtSettings.securityKey);
        var tokenDesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity( 
                new Claim[] { new Claim(ClaimTypes.Name, user.UsrUserName), new Claim(ClaimTypes.Role, role.RoleRoleName)}
            ), Expires = DateTime.Now.AddMinutes(10),
            NotBefore = DateTime.Now,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDesc);
        string finalToken = tokenHandler.WriteToken(token);
        var response = new TokenResponse(){jwtToken = finalToken, refreshToken= await _rfrshTknGen.GenerateToken(user.UsrUserName)};
        return Ok(response);
    }
    
    [NonAction]
    public async Task<TokenResponse>TokenAuthenticate(string user, Claim[] claims)
    {
        var token = new JwtSecurityToken(
            claims:claims,
            expires: DateTime.Now.AddSeconds(20),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtSettings.securityKey)),
            SecurityAlgorithms.HmacSha256));
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new TokenResponse(){jwtToken = jwtToken, refreshToken = await _rfrshTknGen.GenerateToken(user)};
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult>RefreshToken([FromBody]TokenResponse tknResponse){
        
        //Generate Token
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(tknResponse.jwtToken, new TokenValidationParameters{
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtSettings.securityKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        }, out securityToken);
        var token = securityToken as JwtSecurityToken;
        if(token!=null && !token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256)){
            return Unauthorized();
        }
        var username = principal.Identity?.Name;
        var user = await _DBContext.Users.FirstOrDefaultAsync(item => item.UsrUserName == username);
        var tknDet = await _DBContext.RefreshTokens.FirstOrDefaultAsync(
            item => item.TknUserId == user.UsrUserId &&
            item.TknRfrshToken == tknResponse.refreshToken
        );
        var role = await _DBContext.Roles.FirstOrDefaultAsync(item => item.RoleRoleId == user.UsrRoleId);
        if(tknDet == null)
            return Unauthorized();
        var response = TokenAuthenticate(username, principal.Claims.ToArray()).Result;

        return Ok(response);
    }
}