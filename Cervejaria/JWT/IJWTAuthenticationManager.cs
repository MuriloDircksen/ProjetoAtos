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
        //List<Usuario> users = new List<Usuario>();
       
        public JWTAuthenticationManager(CervejariaContexto contexto)
        {
            _contexto = contexto;
            //users = _contexto.Usuarios.ToList();
        }               
        public bool GetUser(string username, string password)
        {
            CervejariaContexto _contexto = new CervejariaContexto();
            Usuario user = _contexto.Usuarios.FirstOrDefault(u => u.Email == username );
            if (user == null || user.Senha != password) 
            {
                return false;
            }
            return false;
        }
        

        private readonly string tokenKey;

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }
        public string Authenticate(string username, string password)
        {
            
            //no nosso projeto, substituir por consulta no banco
            if (this.GetUser(username, password)) //verifica e existe um usuário na dictonary
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);

            var tokenDescriptor = new SecurityTokenDescriptor // dita comportamento do token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
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
