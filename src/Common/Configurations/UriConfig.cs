namespace EMS.Configurations;

public sealed record UriConfig(
    string? BotService,
    string? EmploymentHistoryService,
    string? PaymentsService,
    string? PersonService,
    string? RecruitingService,
    string? StaffService,
    string? StructureService,
    string? VacationsService)
{
    public const string SectionName = "UriConfig";
};