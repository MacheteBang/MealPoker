using CrypticWizard.RandomWordGenerator;
using MealBot.Api.Users.Features.GetUser;
using static CrypticWizard.RandomWordGenerator.WordGenerator; //for brevity, not required

namespace MealBot.Api.Families.Features.CreateFamily;

internal sealed class CreateFamilyCommandHandler(
    IValidator<CreateFamilyCommand> validator,
    MealBotDbContext dbContext,
    ISender sender,
    WordGenerator wordGenerator) : IRequestHandler<CreateFamilyCommand, ErrorOr<Family>>
{
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

        var family = new Family
        {
            Code = GenerateFamilyCode(),
            Name = command.Name,
            Description = command.Description
        };

        user.FamilyId = family.FamilyId;

        await _dbContext.Families.AddAsync(family, cancellationToken);
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return family;
    }

    private string GenerateFamilyCode()
    {
        string adv = _wordGenerator.GetWord(PartOfSpeech.adv);
        string adj = _wordGenerator.GetWord(PartOfSpeech.adj);
        string noun = _wordGenerator.GetWord(PartOfSpeech.noun);

        return adv.First().ToString().ToUpper() + adv[1..] +
               adj.First().ToString().ToUpper() + adj[1..] +
               noun.First().ToString().ToUpper() + noun[1..];
    }
}