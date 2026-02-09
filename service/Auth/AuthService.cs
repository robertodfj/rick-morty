using RickYMorty.data;
using RickYMorty.dto.auth;
using RickYMorty.middleware;
using RickYMorty.model;

namespace RickYMorty.service.auth
{
    public class AuthService
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Register(RegisterDTO registerDTO)
        {
            await ValidateUser(registerDTO);
            User user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                Role = "User"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return "User registered successfully";
        }

        public async Task<string> RegisterAdmin(RegisterDTO registerDTO)
        {
            await ValidateUser(registerDTO);
            User user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                Role = "Admin"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return "Admin registered successfully";
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == loginDTO.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                throw new BadRequestException("Invalid username or password");
            }
        
            return "Crear el token aqui"; 
        }

        public async Task<bool> ValidateUser(RegisterDTO registerDTO)
        {
            if (_context.Users.Any(u => u.Username == registerDTO.Username))
            {
                throw new ConflictException("Username already exists");
            }
            if (_context.Users.Any(u => u.Email == registerDTO.Email))
            {
                throw new ConflictException("Email already exists");
            }
            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                throw new BadRequestException("Passwords do not match");
            }
            return true;
        }
    
    }
}