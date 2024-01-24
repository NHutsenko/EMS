namespace EMS.Configurations;

public sealed record UriConfig
{
    public const string SectionName = "UriConfig";
    public string? PaymentsService { get; set; }
    public string? PersonService { get; set; }
    public string? RecruitingService { get; set; }
    public string? StaffService { get; set; }
    public string? StructureService { get; set; }
    public string? VacationsService { get; set; }
};