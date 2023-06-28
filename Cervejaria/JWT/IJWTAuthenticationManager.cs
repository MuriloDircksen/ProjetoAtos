using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cervejaria.JWT
{
    public interface IJWTAuthenticationManager
    {
        string Authenticate(string email, string senha);
    }
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly CervejariaContexto _contexto;
       // private bool teste = false;
        //List<Usuario> users = new List<Usuario>();
       
        public JWTAuthenticationManager(string token, CervejariaContexto contexto)
        {
            _contexto = contexto;
            this.tokenKey = token;
        }               
        public async Task<bool> GetUser(string email, string senha)
        {
            //CervejariaContexto _contexto = new CervejariaContexto();
            //Usuario user = _contexto.Usuarios.FirstOrDefault(u => u.Email == email );
            List<Usuario> users = await _contexto.Usuarios.ToListAsync();
            //users =await _contexto.Usuarios.ToListAsync();

            foreach (var user in users)
            {
                if (user.Email == email && user.Senha == senha)
                {
                   return true;
                }
            }
            
            return false;
        }
        

        private readonly string tokenKey;

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }
        public string Authenticate(string email, string senha)
        {
            
            if (!this.GetUser(email, senha).Result) 
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);

            var tokenDescriptor = new SecurityTokenDescriptor // dita comportamento do token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
