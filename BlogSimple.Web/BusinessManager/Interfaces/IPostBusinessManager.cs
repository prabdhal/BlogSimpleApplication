﻿using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BlogSimple.Web.BusinessManager.PostBusinessManager;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IPostBusinessManager
{
    // Events 
    event PostCreatedEventHandler OnPostCreatedEvent;
    event PostPublishedEventHandler OnPostPublishedEvent;
    event PostEditedEventHandler OnPostEditedEvent;
    event PostFavoritedEventHandler OnPostFavoritedEvent;
    event CommentPublishedEventHandler OnCommentPublishedEvent;
    event OnCommentLikedEventHandler OnCommentLikedEvent;
    event ReplyPublishedEventHandler OnReplyPublishedEvent;

    //Methods
    Task<DashboardIndexViewModel> GetDashboardIndexViewModel(string searchString, ClaimsPrincipal claimsPrincipal);
    Task<FavoritePostsViewModel> GetFavoritePostsViewModel(string searchString, ClaimsPrincipal claimsPrincipal);
    Task<CreatePostViewModel> GetCreatePostViewModel(ClaimsPrincipal claimsPrincipal);
    Task<PostDetailsViewModel> GetPostDetailsViewModel(string postId, ClaimsPrincipal claimsPrincipal);
    Task<Post> CreatePost(CreatePostViewModel createViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<EditPostViewModel>> EditPost(EditPostViewModel editPostViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<Post>> DeletePost(string postId, ClaimsPrincipal claimsPrincipal);
    Task<PostDetailsViewModel> FavoritePost(string postId, ClaimsPrincipal claimsPrincipal);
    Task<EditPostViewModel> GetEditPostViewModel(string postId, ClaimsPrincipal claimsPrincipal);
    Task<Comment> CreateComment(PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<PostDetailsViewModel>> EditComment(string commentId, PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    void DeleteComment(string commentId, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<PostDetailsViewModel>> LikeComment(string commentId, PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<EditPostViewModel> GetEditPostViewModelViaComment(string commentId);
    Task<CommentReply> CreateReply(PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<PostDetailsViewModel>> EditReply(string commentId, PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    void DeleteReply(string replyId, ClaimsPrincipal claimsPrincipal);
    Task<EditPostViewModel> GetEditPostViewModelViaReply(string replyId);
    void DeleteUser(ClaimsPrincipal claimsPrincipal);
}
