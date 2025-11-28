using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MovieAppApi.Src.Application.Validation;

namespace MovieAppApi.Src.Application.Dto;

public class CreatePlaylistRequestBodyDto
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = "";

    public string? Description { get; set; }

    [Required]
    [PositiveIntList]
    [JsonPropertyName("movie_ids")]
    public List<int> MovieIds { get; set; } = new();
}

public class PlaylistDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string? Description { get; set; }

    [JsonPropertyName("movie_ids")]
    public List<int> MovieIds { get; set; } = new();
}
