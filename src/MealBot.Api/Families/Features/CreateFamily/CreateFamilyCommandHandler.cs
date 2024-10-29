using CrypticWizard.RandomWordGenerator;
using MealBot.Api.Families.Services;
using static CrypticWizard.RandomWordGenerator.WordGenerator; //for brevity, not required

namespace MealBot.Api.Families.Features.CreateFamily;

internal sealed class CreateFamilyCommandHandler(
    IValidator<CreateFamilyCommand> validator,
    IFamilyService familyService,
    WordGenerator wordGenerator) : IRequestHandler<CreateFamilyCommand, ErrorOr<Family>>
{
    private readonly IValidator<CreateFamilyCommand> _validator = validator;
    private readonly IFamilyService _familyService = familyService;
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

        var family = new Family
        {
            Code = GenerateFamilyCode(),
            Name = command.Name,
            Description = command.Description
        };

        await _familyService.AddFamilyAsync(family);

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