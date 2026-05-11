namespace SmartBus.Application.HelperMethod.EmailBodyBuilder
{
    public static class EmailBodyOTPRequestBuilder
    {
        public static string OTPBodyRequest(string UserName, string OTP)
        {
            return $"""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Reset Password OTP</title>
</head>
<body style="margin:0; padding:0; background-color:#f4f6f8; font-family:Arial, Helvetica, sans-serif;">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" style="padding:40px 0;">
                <table width="600" cellpadding="0" cellspacing="0" style="background-color:#ffffff; border-radius:8px; box-shadow:0 2px 8px rgba(0,0,0,0.1);">
                    
                    <tr>
                        <td style="padding:20px 30px; background-color:#2563eb; color:#ffffff; border-radius:8px 8px 0 0;">
                            <h2 style="margin:0;">Smart Bus</h2>
                        </td>
                    </tr>

                    <tr>
                        <td style="padding:30px;">
                            <p style="font-size:16px; color:#333;">
                                Hello {UserName},
                            </p>

                            <p style="font-size:15px; color:#555;">
                                We received a request to reset your password.
                                Please use the OTP below to proceed:
                            </p>

                            <div style="text-align:center; margin:30px 0;">
                                <span style="
                                    display:inline-block;
                                    padding:15px 30px;
                                    font-size:24px;
                                    font-weight:bold;
                                    letter-spacing:5px;
                                    color:#2563eb;
                                    background-color:#f1f5f9;
                                    border-radius:6px;">
                                    {OTP}
                                </span>
                            </div>

                            <p style="font-size:14px; color:#555;">
                                This OTP is valid for <strong>5 minutes</strong>.
                                If you did not request a password reset, please ignore this email.
                            </p>

                            <p style="font-size:14px; color:#555;">
                                For security reasons, do not share this code with anyone.
                            </p>

                            <p style="font-size:14px; color:#333; margin-top:30px;">
                                Best regards,<br>
                                <strong>IDS-AI Team</strong>
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td style="padding:15px; text-align:center; font-size:12px; color:#888; background-color:#f9fafb; border-radius:0 0 8px 8px;">
                            © {DateTime.UtcNow.Year} Smart Bus. All rights reserved.
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>
</html>
""";
        }
    }
}
