using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using UsersEditor.Models;
using UsersEditor.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/accessdenied";
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();


app.UseRouting();
app.UseAuthorization();
app.MapControllers();


app.MapGet("/accessdenied", async (HttpContext context) =>
{
    context.Response.StatusCode = 403;
    await context.Response.WriteAsync("Access Denied");
});

app.MapPost("/login", async (string? login, string? password, HttpContext context) =>
{
    // ������� ������������ 
    User? user = await UserRepository.Find(login, password);
    // ���� ������������ �� ������, ���������� ��������� ��� 401
    if (user is null) return Results.Unauthorized();
    //������������ ������������
    else if (user.RevokedOn > DateTime.MinValue) return Results.Unauthorized();

    var claims = new List<Claim>
    {
        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Admin == true ? "admin" : "user")
    };
    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    await context.SignInAsync(claimsPrincipal);
    return Results.Ok();
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok("Data deleted");
});

app.Run();
