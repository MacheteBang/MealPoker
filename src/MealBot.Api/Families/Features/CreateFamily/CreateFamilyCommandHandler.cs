using CrypticWizard.RandomWordGenerator;
using MealBot.Api.Users.Features.GetUser;
using static CrypticWizard.RandomWordGenerator.WordGenerator; //for brevity, not required

namespace MealBot.Api.Families.Features.CreateFamily;

internal sealed class CreateFamilyCommandHandler(
    ILogger<CreateFamilyCommandHandler> logger,
    IValidator<CreateFamilyCommand> validator,
    MealBotDbContext dbContext,
    ISender sender,
    WordGenerator wordGenerator) : IRequestHandler<CreateFamilyCommand, ErrorOr<Family>>
{
    private readonly ILogger<CreateFamilyCommandHandler> _logger = logger;
    private readonly IValidator<CreateFamilyCommand> _validator = validator;
    private readonly MealBotDbContext _dbContext = dbContext;
    private readonly ISender _sender = sender;
    private readonly WordGenerator _wordGenerator = wordGenerator;

    public async Task<ErrorOr<Family>> Handle(CreateFamilyCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var userResult = await _sender.Send(new GetUserQuery(command.UserId), cancellationToken);
        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        var user = userResult.Value;

        if (user.FamilyId.HasValue)
        {
            return Errors.UserAlreadyHasFamily();
        }

        // In an attempt to ensure uniqueness of family codes, we will retry a few times
        int maxAttempts = 5;
        int retryAttempts = 0;
        while (retryAttempts <= maxAttempts)
        {
            retryAttempts++;

            var family = CreateFamilyWithCode(command.Name, command.Description);
            user.FamilyId = family.FamilyId;

            try
            {
                await _dbContext.Families.AddAsync(family, cancellationToken);
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return family;
            }
            catch (DbUpdateException)
            {
                _dbContext.Families.Remove(family);
                _logger.LogError("Failed to create family on attempt {Attempt}. Retrying...", retryAttempts);
                if (retryAttempts > maxAttempts)
                {
                    throw;
                }
            }
        }

        return Error.Failure("Failed to create family.");
    }

    private Family CreateFamilyWithCode(string name, string? description)
    {
        string adv = _wordGenerator.GetWord(PartOfSpeech.adv);
        string adj = _wordGenerator.GetWord(PartOfSpeech.adj);
        string noun = _wordGenerator.GetWord(PartOfSpeech.noun);

        string familyCode = adv.First().ToString().ToUpper() + adv[1..] +
               adj.First().ToString().ToUpper() + adj[1..] +
               noun.First().ToString().ToUpper() + noun[1..];

        return new Family
        {
            Code = familyCode,
            Name = name,
            Description = description
        };
    }
}