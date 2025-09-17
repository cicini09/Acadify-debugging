    using Student_Performance_Tracker.Services;
    using System.Text.Json;
    
    public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public SendGridEmailService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task SendPasswordResetEmailAsync(string email, string userName, string resetToken)
    {
        var apiKey = _configuration["EmailSettings:SendGridApiKey"];
        var fromEmail = _configuration["EmailSettings:FromEmail"];
        var fromName = _configuration["EmailSettings:FromName"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(fromEmail))
        {
            throw new InvalidOperationException("SendGrid configuration is missing. Please check EmailSettings in appsettings.json");
        }

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        var baseUrl = _configuration["EmailSettings:BaseUrl"] ?? "https://localhost:5001";
        var resetUrl = $"{baseUrl}/Account/ResetPassword?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(resetToken)}";

        var emailData = new
        {
            personalizations = new[]
            {
                    new
                    {
                        to = new[] { new { email = email, name = userName } },
                        subject = "Password Reset - Student Performance Tracker"
                    }
                },
            from = new { email = fromEmail, name = fromName },
            content = new[]
            {
                    new
                    {
                        type = "text/html",
                        value = CreateEmailBody(userName, resetUrl)
                    }
                }
        };

        var json = JsonSerializer.Serialize(emailData, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.sendgrid.com/v3/mail/send", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to send email via SendGrid: {response.StatusCode} - {errorContent}");
        }
    }

    private static string CreateEmailBody(string userName, string resetUrl)
    {
        return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Password Reset</title>
                <style>
                    body {{ 
                        font-family: Arial, sans-serif;
                        line-height: 1.6; 
                        color: #333;
                        margin: 0;
                        padding: 0;
                        background-color: #f4f4f4;
                    }}
                    .container {{ 
                        max-width: 600px; 
                        margin: 40px auto; 
                        background-color: white;
                        border-radius: 8px;
                        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                        overflow: hidden;
                    }}
                    .header {{ 
                        background-color: #007bff;
                        color: white; 
                        padding: 40px 30px; 
                        text-align: center; 
                    }}
                    .header h1 {{
                        margin: 0;
                        font-size: 28px;
                        font-weight: 600;
                    }}
                    .content {{ 
                        padding: 40px 30px; 
                    }}
                    .content h2 {{
                        color: #333;
                        font-size: 24px;
                        margin-bottom: 20px;
                    }}
                    .content p {{
                        font-size: 16px;
                        margin-bottom: 20px;
                        color: #666;
                    }}
                    .button-container {{
                        text-align: center;
                        margin: 30px 0;
                    }}
                    .button {{ 
                        display: inline-block; 
                        background-color: #007bff;
                        color: white; 
                        padding: 15px 30px; 
                        text-decoration: none; 
                        border-radius: 5px; 
                        font-weight: 600;
                        font-size: 16px;
                    }}
                    .warning {{
                        background-color: #fff3cd;
                        border: 1px solid #ffeaa7;
                        color: #856404;
                        padding: 15px;
                        border-radius: 5px;
                        margin: 20px 0;
                    }}
                    .footer {{ 
                        background-color: #f8f9fa;
                        text-align: center; 
                        padding: 30px; 
                        color: #6c757d; 
                        font-size: 14px;
                        border-top: 1px solid #e9ecef;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🎓 Student Performance Tracker</h1>
                    </div>
                    <div class='content'>
                        <h2>Password Reset Request</h2>
                        <p>Hello <strong>{userName}</strong>,</p>
                        <p>We received a request to reset your password for your Student Performance Tracker account. Click the button below to create a new password:</p>
                        
                        <div class='button-container'>
                            <a href='{resetUrl}' class='button'>Reset My Password</a>
                        </div>
                        
                        <div class='warning'>
                            <strong>⚠️ Important:</strong>
                            <ul style='margin: 10px 0; padding-left: 20px;'>
                                <li>This link will expire in <strong>24 hours</strong> for security reasons</li>
                                <li>If you didn't request a password reset, please ignore this email</li>
                                <li>Never share this link with anyone</li>
                            </ul>
                        </div>
                        
                        <p style='font-size: 14px; color: #999; margin-top: 30px;'>
                            If the button doesn't work, copy and paste this link into your browser:<br>
                            <a href='{resetUrl}' style='color: #007bff; word-break: break-all;'>{resetUrl}</a>
                        </p>
                        
                        <p>Best regards,<br><strong>Student Performance Tracker Team</strong></p>
                    </div>
                    <div class='footer'>
                        <p>This is an automated email. Please do not reply to this message.</p>
                    </div>
                </div>
            </body>
            </html>";
    }
}