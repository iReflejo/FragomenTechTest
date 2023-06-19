using FluentValidation;
using FragomenTechTest.Api.Models;

namespace FragomenTechTest.Api.Validators;

public class HistoricWeatherRequestValidator : AbstractValidator<HistoricWeatherRequest>
{
    public HistoricWeatherRequestValidator()
    {
        RuleFor(x => x.Longitude)
            .NotEmpty();
        RuleFor(x => x.LongitudeDouble)
            .Must(x => x.HasValue && x.Value >= -180 && x.Value <= 180)
            .When(x => x.Longitude != null)
            .WithMessage("Longitude must be between -180 and 180")
            .OverridePropertyName(nameof(CurrentWeatherRequest.Longitude));
        
        RuleFor(x => x.Latitude)
            .NotEmpty();
        RuleFor(x => x.LatitudeDouble)
            .Must(x => x.HasValue && x.Value >= -90 && x.Value <= 90)
            .When(x => x.Latitude != null)
            .WithMessage("Latitude must be between -90 and 90")
            .OverridePropertyName(nameof(CurrentWeatherRequest.Latitude));

        RuleFor(x => x.Units)
            .Must(x => Enum.TryParse(typeof(Units), x, true, out _))
            .When(x => !string.IsNullOrEmpty(x.Units))
            .WithMessage($"Units must be one of the following: {string.Join(", ", Enum.GetValues<Units>())}");

        RuleFor(x => x.StartDate)
            .NotEmpty();
        
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .Custom((property, context) =>
            {
                if (property < context.InstanceToValidate.StartDate)
                {
                    context.AddFailure("End date must be after start date");
                    return;
                }
                
                if(property!.Value.Subtract(context.InstanceToValidate.StartDate!.Value) > TimeSpan.FromDays(31))
                {
                    context.AddFailure("End date must be within 30 days of start date");
                }
            })
            .When(x => x.StartDate != null && x.EndDate != null);
    }
}