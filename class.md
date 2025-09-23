using ASI.Basecode.WebApp.Extensions.Configuration;
using ASI.Basecode.Resources.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// CustomJwtDataFormat
    /// </summary>
    public class CustomJwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string algorithm;
        private readonly TokenValidationParameters validationParameters;
        private readonly TokenProviderOptionsFactory _tokenProviderOptionsFactory;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the CustomJwtDataFormat class.
        /// </summary>
        /// <param name="algorithm">Algorithm</param>
        /// <param name="validationParameters">Validation parameters</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="tokenProviderOptionsFactory">Token provider options factory</param>
        public CustomJwtDataFormat(string algorithm, TokenValidationParameters validationParameters, IConfiguration configuration, TokenProviderOptionsFactory tokenProviderOptionsFactory)
        {
            this.algorithm = algorithm;
            this.validationParameters = validationParameters;
            this._tokenProviderOptionsFactory = tokenProviderOptionsFactory;
            this._configuration = configuration;
        }

        /// <summary>
        /// Unprotects the specified <paramref name="protectedText" />.
        /// </summary>
        /// <param name="protectedText">Data protected value.</param>
        /// <returns>
        /// An instance of AuthenticationTicket.
        /// </returns>
        public AuthenticationTicket Unprotect(string protectedText)
            => Unprotect(protectedText, null);

        /// <summary>
        /// Unprotects the specified <paramref name="protectedText" /> using the specified <paramref name="purpose" />.
        /// </summary>
        /// <param name="protectedText">Data protected value</param>
        /// <param name="purpose">Purpose</param>
        /// <returns>
        /// An instance of AuthenticationTicket.
        /// </returns>
        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken validToken = null;

            try
            {
                principal = handler.ValidateToken(protectedText, this.validationParameters, out validToken);

                var validJwt = validToken as JwtSecurityToken;

                if (validJwt == null)
                {
                    throw new ArgumentException("Invalid JWT");
                }

                if (!validJwt.Header.Alg.Equals(algorithm, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be '{algorithm}'");
                }
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            var token = _configuration.GetTokenAuthentication();
            return new AuthenticationTicket(principal, new AuthenticationProperties(), Const.AuthenticationScheme);
        }

        /// <summary>
        /// Protects the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>A data protected value</returns>
        public string Protect(AuthenticationTicket ticket)
            => Protect(ticket, null);

        /// <summary>
        /// Protects the specified ticket.
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <param name="purpose">Purpose</param>
        /// <returns>Encoded jwt token</returns>
        public string Protect(AuthenticationTicket ticket, string purpose)
        {
            var token = _configuration.GetTokenAuthentication();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.SecretKey));

            var claimsPrincipal = ticket.Principal;
            var identity = (ClaimsIdentity)claimsPrincipal.Identity;
            var tokenProviderOptions = TokenProviderOptionsFactory.Create(token, signingKey);
            var tokenProvider = new TokenProvider(Options.Create(tokenProviderOptions));
            var encodedJwt = tokenProvider.GetJwtSecurityToken(identity, tokenProviderOptions);
            return encodedJwt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ASI.Basecode.WebApp.Extensions.Configuration;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.Resources.Constants;
