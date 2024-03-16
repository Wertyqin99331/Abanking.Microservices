namespace Application.Managers.Post.Dto;

public record GetPostByIdResponse(Guid Id, Guid UserId, string Title, string Text, DateTime DateCreated);