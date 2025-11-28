namespace MovieAppApi.Src.Application.Models;

public class CreatePlaylistRequestBodyModel
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public List<int> MovieIds { get; set; } = new();
}

public class PlaylistModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public List<int> MovieIds { get; set; } = new();
}
