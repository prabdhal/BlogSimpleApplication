﻿using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Model.ViewModels.PostViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager;

public class PostBusinessManager : IPostBusinessManager
{
    private readonly UserManager<User> _userManager;
    private readonly IPostService _postService;
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _commentReplyService;
    private readonly IAchievementsService _achievementService;
    private readonly IAchievementsBusinessManager _achievementsBusinessManager;
    #region Deleted User Variables
    private readonly string deletedUserCommentText = "<<The comment can no longer be viewed since the user account has been deleted>>";
    private readonly string deletedUserUserNameText = "<<Anonymous>>";
    private readonly Guid deletedUserIdText = new Guid("12345678-1234-1234-1234-123456789012");
    #endregion
    //private readonly int StandardImageWidth = 800;
    //private readonly int StandardImageHeight = 450;

    // Achievement Events
    public delegate void PostCreatedEventHandler(User user);
    public event PostCreatedEventHandler OnPostCreatedEvent;
    public delegate void PostPublishedEventHandler(User user);
    public event PostPublishedEventHandler OnPostPublishedEvent;
    public delegate void PostEditedEventHandler(User user);
    public event PostEditedEventHandler OnPostEditedEvent;
    public delegate void PostFavoritedEventHandler(User user);
    public event PostFavoritedEventHandler OnPostFavoritedEvent;
    public delegate void CommentPublishedEventHandler(User user);
    public event CommentPublishedEventHandler OnCommentPublishedEvent;
    public delegate void OnCommentLikedEventHandler(User user);
    public event OnCommentLikedEventHandler OnCommentLikedEvent;
    public delegate void ReplyPublishedEventHandler(User user);
    public event ReplyPublishedEventHandler OnReplyPublishedEvent;


    public PostBusinessManager(
        UserManager<User> userManager,
        IPostService postService,
        IUserService userService,
        ICommentService commentService,
        ICommentReplyService commentReplyService,
        IAchievementsBusinessManager achievementsBusinessManager,
        IAchievementsService achievementService
        )
    {
        _userManager = userManager;
        _postService = postService;
        _userService = userService;
        _commentService = commentService;
        _commentReplyService = commentReplyService;
        _achievementService = achievementService; 
        _achievementsBusinessManager = achievementsBusinessManager; 
        OnPostCreatedEvent += achievementsBusinessManager.PostCreated; 
        OnPostPublishedEvent += achievementsBusinessManager.PostPublished; 
        OnPostEditedEvent += achievementsBusinessManager.PostEdited;
        OnPostFavoritedEvent += achievementsBusinessManager.PostFavorited;
        OnCommentPublishedEvent += achievementsBusinessManager.CommentPublished;
        OnReplyPublishedEvent += achievementsBusinessManager.ReplyPublished;
        OnCommentLikedEvent += achievementsBusinessManager.CommentLiked; 
    }

    public async Task<DashboardIndexViewModel> GetDashboardIndexViewModel(string searchString, ClaimsPrincipal claimsPrincipal)
    {
        IEnumerable<Post> posts = await _postService.GetAll(searchString ?? string.Empty);

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var loggedInUser = await _userService.Get(user.Id);

        var userPosts = posts.Where(p => p.CreatedById == user.Id);

        return new DashboardIndexViewModel
        {
            UserPosts = userPosts,
            AccountUser = loggedInUser,
        };
    }

    public async Task<FavoritePostsViewModel> GetFavoritePostsViewModel(string searchString, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var usersFavoritePostIds = user.FavoritedPostsId.ToList();

        List<Post> usersFavoritePosts = new List<Post>(); 

        foreach (string postId in usersFavoritePostIds)
        {
            usersFavoritePosts.Add(await _postService.Get(postId));
        }

        return new FavoritePostsViewModel
        {
            UsersFavoritePosts = usersFavoritePosts,
            AccountUser = user
        };
    }