using ASI.Basecode.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// SignInManager
    /// </summary>
    public class SignInManager
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public LoginUser user { get; set; }

        /// <summary>
        /// Initializes a new instance of the SignInManager class.
        /// </summary>
        public SignInManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SignInManager class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="accountService">The account service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public SignInManager(IConfiguration configuration,
                             IHttpContextAccessor httpContextAccessor)
        {
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
            user = new LoginUser();
        }

        /// <summary>
        /// Gets the claims identity.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The successfully completed task</returns>
        public Task<ClaimsIdentity> GetClaimsIdentity(string username, string password)
        {
            ClaimsIdentity claimsIdentity = null;
            User userData = new User();

            user.loginResult = LoginResult.Success;//TODO this._accountService.AuthenticateUser(username, password, ref userData);

            if (user.loginResult == LoginResult.Failed)
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }

            user.userData = userData;
            claimsIdentity = CreateClaimsIdentity(userData);
            return Task.FromResult(claimsIdentity);
        }

        /// <summary>
        /// Creates the claims identity.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Instance of ClaimsIdentity</returns>
        public ClaimsIdentity CreateClaimsIdentity(User user)
        {
            var token = _configuration.GetTokenAuthentication();
            //TODO
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId, ClaimValueTypes.String, Const.Issuer),
                new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String, Const.Issuer),

                new Claim("UserId", user.UserId, ClaimValueTypes.String, Const.Issuer),
                new Claim("UserName", user.Name, ClaimValueTypes.String, Const.Issuer),
            };
            return new ClaimsIdentity(claims, Const.AuthenticationScheme);
        }

        /// <summary>
        /// Creates the claims principal.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns>Created claims principal</returns>
        public IPrincipal CreateClaimsPrincipal(ClaimsIdentity identity)
        {
            var identities = new List<ClaimsIdentity>();
            identities.Add(identity);
            return this.CreateClaimsPrincipal(identities);
        }

        /// <summary>
        /// Creates the claims principal.
        /// </summary>
        /// <param name="identities">The identities.</param>
        /// <returns>Created claims principal</returns>
        public IPrincipal CreateClaimsPrincipal(IEnumerable<ClaimsIdentity> identities)
        {
            var principal = new ClaimsPrincipal(identities);
            return principal;
        }

        /// <summary>
        /// Signs in user asynchronously
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        public async Task SignInAsync(User user, bool isPersistent = false)
        {
            var claimsIdentity = this.CreateClaimsIdentity(user);
            var principal = this.CreateClaimsPrincipal(claimsIdentity);
            await this.SignInAsync(principal, isPersistent);
        }

        /// <summary>
        /// Signs in user asynchronously
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        public async Task SignInAsync(IPrincipal principal, bool isPersistent = false)
        {
            var token = _configuration.GetTokenAuthentication();
            await _httpContextAccessor
                .HttpContext
                .SignInAsync(
                            Const.AuthenticationScheme,
                            (ClaimsPrincipal)principal,
                            new AuthenticationProperties
                            {
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(token.ExpirationMinutes),
                                IsPersistent = isPersistent,
                                AllowRefresh = false
                            });
        }

        /// <summary>
        /// Signs out user asynchronously
        /// </summary>
        public async Task SignOutAsync()
        {
            var token = _configuration.GetTokenAuthentication();
            await _httpContextAccessor.HttpContext.SignOutAsync(Const.AuthenticationScheme);
        }
    }
}

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// TokenProvider
    /// </summary>
    public class TokenProvider
    {
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Initializes a new instance of the TokenProvider class.
        /// </summary>
        /// <param name="options">The options.</param>
        public TokenProvider(IOptions<TokenProviderOptions> options)
        {
            _options = options.Value;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// Gets the JWT security token.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <returns></returns>
        public string GetJwtSecurityToken(ClaimsIdentity identity, TokenProviderOptions tokenProvider)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                                            issuer: tokenProvider.Issuer,
                                            audience: tokenProvider.Audience,
                                            claims: identity.Claims,
                                            notBefore: now,
                                            expires: now.Add(_options.Expiration),
                                            signingCredentials: tokenProvider.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler()
                            .WriteToken(jwt);

            return encodedJwt;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// Adds a token generation endpoint to an application pipeline.
    /// </summary>
    public static class TokenProviderAppBuilderExtensions
    {
        /// <summary>
        /// Adds the TokenProviderMiddleware"/> middleware to the specified IApplicationBuilder"/>, which enables token generation capabilities.
        /// <param name="app">The IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A  TokenProviderOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// </summary>
        public static IApplicationBuilder UseTokenProvider(this IApplicationBuilder app, TokenProviderOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// Token Provider Middleware
    /// </summary>
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly TokenProvider _tokenProvider;

        /// <summary>
        /// Initializes a new instance of the TokenProviderMiddleware class.
        /// </summary>
        /// <param name="next">Request</param>
        /// <param name="options">Options</param>
        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options)
        {
            _next = next;
            _options = options.Value;
            _tokenProvider = new TokenProvider(Options.Create(_options));

            ThrowIfInvalidOptions(_options);
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Task</returns>
        public Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST"))
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            return GenerateTokenUser(context);
        }

        /// <summary>
        /// Generates the user token
        /// </summary>
        /// <param name="context">HttpContext</param>
        private async Task GenerateTokenUser(HttpContext context)
        {
            string encodedJwt;
            var now = DateTime.UtcNow;
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];
            var identity = await _options.IdentityResolver(username, password);

            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, await _options.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)
            };

            identity.AddClaims(claims);
            encodedJwt = _tokenProvider.GetJwtSecurityToken(identity, _options);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }

        /// <summary>
        /// Throws if invalid options.
        /// </summary>
        /// <param name="options">The options.</param>
        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
            }
        }

        /// <summary>
        /// Converts to unix epoch date.
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>Parsed date</returns>
        public static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }
}

using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// Token provider
    /// </summary>
    public class TokenProviderOptions
    {
        /// <summary>
        /// Gets or sets the path for token generation.
        /// </summary>
        public string Path { get; set; } = "api/token";
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);
        /// <summary>
        /// Gets or sets the signing credentials.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
        /// <summary>
        /// Gets or sets the identity resolver.
        /// </summary>
        public Func<string, string, Task<ClaimsIdentity>> IdentityResolver { get; set; }
        /// <summary>
        /// Gets or sets the nonce generator.
        /// </summary>
        public Func<Task<string>> NonceGenerator { get; set; }
          = () => Task.FromResult(Guid.NewGuid().ToString());
    }
}

using ASI.Basecode.WebApp.Models;
using ASI.Basecode.Resources.Constants;
using Microsoft.IdentityModel.Tokens;
using System;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// Token provider factory
    /// </summary>
    public class TokenProviderOptionsFactory
    {
        /// <summary>
        /// Creates the token
        /// </summary>
        /// <param name="token">Token authentication</param>
        /// <param name="signingKey">Signing key</param>
        /// <returns>Token Provider Options</returns>
        public static TokenProviderOptions Create(TokenAuthentication token, SymmetricSecurityKey signingKey)
        {
            var options = new TokenProviderOptions
            {
                Path = token.TokenPath,
                Audience = token.Audience,
                Issuer = Const.Issuer,
                Expiration = TimeSpan.FromMinutes(token.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = new SignInManager(null, null).GetClaimsIdentity,
            };

            return options;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using ASI.Basecode.WebApp.Extensions.Configuration;
using ASI.Basecode.Resources.Constants;

namespace ASI.Basecode.WebApp.Authentication
{
    /// <summary>
    /// Token Validation
    /// </summary>
    public class TokenValidationParametersFactory
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="configuration"></param>
        public TokenValidationParametersFactory(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Creates token validation instance
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters Create()
        {
            var tokenAuthentication = this._configuration.GetTokenAuthentication();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenAuthentication.SecretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,

                //issuer signing key
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,

                ValidIssuer = Const.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,

                ValidAudience = tokenAuthentication.Audience,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            return tokenValidationParameters;
        }
    }
}


