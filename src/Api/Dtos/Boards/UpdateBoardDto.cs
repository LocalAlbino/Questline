using System.ComponentModel.DataAnnotations;

namespace Questline.Api.Dtos.Boards;

public record UpdateBoardDto([Required] [StringLength(30)] string Title);