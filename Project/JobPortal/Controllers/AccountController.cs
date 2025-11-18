using JobPortal.Models;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<AppUser> um, SignInManager<AppUser> sm, IConfiguration configuration)
    {
        userManager = um;
        signInManager = sm;
        _configuration = configuration;
    }

    private string GenerateJwtToken(AppUser user, IList<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim("FullName", user.FullName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpGet("/Account/Register")]
    public IActionResult Register()
    {
        ViewBag.Roles = new[] { "Candidate", "Employer" };
        return View();
    }

    [HttpPost("/Account/Register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = new[] { "Candidate", "Employer" };
            return View(model);
        }

        var user = new AppUser
        {
            FullName = model.FullName,
            Email = model.Email,
            UserName = model.Email,
            Role = model.Role
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            if (!await userManager.IsInRoleAsync(user, model.Role))
                await userManager.AddToRoleAsync(user, model.Role);

            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        ViewBag.Roles = new[] { "Candidate", "Employer" };
        return View(model);
    }

    [HttpGet("/Account/Login")]
    public IActionResult Login(string? returnUrl)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost("/Account/Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Невірний логін або пароль");
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(
            user, model.Password, false, false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Невірний логін або пароль");
            return View(model);
        }

        return Redirect(model.ReturnUrl ?? "/Account/Profile");
    }

    [HttpPost("/api/Account/login")]
    public async Task<IActionResult> LoginApi([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized(new { Message = "Invalid login or password" }); // 401
        }

        var roles = await userManager.GetRolesAsync(user);
        var tokenString = GenerateJwtToken(user, roles);

        return Ok(new { Token = tokenString });
    }

    [Authorize]
    [HttpGet("/Account/Logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet("/Account/Profile")]
    public async Task<IActionResult> Profile()
    {
        var user = await userManager.GetUserAsync(User);

        var vm = new ProfileViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };

        return View(vm);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("/api/Account/profile-api")] 
    public async Task<IActionResult> ProfileApi()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userManager.FindByIdAsync(userId!);

        if (user == null)
            return NotFound();

        var vm = new ProfileViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };

        return Ok(vm);
    }

    [Authorize]
    [HttpPost("/Account/UpdateProfile")]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
    {
        var user = await userManager.FindByIdAsync(model.Id);
        if (user == null) return NotFound();

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;

        await userManager.UpdateAsync(user);

        return RedirectToAction("Profile");
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("profile-update-api")] 
    public async Task<IActionResult> UpdateProfileApi([FromBody] ProfileViewModel model)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (model.Id != userId)
            return Forbid(); // 403 Forbidden: користувач не може оновити чужий профіль

        var user = await userManager.FindByIdAsync(model.Id);
        if (user == null) return NotFound();

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;

        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Ok(new { Message = "Profile updated successfully" });

        return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
    }
}