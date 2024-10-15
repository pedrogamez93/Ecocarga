using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Ecocarga.Data;
using Ecocarga.Models;
using Microsoft.AspNetCore.Identity;
using Ecocarga.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// **Elimina esta línea:**
// builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();

// Configurar Identity con roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<ElectrolineraService>();
builder.Services.AddScoped<UserActionService>();  // Inyectar el servicio de acciones de usuario
builder.Services.AddHttpContextAccessor();  // Para obtener el HttpContext y los datos del usuario actual

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Middleware para manejar excepciones globalmente
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        // Registrar el error
        Console.WriteLine($"An error occurred: {ex.Message}");

        // Guardar detalles del error en TempData para mostrarlos en la vista
        context.Response.Redirect($"/Home/Error?message={Uri.EscapeDataString(ex.Message)}");
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar CORS
app.UseCors("AllowAll");

app.UseAuthentication(); // Asegúrate de tener esta línea
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute( name: "electrolineras", pattern: "{controller=Electrolineras}/{action=Index}/{page?}");
    endpoints.MapControllerRoute(name: "baterias",pattern: "{controller=Baterias}/{action=Lista}/{id?}");
    endpoints.MapControllerRoute( name: "useractions",pattern: "{controller=UserAction}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
  
});

// Método para crear roles predeterminados
static async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Administrador", "Usuario", "Visitante" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Llama a la función para crear roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}

app.Run();
