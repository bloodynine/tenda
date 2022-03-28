using Tenda.Shared.BaseModels;

namespace Tenda.Reports.GetReportSettings;

public class GetReportSettingsRequest : RequestBase
{
}

public class GetReportSettingsResponse
{
    public List<int> ValidYears { get; set; } = new();
}