namespace GameDataService.Services;

public record PointsDto(int Points);
public record TimeDto(int PeriodSeconds);
public record CreateGameDto(DateTime GameDate, int HomeTeamId, int AwayTeamId, int? PeriodSeconds);
