using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using FinanceManager.Business.Exceptions;
using FinanceManager.Business.Utils;
using FinanceManager.Data;
using FinanceManager.Data.Enums;
using FinanceManager.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinanceManager.Business.Services;

public class AuthenticationService(
    IConfiguration configuration,
    FinanceManagerDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ILogger<AuthenticationService> logger,
    ISmtpEmailService emailService)
    : IAuthenticationService
{
    private const string SALT_SETTINGS_KEY = "Security:Salt";

    /// <summary>
    /// Registers a new user by creating a user account, assigning a default role,
    /// generating an email confirmation token, and sending a confirmation email.
    /// </summary>
    /// <param name="model">An object containing the user's registration details, such as email, first name, and last name.</param>
    /// <returns>
    /// An <see cref="IdentityResult"/> object representing the success or failure of the user creation process.
    /// Returns a success result if the user is created and the confirmation email is sent successfully.
    /// Returns a failure result if the email is already registered, the user creation fails, or if an unexpected error occurs.
    /// </returns>
    public async Task<IdentityResult> CreateUser(RegisterUserDTO model)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(model.EmailAddress);
            if (existingUser != null)
            {
                logger.LogWarning("Attempt to register with an existing email: {Email}", model.EmailAddress);
                return IdentityResult.Failed(new IdentityError
                    { Description = "Registration failed. Please try again." });
            }

            var user = new User()
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = false,
                LockoutEnabled = true,
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                logger.LogError("Failed to create user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return result;
            }

            // give all users the user role by default
            var roleName = Roles.User.StringValue();
            if (await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await userManager.AddToRoleAsync(user, roleName);
                if (!roleResult.Succeeded)
                {
                    logger.LogError("Failed to add user to role '{Role}': {Errors}", roleName,
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    return roleResult;
                }
            }

            // Generate and store the email confirmation token
            var confirmationToken =
                await userManager.GenerateUserTokenAsync(user, "EmailConfirmationTokenProvider", "EmailConfirmation");
            await userManager.SetAuthenticationTokenAsync(user,
                "EmailConfirmationTokenProvider", "EmailConfirmation", confirmationToken);

            // Generate the confirmation link and send mail
            var confirmationLink = GenerateEmailConfirmationLink(confirmationToken);
            await emailService.SendEmailAsync(user.Email, "Confirm your email address",
                $"Please confirm your email by clicking the following link: {confirmationLink}");

            logger.LogInformation("User registered successfully and confirmation email sent to {Email}", user.Email);

            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred during user registration.");
            return IdentityResult.Failed(new IdentityError
                { Description = "An error occurred while processing your request. Please try again later." });
        }
    }

    /// <summary>
    /// Resends an email confirmation link to a user if their email is not yet confirmed.
    /// Generates a new confirmation token and sends it via email to the specified address.
    /// </summary>
    /// <param name="model">An object containing the email address for which the confirmation link is to be resent.</param>
    /// <returns>
    /// An <see cref="IdentityResult"/> object representing the outcome of the operation.
    /// Returns a success result if the email confirmation link is successfully sent or if the email is already confirmed.
    /// Returns a failure result if the email sending process fails due to an error.
    /// </returns>
    public async Task<IdentityResult> ResendEmailConfirmation(ResendEmailConfirmationDTO model)
    {
        logger.LogInformation("Resend email confirmation request received for email: {Email}", model.EmailAddress);

        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user == null)
        {
            logger.LogWarning("Resend email confirmation attempted for non-existent email: {Email}",
                model.EmailAddress);
            return IdentityResult.Success;
        }

        if (user.EmailConfirmed)
        {
            logger.LogInformation("Email confirmation attempt for already confirmed email: {Email}",
                model.EmailAddress);
            return IdentityResult.Success;
        }

        // Generate a new email confirmation token
        var token = await userManager.GenerateUserTokenAsync(user, "EmailConfirmationTokenProvider",
            "EmailConfirmation");

        // Generate the email confirmation link
        var confirmationLink = GenerateEmailConfirmationLink(token);

        // Send the confirmation email
        try
        {
            await emailService.SendEmailAsync(user.Email!, "Confirm your email address",
                $"Please confirm your email by clicking the following link: {confirmationLink}");
            logger.LogInformation("Confirmation email successfully sent to: {Email}", user.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send confirmation email to: {Email}", user.Email);
            return IdentityResult.Failed(new IdentityError
                { Description = "Failed to send email. Please try again later." });
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// Confirms a user's email address using a token provided in the confirmation process.
    /// Validates the token and updates the user's email status if the token is valid.
    /// </summary>
    /// <param name="model">An object containing the token used for confirming the user's email address.</param>
    /// <returns>
    /// An <see cref="IdentityResult"/> object representing the outcome of the email confirmation process.
    /// Returns a success result if the email is confirmed successfully.
    /// Returns a failure result if the token is invalid, expired, or if the email confirmation process fails.
    /// </returns>
    public async Task<IdentityResult> ConfirmEmailAddress(ConfirmEmailAddressDTO model)
    {
        // Find the associated user
        var userToken = await context.UserTokens
            .SingleOrDefaultAsync(ut =>
                ut.LoginProvider == "EmailConfirmationTokenProvider" &&
                ut.Name == "EmailConfirmation" &&
                ut.Value == model.Token);

        if (userToken == null)
        {
            logger.LogWarning("Invalid or expired email confirmation token.");
            return IdentityResult.Failed(new IdentityError { Description = "Invalid confirmation token." });
        }

        // Retrieve the user using the UserId from the token entry
        var user = await userManager.FindByIdAsync(userToken.UserId.ToString());
        if (user == null)
        {
            logger.LogWarning("User not found for the provided email confirmation token.");
            return IdentityResult.Failed(new IdentityError { Description = "Invalid confirmation token." });
        }

        // Verify the token for the user
        var isTokenValid =
            await userManager.VerifyUserTokenAsync(user, "EmailConfirmationTokenProvider", "EmailConfirmation",
                model.Token);
        if (!isTokenValid)
        {
            logger.LogWarning("Token verification failed.");
            return IdentityResult.Failed(new IdentityError { Description = "Invalid or expired confirmation token." });
        }

        // Step 4: Confirm the user's email
        var result = await userManager.ConfirmEmailAsync(user, model.Token);
        if (result.Succeeded)
        {
            logger.LogInformation("Email successfully confirmed for user: {UserId}", user.Id);
        }
        else
        {
            logger.LogError("Email confirmation failed for user: {UserId}", user.Id);
        }

        return result;
    }

    /// <summary>
    /// Initiates the password reset process for a user by generating a reset token and sending a password reset link to the user's email address.
    /// </summary>
    /// <param name="model">An object containing the email address of the user requesting the password reset.</param>
    /// <returns>
    /// An <see cref="IdentityResult"/> object representing the outcome of the password reset initiation process.
    /// Returns a success result if the password reset email is successfully sent or if the email does not exist.
    /// Returns a failure result if the email is not confirmed or if the email sending process fails.
    /// </returns>
    public async Task<IdentityResult> ForgotPassword(ForgotPasswordDTO model)
    {
        // Log that a forgot password request was received
        logger.LogInformation("Forgot password request received for email: {Email}", model.EmailAddress);

        // Find the user by email
        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user == null)
        {
            logger.LogWarning("Forgot password attempt for non-existent email: {Email}", model.EmailAddress);
            return IdentityResult.Success;
        }

        // Check if the email is confirmed
        if (!user.EmailConfirmed)
        {
            logger.LogWarning("Password reset attempt for unconfirmed email: {Email}", model.EmailAddress);
            return IdentityResult.Failed(new IdentityError
                { Description = "Email must be confirmed before resetting the password." });
        }

        // Generate a password reset token
        var token = await userManager.GenerateUserTokenAsync(user, "PasswordResetTokenProvider", "ResetPassword");

        // Generate the password reset link
        var resetLink = GeneratePasswordResetLink(token);

        // Send the reset password email
        try
        {
            await emailService.SendEmailAsync(user.Email!, "Reset your password",
                $"Please reset your password by clicking the following link: {resetLink}");
            logger.LogInformation("Password reset email successfully sent to: {Email}", user.Email);
        }
        catch (Exception ex)
        {
            // Log any exception that occurred during the sending of the email
            logger.LogError(ex, "Failed to send password reset email to: {Email}", user.Email);
            return IdentityResult.Failed(new IdentityError
                { Description = "Failed to send email. Please try again later." });
        }

        // Return success after all operations are complete
        return IdentityResult.Success;
    }

    /// <summary>
    /// Resets a user's password using a token generated in the password reset process.
    /// Validates the token and updates the user's password if the token is valid.
    /// </summary>
    /// <param name="model">An object containing the token, new password, and user information required to reset the password.</param>
    /// <returns>
    /// An <see cref="IdentityResult"/> object representing the outcome of the password reset process.
    /// Returns a success result if the password is reset successfully.
    /// Throws an <see cref="InvalidTokenException"/> if the token is invalid or expired.
    /// Throws a <see cref="UserNotFoundException"/> if the user cannot be found.
    /// Returns a failure result if the password reset fails for any other reason.
    /// </returns>
    public async Task<IdentityResult> ResetPassword(ResetPasswordDTO model)
    {
        logger.LogInformation("Password reset request received for token: {Token}", model.Token);

        var userToken = await context.UserTokens
            .SingleOrDefaultAsync(ut =>
                ut.LoginProvider == "ResetPasswordTokenProvider" &&
                ut.Name == "ResetPassword" &&
                ut.Value == model.Token);

        if (userToken == null)
        {
            logger.LogWarning("Invalid password reset token: {Token}", model.Token);
            throw new InvalidTokenException("Invalid password reset token.");
        }

        var user = await userManager.FindByIdAsync(userToken.UserId.ToString());
        if (user == null)
        {
            logger.LogError("User not found for ID: {UserId}", userToken.UserId);
            throw new UserNotFoundException("User not found.");
        }

        logger.LogInformation("Resetting password for user with email: {Email}", user.Email);

        var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to reset password for user with email: {Email}. Errors: {Errors}",
                user.Email,
                string.Join(", ", result.Errors.Select(e => e.Description)));
            return result;
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// Authenticates a user by validating their email and password, and generates access and refresh tokens for them.
    /// If authentication is successful, the refresh token is stored securely, and both tokens are returned.
    /// </summary>
    /// <param name="model">An object containing the user's login credentials, including email address and password.</param>
    /// <returns>
    /// An <see cref="AuthorizationTokenDTO"/> object containing the access token and refresh token if authentication is successful.
    /// Throws a <see cref="UserNotFoundException"/> if the user is not found or if the password is incorrect.
    /// </returns>
    public async Task<AuthorizationTokenDTO?> LoginUser(LoginUserDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new UserNotFoundException("User not found.");
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await userManager.GenerateUserTokenAsync(user, "RefreshTokenProvider", "RefreshToken");

        await userManager.SetAuthenticationTokenAsync(user,
            "RefreshTokenProvider",
            "RefreshToken",
            SecurityUtils.HashValue(refreshToken, configuration[SALT_SETTINGS_KEY] ?? string.Empty));

        return new AuthorizationTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Refreshes a user's access token using a valid refresh token. If the refresh token is valid, a new access token
    /// and refresh token are generated and returned. The old refresh token is replaced with a new one.
    /// </summary>
    /// <param name="model">An object containing the refresh token used to authenticate the request.</param>
    /// <returns>
    /// An <see cref="AuthorizationTokenDTO"/> object containing the new access token and refresh token if the refresh operation is successful.
    /// Throws an <see cref="InvalidTokenException"/> if the refresh token is invalid or expired.
    /// Throws a <see cref="UserNotFoundException"/> if the user associated with the token is not found.
    /// </returns>
    public async Task<AuthorizationTokenDTO?> RefreshAccessToken(RefreshAccessTokenDTO model)
    {
        var hashedRefreshToken =
            SecurityUtils.HashValue(model.RefreshToken, configuration[SALT_SETTINGS_KEY] ?? string.Empty);

        var userToken = await context.UserTokens
            .SingleOrDefaultAsync(ut =>
                ut.LoginProvider == "RefreshTokenProvider" &&
                ut.Name == "RefreshToken" &&
                ut.Value == hashedRefreshToken);

        if (userToken == null)
        {
            throw new InvalidTokenException("Invalid refresh token.");
        }

        var user = await userManager.FindByIdAsync(userToken.UserId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException("User not found.");
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await userManager.GenerateUserTokenAsync(user, "RefreshTokenProvider", "RefreshToken");

        await userManager.SetAuthenticationTokenAsync(user,
            "RefreshTokenProvider",
            "RefreshToken",
            SecurityUtils.HashValue(refreshToken, configuration[SALT_SETTINGS_KEY] ?? string.Empty));

        return new AuthorizationTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Logs out the user by invalidating all active refresh tokens associated with the user.
    /// Ensures that any existing tokens cannot be used to refresh access tokens after logout.
    /// </summary>
    /// <param name="model">An object containing the user's ID.</param>
    /// <returns>
    /// An <see cref="IdentityResult"/> object representing the outcome of the logout process.
    /// Returns a success result if all tokens are invalidated successfully.
    /// Returns a failure result if the user is not found or if the logout process fails.
    /// </returns>
    public async Task<IdentityResult> LogoutUser(LogoutUserDTO model)
    {
        logger.LogInformation("Logout request received for user ID: {UserId}", model.UserId);

        // Check for a valid user ID
        if (model.UserId == null)
        {
            logger.LogError("User ID is null in logout request.");
            return IdentityResult.Failed(new IdentityError { Description = "Invalid user ID." });
        }
        
        // Retrieve the user by ID
        var user = await userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            logger.LogError("User not found for ID: {UserId}", model.UserId);
            throw new UserNotFoundException("User not found.");
        }

        // Invalidate all refresh tokens for the user by removing them from the database
        var userTokens = context.UserTokens.Where(ut => ut.UserId == user.Id && ut.LoginProvider == "RefreshTokenProvider");

        context.UserTokens.RemoveRange(userTokens);

        try
        {
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully invalidated all refresh tokens for user ID: {UserId}", model.UserId);
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while invalidating tokens for user ID: {UserId}", model.UserId);
            return IdentityResult.Failed(new IdentityError { Description = "An error occurred while logging out. Please try again." });
        }
    }
    
    private string GenerateEmailConfirmationLink(string token)
    {
        var baseUrl = configuration["ApplicationSettings:FrontendBaseUrl"];
        var confirmationPath = configuration["ApplicationSettings:EmailConfirmationPath"];
        var encodedToken = Uri.EscapeDataString(token);
        return $"{baseUrl}{confirmationPath}?token={encodedToken}";
    }

    private string GeneratePasswordResetLink(string token)
    {
        var baseUrl = configuration["ApplicationSettings:FrontendBaseUrl"];
        var confirmationPath = configuration["ApplicationSettings:PasswordResetPath"];
        var encodedToken = Uri.EscapeDataString(token);
        return $"{baseUrl}{confirmationPath}?token={encodedToken}";
    }

    private string GenerateAccessToken(User user)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Authentication:Secret"] ??
                                          throw new ConfigurationException("Authentication Secret can't be null"));
        var issuingOnAt = DateTimeOffset.UtcNow;
        var expiringOnAt =
            issuingOnAt.AddMinutes(Convert.ToDouble(configuration["Authentication:AccessTokenExpirationInMinutes"]));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["Authentication:ValidIssuer"],
            Audience = configuration["Authentication:ValidAudience"],
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            ]),
            Expires = expiringOnAt.UtcDateTime,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateToken(tokenDescriptor);
    }
}