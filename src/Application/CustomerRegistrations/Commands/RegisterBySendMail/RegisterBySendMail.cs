using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Application.Common.Models;

namespace AlphaVisa.Application.CustomerRegistrations.Commands.RegisterBySendEmail;

public record RegisterBySendMailCommand : IRequest<int>
{
    public string? Title { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Description { get; set; }
}

public class RegisterBySendMailCommandValidator : AbstractValidator<RegisterBySendMailCommand>
{
    public RegisterBySendMailCommandValidator(ISharedLocalizer localizer)
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage(localizer["EmailInvalid"]);

        RuleFor(x => x.Phone)
            .Matches(@"^\d+$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage(localizer["PhoneInvalid"]);

        RuleFor(x => x)
            .Must(HaveEitherEmailOrPhone)
            .WithMessage(localizer["EmailOrPhoneRequired"]);
    }

    private bool HaveEitherEmailOrPhone(RegisterBySendMailCommand command)
    {
        return !string.IsNullOrEmpty(command.Email) || !string.IsNullOrEmpty(command.Phone);
    }
}

public class RegisterBySendMailCommandHandler : IRequestHandler<RegisterBySendMailCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMailService _mailService;

    public RegisterBySendMailCommandHandler(IApplicationDbContext context, IMailService mailService)
    {
        _context = context;
        _mailService = mailService;
    }

    public async Task<int> Handle(RegisterBySendMailCommand request, CancellationToken cancellationToken)
    {
        var to = new string[] { request.Email ?? string.Empty };

        await _mailService.SendHtmlEmailAsync(to, request.Title ?? "Booking consultant", GenerateBody(request));

        return 1;
    }

    public string GenerateBody(RegisterBySendMailCommand request)
    {
        return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <style>
            body {{ font-family: 'Helvetica Neue', Arial, sans-serif; color: #333333; background-color: #f4f4f4; margin: 0; padding: 0; }}
            .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1); }}
            /* Header Section */
            .header {{ background-color: #ffffff; padding: 20px; text-align: center; border-bottom: 2px solid #eeeeee; }}
            .header img {{ max-height: 50px; vertical-align: middle; }}
            .header h1 {{ font-size: 24px; color: #004080; margin: 10px 0; }}
            /* Content Section */
            .content {{ padding: 20px; }}
            .content p {{ margin: 10px 0; font-size: 16px; line-height: 1.5; color: #333333; }}
            .content p strong {{ color: #004080; }} /* Dark blue for emphasis */
            .button {{ display: inline-block; margin-top: 20px; padding: 10px 20px; background-color: #004080; color: #ffffff; text-decoration: none; border-radius: 5px; }}
            /* Footer Section */
            .footer {{ margin-top: 20px; padding: 20px; background-color: #004080; text-align: center; color: #ffffff; border-radius: 0 0 8px 8px; }}
            .footer p {{ margin: 0; font-size: 14px; }}
            .footer a {{ color: #ffffff; text-decoration: none; }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <!-- Header Section -->
            <div class=""header"">
                <img src=""https://alphavisa.vn/assets/assets1/images/logo-img.png"" alt=""AlphaVisa"" />
                <h1>{request.Title}</h1>
            </div>
            <!-- Content Section -->
            <div class=""content"">
                <p><strong>Full Name:</strong> {request.FullName}</p>
                <p><strong>Email:</strong> <a href=""mailto:{request.Email}"">{request.Email}</a></p>
                <p><strong>Phone:</strong> {request.Phone}</p>
                <p><strong>Description:</strong></p>
                <p>{request.Description}</p>
                <a href=""https://example.com/confirm-booking"" class=""button"">Confirm Booking</a>
            </div>
            <!-- Footer Section -->
            <div class=""footer"">
                <p>&copy; 2024 AlphaVisa. All rights reserved.</p>
                <p><a href=""https://example.com/unsubscribe"">Unsubscribe</a> | <a href=""https://example.com/privacy-policy"">Privacy Policy</a></p>
            </div>
        </div>
    </body>
    </html>";
    }
}