    public async Task<PostDetailsViewModel> FavoritePost(string id, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var post = await _postService.Get(id);
        PostAndCreator postAndCreator = new PostAndCreator
        {
            Post = post,
            Creator = await _userService.Get(post.CreatedById),
        };
        var userAlreadyFavorited = user.FavoritedPostsId.Where(p => p == post.Id).FirstOrDefault();
        if (userAlreadyFavorited is null)
        {
            user.FavoritedPostsId.Add(post.Id);
        }
        else
        {
            var removePost = user.FavoritedPostsId.Where(p => p == post.Id).FirstOrDefault();
            user.FavoritedPostsId.Remove(removePost);
        }

        var comments = await _commentService.GetAllByPost(post.Id);
        List<CommentAndCreator> commentsAndCreator = new List<CommentAndCreator>();
        foreach (Comment comment in comments) 
        {
            CommentAndCreator cAC = new CommentAndCreator
            {
                Comment = comment,
                Creator = await _userService.Get(comment.CreatedById),
            };
            commentsAndCreator.Add(cAC);    
        }

        List<string> postCats = new List<string>();

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            postCats.Add(cat.ToString());
        }

        // Saved post to users favorite list
        var savedUser = await _userService.Update(user.UserName, user);

        //  Achievement Event Trigger
        Achievements achievements = await _achievementService.Get(user.AchievementId);
        if (achievements.FavoritePostFirstTime == false)
        {
            OnPostFavoritedEvent?.Invoke(user);
        }

