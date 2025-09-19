namespace ASI.Basecode.Resources.Templates;

public static class EmailTemplates
{
    public static string GetPasswordResetEmailTemplate(string userName, string resetUrl)
    {
        return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Password Reset</title>
            <script src='https://cdn.tailwindcss.com'></script>
        </head>
        <body class='bg-gray-100 font-sans'>
            <div class='max-w-2xl mx-auto my-10 bg-white rounded-lg shadow-lg overflow-hidden'>
                <!-- Header -->
                <div class='bg-blue-600 text-white text-center py-10 px-8'>
                    <h1 class='text-3xl font-bold'>🎓 Student Performance Tracker</h1>
                </div>
                
                <!-- Content -->
                <div class='p-10'>
                    <h2 class='text-2xl font-semibold text-gray-800 mb-5'>Password Reset Request</h2>
                    
                    <p class='text-gray-600 mb-5 text-base'>Hello <strong class='text-gray-800'>{userName}</strong>,</p>
                    
                    <p class='text-gray-600 mb-8 text-base leading-relaxed'>
                        We received a request to reset your password for your Student Performance Tracker account. 
                        Click the button below to create a new password:
                    </p>
                    
                    <!-- Reset Button -->
                    <div class='text-center my-8'>
                        <a href='{resetUrl}' 
                           class='inline-block bg-blue-600 hover:bg-blue-700 text-white font-semibold py-4 px-8 rounded-lg text-lg transition duration-200 no-underline'>
                            Reset My Password
                        </a>
                    </div>
                    
                    <!-- Warning Box -->
                    <div class='bg-yellow-50 border border-yellow-200 rounded-lg p-4 my-6'>
                        <div class='text-yellow-800'>
                            <strong>⚠️ Important:</strong>
                            <ul class='mt-2 ml-5 space-y-1 list-disc'>
                                <li>This link will expire in <strong>24 hours</strong> for security reasons</li>
                                <li>If you didn't request a password reset, please ignore this email</li>
                                <li>Never share this link with anyone</li>
                            </ul>
                        </div>
                    </div>
                    
                    <!-- Fallback Link -->
                    <div class='mt-8 text-sm text-gray-500'>
                        <p>If the button doesn't work, copy and paste this link into your browser:</p>
                        <p class='break-all mt-2'>
                            <a href='{resetUrl}' class='text-blue-600 hover:text-blue-800 no-underline'>{resetUrl}</a>
                        </p>
                    </div>
                    
                    <p class='mt-8 text-gray-600'>
                        Best regards,<br>
                        <strong class='text-gray-800'>Student Performance Tracker Team</strong>
                    </p>
                </div>
                
                <!-- Footer -->
                <div class='bg-gray-50 border-t border-gray-200 text-center py-6 px-8'>
                    <p class='text-gray-500 text-sm'>This is an automated email. Please do not reply to this message.</p>
                </div>
            </div>
        </body>
        </html>";
    }
}
