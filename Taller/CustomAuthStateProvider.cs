using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;
using Taller.Models;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Obtener el token primero
            var token = await _jsRuntime.InvokeAsync<string>("authManager.getAuthToken");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Obtener el objeto Cliente completo
            var cliente = await _jsRuntime.InvokeAsync<Cliente>("authManager.getCurrentUser");

            if (cliente == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
                new Claim(ClaimTypes.Name, cliente.Nombre),
                new Claim(ClaimTypes.Email, cliente.Correo ?? ""),
                new Claim("Usuario", cliente.Usuario ?? "")
            }, "apiauth_type");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            // Si hay error al deserializar
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task NotifyUserAuthenticationAsync(string token, Cliente cliente)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("authManager.storeUserData", token, cliente);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
                new Claim(ClaimTypes.Name, cliente.Nombre),
                new Claim(ClaimTypes.Email, cliente.Correo ?? ""),
                new Claim("Usuario", cliente.Usuario ?? "")
            }, "apiauth_type");

            var authState = new AuthenticationState(new ClaimsPrincipal(identity));
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        catch
        {
            // Manejar error
        }
    }

    public async Task NotifyUserUpdatedAsync(Cliente cliente)
    {
        await _jsRuntime.InvokeVoidAsync("authManager.updateUserData", cliente);

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
            new Claim(ClaimTypes.Name, cliente.Nombre),
            new Claim(ClaimTypes.Email, cliente.Correo ?? ""),
            new Claim("Usuario", cliente.Usuario ?? "")
        }, "apiauth_type");

        var authState = new AuthenticationState(new ClaimsPrincipal(identity));
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task NotifyUserLogout()
    {
        await _jsRuntime.InvokeVoidAsync("authManager.clearUserData");
        var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }
}