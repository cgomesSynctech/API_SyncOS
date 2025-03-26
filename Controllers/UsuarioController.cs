using info;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Services;
using System.Security.Cryptography;

namespace SyncOS_API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public UsuarioController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpGet("{loginName}/{loginPass}")]
        public async Task<ActionResult<Usuario>> Get(string loginName, string loginPass)
        {
            try
            {
                var usr = new Usuario()
                {
                    Email = loginName,
                    LoginPass= loginPass    
                };


                var nome = loginName.ToLower();
                var senha = ToPassWord.Get(usr);
                var user = await _ctx.Usuarios
                    .FirstOrDefaultAsync(u => u.Email!.ToLower() == nome && u.LoginPass == senha);
                return Ok(user ?? new Usuario());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
