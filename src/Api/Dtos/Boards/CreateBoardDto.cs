using System.ComponentModel.DataAnnotations;

namespace Questline.Api.Dtos.Boards;

public record CreateBoardDto([Required] [StringLength(30)] string Title);