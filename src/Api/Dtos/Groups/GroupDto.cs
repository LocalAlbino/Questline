namespace Questline.Api.Dtos.Groups;

// We don't need to track the board id here because the GET route for these
// should only be called when the user is already on that board's page. We can just
// order by rank.
public record GroupDto(long Id, string Title, int Rank);