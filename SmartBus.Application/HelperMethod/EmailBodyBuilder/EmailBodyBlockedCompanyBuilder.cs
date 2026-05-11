using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.HelperMethod.EmailBodyBuilder
{
    public static class EmailBodyBlockedCompanyBuilder
    {
        public static string BlockedCompanyBody(string companyName, DateOnly blockedDate, string reason)
        {
            return  $@"
<table border='0' cellpadding='0' cellspacing='0' width='100%' style='background-color: #f8f9fa; padding: 20px; font-family: Helvetica, Arial, sans-serif;'>
    <tr>
        <td align='center'>
            <table border='0' cellpadding='0' cellspacing='0' width='600' style='background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 4px; overflow: hidden;'>
                <tr>
                    <td align='center' style='background-color: #d9534f; padding: 30px;'>
                        <h1 style='margin: 0; color: #ffffff; font-size: 22px; text-transform: uppercase; letter-spacing: 1px;'>Account Status Alert</h1>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 40px; color: #333333; line-height: 1.6;'>
                        <p style='margin-top: 0; font-size: 16px;'>Dear Administrator,</p>
                        <p>This is an automated notification to inform you that the company profile for <strong>{companyName}</strong> has been <strong>Blocked</strong>.</p>
                        
                        <table border='0' cellpadding='10' cellspacing='0' width='100%' style='background-color: #fff5f5; border-left: 4px solid #d9534f; margin: 20px 0;'>
                            <tr>
                                <td>
                                    <p style='margin: 0; font-size: 14px;'><strong>Status:</strong> Blocked</p>
                                    <p style='margin: 0; font-size: 14px;'><strong>Effective Date:</strong> {blockedDate}</p>
                                    <p style='margin: 0; font-size: 14px;'><strong>Reason:</strong> {reason}</p>
                                </td>
                            </tr>
                        </table>

                        <p>While blocked, users associated with this company will not be able to access their accounts or perform standard operations.</p>
                      
                        <p style='font-size: 13px; color: #666666;'>If you believe this was done in error, please contact the system security team immediately.</p>
                    </td>
                </tr>
                <tr>
                    <td align='center' style='padding: 20px; background-color: #f1f1f1; color: #888888; font-size: 11px;'>
                        <p style='margin: 0;'>Security Notification | System ID: ADMIN-ALERT-88</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>";
        }
    }
}
