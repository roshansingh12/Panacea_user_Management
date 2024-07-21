using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Panacea_User_Management;
using Panacea_User_Management.Container;
using Panacea_User_Management.Models;
using Panacea_User_Management.Panacea_DbContext;
using Panacea_User_Management.Services;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IRegister, _Register>();
builder.Services.AddScoped<ILogin, Login>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Panacea_User_Management_DbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DbConnection")
    ));
builder.Services.AddLogging();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddIdentity<User_Model, IdentityRole>().
    AddEntityFrameworkStores<Panacea_User_Management_DbContext>().
    AddDefaultTokenProviders();

//Adding authentication JWT
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    )
    .AddJwtBearer(
            options =>
            {
                //options.SaveToken = true;
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]
                        ))
                };
            }
    );

//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.SameAsRequest,
    MinimumSameSitePolicy=SameSiteMode.Strict
}) ;
app.UseRouting();
app.UseJwtCookieMiddleware();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "Register",
    pattern: "{controller=Home}/{action=Register}"
    );

app.MapControllerRoute(
    name: "Login",
    pattern: "{controller=Home}/{action=Login}"
    );

app.MapControllerRoute(
    name: "LoggedIn",
    pattern: "{controller=Home}/{action=LoggedIn}"
    );
app.MapControllerRoute(
    name: "Dashboard",
    pattern: "{controller=Admin}/{action=Dashboard}"
    );
app.MapControllerRoute(
    name: "User_List",
    pattern: "{controller=Admin}/{action=User_List}"
    );
/*app.MapControllerRoute(
    name: "Update_user",
    pattern: "{controller=Admin}/{action=Update_user}"
    );*/
app.MapControllerRoute(
    name: "Delete",
    pattern: "{controller=Admin}/{action=Delete}/{userid?}"
    );
app.Run();
