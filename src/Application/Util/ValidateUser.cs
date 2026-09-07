using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Util;

public static class ValidateUser
{
    public static async Task<Result<User>> ValidateAsync
        (
            Guid userId,
            IUserContext userContext,
            IApplicationDbContext context,
            CancellationToken? cancellationToken = null
        )
    {
        if (userId != userContext.UserId)
        {
            return Result<User>.ValidationFailure(UserErrors.Unauthorized());
        }

        User? user = await context.Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken ?? CancellationToken.None);

        if (user is null)
        {
            return Result<User>.ValidationFailure(UserErrors.NotFound(userId));
        }

        return Result<User>.Success(user);
    }
}