        return new PostDetailsViewModel
        {
            PostCategories = postCats,
            PostAndCreator = postAndCreator,
            Comment = null,
            Comments = commentsAndCreator,
            AccountUser = user,
            CommentCount = 0,
        };
    }

    public async Task<PostDetailsViewModel> GetPostDetailsViewModel(string postId, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        Post post = await _postService.Get(postId);
        post.HeaderImage = GetImage(Convert.ToBase64String(post.HeaderImage));
        PostAndCreator postAndCreator = new PostAndCreator
        {
            Post = post,
            Creator = await _userService.Get(post.CreatedById),
        };
        List<string> postCats = new List<string>();
        var publishedPosts = await _postService.GetPublishedOnly("");
        List<PostAndCreator> publishedPostsAndCreator = new List<PostAndCreator>();
        foreach (Post p in publishedPosts)
        {
            PostAndCreator pAC = new PostAndCreator
            {
                Post = p,
                Creator = await _userService.Get(p.CreatedById),
            };
            publishedPostsAndCreator.Add(pAC);
        }

        var comments = await _commentService.GetAllByPost(postId);
        List<CommentAndCreator> commentsAndCreator = new List<CommentAndCreator>();
        foreach (Comment comment in comments)
        {
            CommentAndCreator cAC = new CommentAndCreator
            {
                Comment = comment,
                Creator = await _userService.Get(comment.CreatedById),
            };
            commentsAndCreator.Add(cAC);
        }

        var replies = await _commentReplyService.GetAllByPost(postId);
        List<CommentReplyAndCreator> repliesAndCreator = new List<CommentReplyAndCreator>();
        foreach (CommentReply reply in replies)
        {
            CommentReplyAndCreator rAC = new CommentReplyAndCreator
            {
                CommentReply = reply,
                Creator = await _userService.Get(reply.CreatedById),
            };
            repliesAndCreator.Add(rAC);
        }

        int commentCount = comments.Count() + replies.Count();

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            postCats.Add(cat.ToString());
        }

        Achievements achievements;
        List<AchievementsNotificationModel> achievementNotificationsList = new List<AchievementsNotificationModel>();

        if (user != null)
        {
            achievements = await _achievementService.Get(user.AchievementId);
            achievementNotificationsList = GetAchievementsNotificationsList(achievements);
            await _achievementService.Update(achievements.Id, achievements);
        }

        return new PostDetailsViewModel
        {
            AllPosts = publishedPostsAndCreator,
            PostCategories = postCats,
            PostAndCreator = postAndCreator,
            Comments = commentsAndCreator,
            CommentReplies = repliesAndCreator,
            AccountUser = user,
            CommentCount = commentCount,
            AchievementsNotificationList = achievementNotificationsList

        };
    }

    private List<AchievementsNotificationModel> GetAchievementsNotificationsList(Achievements achievements)
    {
        List<AchievementsNotificationModel> results = new List<AchievementsNotificationModel>();
        AchievementsNotificationModel createdPostFirstTimeAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedPostFirstTimeAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedFivePostsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedTenPostsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel editPostFirstTimeAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel favoritedPostFirstTimeAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedCommentFirstTimeAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel likedCommentFirstTimeAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedReplyFirstTimeAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedOver500TotalWordsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedOver1000TotalWordsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedOver5000TotalWordsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedOver10000TotalWordsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedOver50000TotalWordsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedFiveCommentsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel publishedTenCommentsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel published20CommentsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel likedFiveCommentsAchievementNotification = new AchievementsNotificationModel();
        AchievementsNotificationModel likedTenCommentsAchievementNotification = new AchievementsNotificationModel();

        if (achievements.CreatedPostFirstTimeActive && achievements.CreatedPostFirstTime)
        {
            achievements.CreatedPostFirstTimeActive = false;

            createdPostFirstTimeAchievementNotification.Name = achievements.CreatedPostFirstTimeName;
            createdPostFirstTimeAchievementNotification.Description = achievements.CreatedPostFirstTimeDescription;
            results.Add(createdPostFirstTimeAchievementNotification);
        }
        if (achievements.PublishedPostFirstTimeActive && achievements.PublishedPostFirstTime)
        {
            achievements.PublishedPostFirstTimeActive = false;

            publishedPostFirstTimeAchievementNotification.Name = achievements.PublishedPostFirstTimeName;
            publishedPostFirstTimeAchievementNotification.Description = achievements.PublishedPostFirstTimeDescription;
            results.Add(publishedPostFirstTimeAchievementNotification);
        }
        if (achievements.PublishedFivePostsActive && achievements.PublishedFivePosts)
        {
            achievements.PublishedFivePostsActive = false;

            publishedFivePostsAchievementNotification.Name = achievements.PublishedFivePostsName;
            publishedFivePostsAchievementNotification.Description = achievements.PublishedFivePostsDescription;
            results.Add(publishedFivePostsAchievementNotification);
        }
        if (achievements.PublishedTenPostsActive && achievements.PublishedTenPosts)
        {
            achievements.PublishedTenPostsActive = false;

            publishedTenPostsAchievementNotification.Name = achievements.PublishedTenPostsName;
            publishedTenPostsAchievementNotification.Description = achievements.PublishedTenPostsDescription;
            results.Add(publishedTenPostsAchievementNotification);
        }
        if (achievements.EditedPostFirstTimeActive && achievements.EditedPostFirstTime)
        {
            achievements.EditedPostFirstTimeActive = false;

            editPostFirstTimeAchievementNotification.Name = achievements.EditedPostFirstTimeName;
            editPostFirstTimeAchievementNotification.Description = achievements.EditedPostFirstTimeDescription;
            results.Add(editPostFirstTimeAchievementNotification);
        }
        if (achievements.FavoritePostFirstTimeActive && achievements.FavoritePostFirstTime)
        {
            achievements.FavoritePostFirstTimeActive = false;

            favoritedPostFirstTimeAchievementNotification.Name = achievements.FavoritePostFirstTimeName;
            favoritedPostFirstTimeAchievementNotification.Description = achievements.FavoritePostFirstTimeDescription;
            results.Add(favoritedPostFirstTimeAchievementNotification);
        }
        if (achievements.PublishedCommentFirstTimeActive && achievements.PublishedCommentFirstTime)
        {
            achievements.PublishedCommentFirstTimeActive = false;

            publishedCommentFirstTimeAchievementNotification.Name = achievements.PublishedCommentFirstTimeName;
            publishedCommentFirstTimeAchievementNotification.Description = achievements.PublishedCommentFirstTimeDescription;
            results.Add(publishedCommentFirstTimeAchievementNotification);
        }
        if (achievements.LikedCommentFirstTimeActive && achievements.LikedCommentFirstTime)
        {
            achievements.LikedCommentFirstTimeActive = false;

            likedCommentFirstTimeAchievementNotification.Name = achievements.LikedCommentFirstTimeName;
            likedCommentFirstTimeAchievementNotification.Description = achievements.LikedCommentFirstTimeDescription;
            results.Add(likedCommentFirstTimeAchievementNotification);
        }
        if (achievements.PublishedReplyFirstTimeActive && achievements.PublishedReplyFirstTime)
        {
            achievements.PublishedReplyFirstTimeActive = false;

            publishedReplyFirstTimeAchievementNotification.Name = achievements.PublishedReplyFirstTimeName;
            publishedReplyFirstTimeAchievementNotification.Description = achievements.PublishedReplyFirstTimeDescription;
            results.Add(publishedReplyFirstTimeAchievementNotification);
        }
        if (achievements.PublishedOver500TotalWordsActive && achievements.PublishedOver500TotalWords)
        {
            achievements.PublishedOver500TotalWordsActive = false;

            publishedOver500TotalWordsAchievementNotification.Name = achievements.PublishedOver500TotalWordsName;
            publishedOver500TotalWordsAchievementNotification.Description = achievements.PublishedOver500TotalWordsDescription;
            results.Add(publishedOver500TotalWordsAchievementNotification);
        }
        if (achievements.PublishedOver1000TotalWordsActive && achievements.PublishedOver1000TotalWords)
        {
            achievements.PublishedOver1000TotalWordsActive = false;

            publishedOver1000TotalWordsAchievementNotification.Name = achievements.PublishedOver1000TotalWordsName;
            publishedOver1000TotalWordsAchievementNotification.Description = achievements.PublishedOver1000TotalWordsDescription;
            results.Add(publishedOver1000TotalWordsAchievementNotification);
        }
        if (achievements.PublishedOver5000TotalWordsActive && achievements.PublishedOver5000TotalWords)
        {
            achievements.PublishedOver5000TotalWordsActive = false;

            publishedOver5000TotalWordsAchievementNotification.Name = achievements.PublishedOver5000TotalWordsName;
            publishedOver5000TotalWordsAchievementNotification.Description = achievements.PublishedOver5000TotalWordsDescription;
            results.Add(publishedOver5000TotalWordsAchievementNotification);
        }
        if (achievements.PublishedOver10000TotalWordsActive && achievements.PublishedOver10000TotalWords)
        {
            achievements.PublishedOver10000TotalWordsActive = false;

            publishedOver10000TotalWordsAchievementNotification.Name = achievements.PublishedOver10000TotalWordsName;
            publishedOver10000TotalWordsAchievementNotification.Description = achievements.PublishedOver10000TotalWordsDescription;
            results.Add(publishedOver10000TotalWordsAchievementNotification);
        }
        if (achievements.PublishedOver50000TotalWordsActive && achievements.PublishedOver50000TotalWords)
        {
            achievements.PublishedOver50000TotalWordsActive = false;

            publishedOver50000TotalWordsAchievementNotification.Name = achievements.PublishedOver50000TotalWordsName;
            publishedOver50000TotalWordsAchievementNotification.Description = achievements.PublishedOver50000TotalWordsDescription;
            results.Add(publishedOver50000TotalWordsAchievementNotification);
        }
        if (achievements.PublishedTenCommentsActive && achievements.PublishedTenComments)
        {
            achievements.PublishedTenCommentsActive = false;

            publishedTenCommentsAchievementNotification.Name = achievements.PublishedTenCommentsName;
            publishedTenCommentsAchievementNotification.Description = achievements.PublishedTenCommentsDescription;
            results.Add(publishedTenCommentsAchievementNotification);
        }
        if (achievements.Published20CommentsActive && achievements.Published20Comments)
        {
            achievements.Published20CommentsActive = false;

            published20CommentsAchievementNotification.Name = achievements.Published20CommentsName;
            published20CommentsAchievementNotification.Description = achievements.Published20CommentsDescription;
            results.Add(published20CommentsAchievementNotification);
        }
        if (achievements.PublishedFiveCommentsActive && achievements.PublishedFiveComments)
        {
            achievements.PublishedFiveCommentsActive = false;

            publishedFiveCommentsAchievementNotification.Name = achievements.PublishedFiveCommentsName;
            publishedFiveCommentsAchievementNotification.Description = achievements.PublishedFiveCommentsDescription;
            results.Add(publishedFiveCommentsAchievementNotification);
        }
        if (achievements.LikedFiveCommentsActive && achievements.LikedFiveComments)
        {
            achievements.LikedFiveCommentsActive = false;

            likedFiveCommentsAchievementNotification.Name = achievements.LikedFiveCommentsName;
            likedFiveCommentsAchievementNotification.Description = achievements.LikedFiveCommentsDescription;
            results.Add(likedFiveCommentsAchievementNotification);
        }
        if (achievements.LikedTenCommentsActive && achievements.LikedTenComments)
        {
            achievements.LikedTenCommentsActive = false;

            likedTenCommentsAchievementNotification.Name = achievements.LikedTenCommentsName;
            likedTenCommentsAchievementNotification.Description = achievements.LikedTenCommentsDescription;
            results.Add(likedTenCommentsAchievementNotification);
        }

        return results;
    }

    private byte[] GetImage(string sBase64String)
    {
        byte[] bytes = null;
        if (!string.IsNullOrEmpty(sBase64String))
        {
            bytes = Convert.FromBase64String(sBase64String);
        }
        Console.WriteLine(bytes);
        return bytes;
    }

    public async Task<CreatePostViewModel> GetCreatePostViewModel(ClaimsPrincipal claimsPrincipal)
    {
        CreatePostViewModel createViewModel = new CreatePostViewModel();

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        User loggedInUser = await _userService.Get(user.Id);

        createViewModel.AccountUser = loggedInUser;

        return createViewModel;
    }

    public async Task<Post> CreatePost(CreatePostViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
    {
        Post post = createViewModel.Post;

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        if (createViewModel.HeaderImage == null)
        {
        }
        else if (createViewModel.HeaderImage.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                createViewModel.HeaderImage.CopyTo(ms);
                var fileBytes = ms.ToArray();

                byte[] resizedImage = fileBytes;//ResizeImage(fileBytes, StandardImageWidth, StandardImageHeight);
                post.HeaderImage = resizedImage;
            }
        }

        post.CreatedById = user.Id;
        post.CreatedOn = DateTime.Now;
        post.UpdatedOn = DateTime.Now;
        post.WordCount = UtilityMethods.GetWordCount(post.Content);

        // Create Post
        post = await _postService.Create(post);

        // Achievement Event Trigger
        Achievements achievements = await _achievementService.Get(user.AchievementId);
        if (achievements.CreatedPostFirstTime == false)
        {
            OnPostCreatedEvent?.Invoke(user);
        }
        if (post.IsPublished)
        {
            if (achievements.PublishedPostFirstTime == false || 
                achievements.PublishedOver500TotalWords == false || 
                achievements.PublishedOver1000TotalWords == false ||
                achievements.PublishedOver5000TotalWords == false ||
                achievements.PublishedOver10000TotalWords == false ||
                achievements.PublishedOver50000TotalWords == false ||
                achievements.PublishedFivePosts == false ||
                achievements.PublishedTenPosts == false
                )
            {
                OnPostPublishedEvent?.Invoke(user);
            }
        }

        return post;
    }

    private static byte[] ResizeImage(byte[] fileContents, int setWidth, int setHeight,
    SKFilterQuality quality = SKFilterQuality.Medium)
    {
        using MemoryStream ms = new MemoryStream(fileContents);
        using SKBitmap sourceBitmap = SKBitmap.Decode(ms);

        //int height = Math.Min(maxHeight, sourceBitmap.Height);
        //int width = Math.Min(maxWidth, sourceBitmap.Width);

        using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(setWidth, setHeight), quality);
        using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
        using SKData data = scaledImage.Encode();

        return data.ToArray();
    }

    public async Task<Comment> CreateComment(PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        Comment comment = postDetailsViewModel.Comment;

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var post = await _postService.Get(postDetailsViewModel.PostAndCreator.Post.Id);

        comment.CommentedPostId = post.Id;
        comment.CreatedById = user.Id;
        comment.CreatedOn = DateTime.Now;
        comment.UpdatedOn = DateTime.Now;

        // Create Comment
        comment = await _commentService.Create(comment);

        // Achievement Event Triggered
        Achievements achievements = await _achievementService.Get(user.AchievementId);
        if (achievements.PublishedCommentFirstTime == false ||
            achievements.PublishedFiveComments == false || 
            achievements.PublishedTenComments == false || 
            achievements.Published20Comments == false 
            )
        {
            OnCommentPublishedEvent?.Invoke(user);
        }

        return comment;
    }

    public async Task<CommentReply> CreateReply(PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        CommentReply reply = postDetailsViewModel.CommentReply;

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var post = await _postService.Get(postDetailsViewModel.PostAndCreator.Post.Id);
        var comment = await _commentService.Get(postDetailsViewModel.Comment.Id);

        reply.RepliedPostId = post.Id;
        reply.RepliedCommentId = comment.Id;
        reply.CreatedById = user.Id;
        reply.CreatedOn = DateTime.Now;
        reply.UpdatedOn = DateTime.Now;

        // Reply created
        reply = await _commentReplyService.Create(reply);

        // Achievement Event Triggered
        Achievements achievements = await _achievementService.Get(user.AchievementId);
        if (achievements.PublishedReplyFirstTime == false)
        {
            OnReplyPublishedEvent?.Invoke(user);
        }

        await _commentService.Update(comment.Id, comment);

        return reply;
    }

    public async Task<EditPostViewModel> GetEditPostViewModel(string postId, ClaimsPrincipal claimsPrincipal)
    {
        var post = await _postService.Get(postId);

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        User loggedInUser = await _userService.Get(user.Id);

        return new EditPostViewModel
        {
            Post = post,
            AccountUser = loggedInUser
        };
    }

    public async Task<EditPostViewModel> GetEditPostViewModelViaComment(string commentId)
    {
        var comment = await _commentService.Get(commentId);
        var post = await _postService.Get(comment.CommentedPostId);

        return new EditPostViewModel
        {
            Post = post
        };
    }

    public async Task<EditPostViewModel> GetEditPostViewModelViaReply(string replyId)
    {
        var reply = await _commentReplyService.Get(replyId);
        var comment = await _commentService.Get(reply.RepliedCommentId);
        var post = await _postService.Get(comment.CommentedPostId);

        return new EditPostViewModel
        {
            Post = post
        };
    }

    public async Task<ActionResult<EditPostViewModel>> EditPost(EditPostViewModel editPostViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var post = await _postService.Get(editPostViewModel.Post.Id);
        if (post is null)
            return new NotFoundResult();

        post.Title = editPostViewModel.Post.Title;
        post.Category = editPostViewModel.Post.Category;
        post.Description = editPostViewModel.Post.Description;
        post.Content = editPostViewModel.Post.Content;
        post.IsPublished = editPostViewModel.Post.IsPublished;
        post.UpdatedOn = DateTime.Now;
        post.WordCount = UtilityMethods.GetWordCount(post.Content);

        if (editPostViewModel.HeaderImage == null)
        {
        }
        else if (editPostViewModel.HeaderImage.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                editPostViewModel.HeaderImage.CopyTo(ms);
                var fileBytes = ms.ToArray();
                byte[] resizedImage = fileBytes;//ResizeImage(fileBytes, StandardImageWidth, StandardImageHeight);
                post.HeaderImage = resizedImage;
            }
        }

        // Post Saved
        post = await _postService.Update(editPostViewModel.Post.Id, post);

        // Achievement Event Triggered
        Achievements achievements = await _achievementService.Get(user.AchievementId);
        if (achievements.EditedPostFirstTime == false)
        {
            OnPostEditedEvent?.Invoke(user);
        }
        if (post.IsPublished)
        {
            if (achievements.PublishedPostFirstTime == false ||
                achievements.PublishedOver500TotalWords == false ||
                achievements.PublishedOver1000TotalWords == false ||
                achievements.PublishedOver5000TotalWords == false ||
                achievements.PublishedOver10000TotalWords == false ||
                achievements.PublishedOver50000TotalWords == false)
            {
                OnPostPublishedEvent?.Invoke(user);
            }
        }

        return new EditPostViewModel
        {
            Post = post
        };
    }

    public async Task<ActionResult<PostDetailsViewModel>> EditComment(string commentId, PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var post = await _postService.Get(postDetailsViewModel.PostAndCreator.Post.Id);
        if (post is null)
            return new NotFoundResult();
        PostAndCreator postAndCreator = new PostAndCreator
        {
            Post = post,
            Creator = await _userService.Get(post.CreatedById),
        };

        var comment = await _commentService.Get(commentId);
        if (comment is null)
            return new NotFoundResult();

        var replies = await _commentReplyService.GetAllByPost(post.Id);
        comment.Content = postDetailsViewModel.Comment.Content;

        List<string> postCats = new List<string>();
        var comments = await _commentService.GetAllByPost(post.Id);
        List<CommentAndCreator> commentsAndCreator = new List<CommentAndCreator>();
        foreach (Comment c in comments)
        {
            CommentAndCreator cAC = new CommentAndCreator
            {
                Comment = c,
                Creator = await _userService.Get(c.CreatedById),
            };
            commentsAndCreator.Add(cAC);
        }

        int commentCount = comments.Count() + replies.Count();

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            postCats.Add(cat.ToString());
        }

        // Comment Updated
        comment = await _commentService.Update(commentId, comment);

        return new PostDetailsViewModel
        {
            PostCategories = postCats,
            PostAndCreator = postAndCreator,
            Comment = comment,
            Comments = commentsAndCreator,
            AccountUser = user,
            CommentCount = commentCount
        };
    }

    public async Task<ActionResult<PostDetailsViewModel>> EditReply(string replyId, PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var post = await _postService.Get(postDetailsViewModel.PostAndCreator.Post.Id);
        if (post is null)
            return new NotFoundResult();
        PostAndCreator postAndCreator = new PostAndCreator
        {
            Post = post,
            Creator = await _userService.Get(post.CreatedById),
        };

        var comment = await _commentService.Get(postDetailsViewModel.Comment.Id);
        if (comment is null)
            return new NotFoundResult();

        var replies = await _commentReplyService.GetAllByPost(post.Id);

        var reply = await _commentReplyService.Get(replyId);
        if (reply is null)
            return new NotFoundResult();

        reply.Content = postDetailsViewModel.CommentReply.Content;

        List<string> postCats = new List<string>();
        var comments = await _commentService.GetAllByPost(post.Id);
        List<CommentAndCreator> commentsAndCreator = new List<CommentAndCreator>();
        foreach (Comment c in comments)
        {
            CommentAndCreator cAC = new CommentAndCreator
            {
                Comment = c,
                Creator = await _userService.Get(c.CreatedById),
            };
            commentsAndCreator.Add(cAC);
        }

        int commentCount = comments.Count() + replies.Count();

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            postCats.Add(cat.ToString());
        }

        // Reply Updated
        reply = await _commentReplyService.Update(replyId, reply);

        return new PostDetailsViewModel
        {
            PostCategories = postCats,
            PostAndCreator = postAndCreator,
            Comment = comment,
            Comments = commentsAndCreator,
            CommentReply = reply,
            AccountUser = user,
            CommentCount = commentCount
        };
    }

    public async Task<ActionResult<PostDetailsViewModel>> LikeComment(string commentId, PostDetailsViewModel postDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var post = await _postService.Get(postDetailsViewModel.PostAndCreator.Post.Id);
        if (post is null)
            return new NotFoundResult();
        PostAndCreator postAndCreator = new PostAndCreator
        {
            Post = post,
            Creator = await _userService.Get(post.CreatedById),
        };

        var comment = await _commentService.Get(commentId);
        if (comment is null)
            return new NotFoundResult();

        var replies = await _commentReplyService.GetAllByPost(post.Id);

        Guid userAlreadyLiked = comment.CommentLikedByUserNames.Where(u => u == user.Id).FirstOrDefault();

        Console.WriteLine(userAlreadyLiked == Guid.Empty);

        if (userAlreadyLiked == Guid.Empty)
        {
            comment.CommentLikedByUserNames.Add(user.Id);
        }
        else
        {
            comment.CommentLikedByUserNames.Remove(userAlreadyLiked);
        }

        List<string> postCats = new List<string>();
        var comments = await _commentService.GetAllByPost(post.Id);
        List<CommentAndCreator> commentsAndCreator = new List<CommentAndCreator>();
        foreach (Comment c in comments)
        {
            CommentAndCreator cAC = new CommentAndCreator
            {
                Comment = c,
                Creator = await _userService.Get(c.CreatedById),
            };
            commentsAndCreator.Add(cAC);
        }

        int commentCount = comments.Count() + replies.Count();

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            postCats.Add(cat.ToString());
        }

        // Comment Updated
        comment = await _commentService.Update(commentId, comment);

        // Achievement Event Triggered
        Achievements achievements = await _achievementService.Get(user.AchievementId);
        if (achievements.LikedCommentFirstTimeActive == false ||
            achievements.LikedFiveComments == false ||
            achievements.LikedTenComments == false
            )
        {
            OnCommentLikedEvent?.Invoke(user);
        }

        return new PostDetailsViewModel
        {
            PostCategories = postCats,
            PostAndCreator = postAndCreator,
            Comment = comment,
            Comments = commentsAndCreator,
            AccountUser = user,
            CommentCount = commentCount
        };
    }

    public async Task<ActionResult<Post>> DeletePost(string postId, ClaimsPrincipal claimsPrincipal)
    {
        var post = await _postService.Get(postId);
        if (post is null)
            return new NotFoundResult();

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return new NotFoundResult();

        // Validate User
        if (post.CreatedById != user.Id)
            return new NotFoundResult();

        List<User> users = await _userService.GetAll();
        if (users == null)
            return new NotFoundResult();

        DeletePostFromUserFavorites(users, post);

        _commentService.RemoveAllByPost(postId);
        _commentReplyService.RemoveAllByPost(postId);

        _postService.Remove(post);
        return post;
    }

    private async void DeletePostFromUserFavorites(List<User> users, Post post)
    {
        // Remove post from all users favorite posts
        foreach (var u in users)
        {
            u.FavoritedPostsId.RemoveAll(p => p == post.Id);
            await _userService.Update(u.UserName, u);
        }
    }

    public async void DeleteComment(string commentId, ClaimsPrincipal claimsPrincipal)
    {

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            throw new Exception("User could not be found");

        var comment = await _commentService.Get(commentId);
        if (comment == null)
            throw new Exception("comment could not be found");

        // Validate User
        if (comment.CreatedById != user.Id)
            throw new Exception("Cannot delete because comment creator and user creater are not the same");


        // remove all replies for the comment
        _commentReplyService.RemoveAllByComment(commentId);

        // remove comment
        _commentService.Remove(commentId);
    }

    public async void DeleteReply(string replyId, ClaimsPrincipal claimsPrincipal)
    {

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            throw new Exception("User could not be found");

        var reply = await _commentReplyService.Get(replyId);
        if (reply == null)
            throw new Exception("Reply could not be found");

        // Validate User
        if (reply.CreatedById != user.Id)
            throw new Exception("Cannot delete because reply creator and user creater are not the same");

        _commentReplyService.Remove(replyId);
    }

    public async void DeleteUser(ClaimsPrincipal claimsPrincipal)
    {
        var u = await _userManager.GetUserAsync(claimsPrincipal);
        if (u == null)
            throw new Exception("User could not be found");

        var user = await _userService.Get(u.Id);
        if (user == null)
            throw new Exception("User could not be found");

        var users = await _userService.GetAll();

        List<Post> posts = await _postService.GetAllByUser(user);
        List<Comment> comments = await _commentService.GetAll();
        List<CommentReply> replies = await _commentReplyService.GetAll();

        User deletedUser = new User
        {
            Id = deletedUserIdText,
            FirstName = deletedUserUserNameText,
            LastName = deletedUserUserNameText,
            UserName = deletedUserUserNameText,
        };

        // Remove user comments from users posts
        foreach (var comment in comments)
        {
            var post = await _postService.Get(comment.CommentedPostId);
            if (post.CreatedById == user.Id)
            {
                _commentService.Remove(comment);
            }
            else if (comment.CreatedById == user.Id)
            {
                Comment removalCommentTemplate = await _commentService.Get(comment.Id);
                removalCommentTemplate.Content = deletedUserCommentText;
                await _commentService.UpdateForRemoval(comment.Id, removalCommentTemplate);
            }
        }

        // Remove user replies from users posts
        foreach (var reply in replies)
        {
            var post = await _postService.Get(reply.RepliedPostId);
            if (post.CreatedById == user.Id)
            {
                _commentReplyService.Remove(reply);
            }
            else if (reply.CreatedById == user.Id)
            {
                CommentReply removalReplyTemplate = await _commentReplyService.Get(reply.Id);
                removalReplyTemplate.Content = deletedUserCommentText;
                await _commentReplyService.UpdateForRemoval(reply.Id, removalReplyTemplate);
            }
        }

        // delete all posts and posts from other users favorites
        foreach (var post in posts)
        {
            DeletePostFromUserFavorites(users, post);
            _postService.Remove(post.Id);
        }

        //delete user achievements model
        _achievementService.Remove(user.AchievementId);

        // delete user 
        _userService.Remove(user.Id);
    }
}
