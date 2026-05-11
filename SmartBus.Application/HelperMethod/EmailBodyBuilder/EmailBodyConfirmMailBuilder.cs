using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SmartBus.Application.HelperMethod.EmailBodyBuilder
{
    public static class EmailBodyConfirmMailBuilder
    {
        public static string ConfirmMailBody(string confirmLink)
        {
            return $@"
<table border='0' cellpadding='0' cellspacing='0' width='100%' style='background-color: #f4f4f4; padding: 20px; font-family: Helvetica, Arial, sans-serif;'>
    <tr>
        <td align='center'>
            <table border='0' cellpadding='0' cellspacing='0' width='600' style='background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                <tr>
                    <td align='center' style='background-color: #4A90E2; padding: 40px;'>
                        <h1 style='margin: 0; color: #ffffff; font-size: 28px;'>Confirm Your Email</h1>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 40px; color: #333333; line-height: 1.6;'>
                        <p style='margin-top: 0; font-size: 18px; font-weight: bold;'>Hello!</p>
                        <p>We're excited to have you. Please click the button below to verify your email address and activate your account.</p>
                        <table border='0' cellpadding='0' cellspacing='0' style='margin: 30px auto;'>
                            <tr>
                                <td align='center' style='border-radius: 5px; background-color: #4A90E2;'>
                                    <a href='{confirmLink}' target='_blank' style='padding: 15px 25px; border: 1px solid #4A90E2; border-radius: 5px; font-family: Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; font-weight: bold; display: inline-block;'>
                                        Confirm Email Address
                                    </a>
                                </td>
                            </tr>
                        </table>
                        <p>If you did not sign up for this account, you can safely ignore this email.</p>
                        <p style='margin-bottom: 0;'>Best regards,<br><strong>The Team</strong></p>
                    </td>
                </tr>
                <tr>
                    <td align='center' style='padding: 20px; background-color: #eeeeee; color: #999999; font-size: 12px;'>
                        <p style='margin: 0;'>&copy; 2026 Your Company. 123 Street, City, State.</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>";
        }
    }
}
