﻿using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager;

public class BlogBusinessManager : IBlogBusinessManager
{
    private readonly UserManager<User> _userManager;
    private readonly IBlogService _blogService;
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _commentReplyService;
    private readonly IWebHostEnvironment webHostEnvironment;

    public BlogBusinessManager(
        UserManager<User> userManager,
        IBlogService blogService,
        IUserService userService,
        ICommentService commentService,
        ICommentReplyService commentReplyService,
            IWebHostEnvironment webHostEnvironment
        )
    {
        _userManager = userManager;
        _blogService = blogService;
        _userService = userService;
        _commentService = commentService;
        _commentReplyService = commentReplyService;
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task<DashboardIndexViewModel> GetDashboardIndexViewModel(string searchString, ClaimsPrincipal claimsPrincipal)
    {
        IEnumerable<Blog> blogs = await _blogService.GetAll(searchString ?? string.Empty);

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var userBlogs = blogs.Where(b => b.CreatedBy.Email == user.Email);

        return new DashboardIndexViewModel
        {
            UserBlogs = userBlogs,
        };
    }

    public async Task<FavoriteBlogsViewModel> GetFavoriteBlogsViewModel(string searchString, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var usersFavoriteBlogs = user.FavoritedBlogs.ToList();

        return new FavoriteBlogsViewModel
        {
            UsersFavoriteBlogs = usersFavoriteBlogs,
        };
    }

    public async Task<BlogDetailsViewModel> FavoriteBlog(string id, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var blog = await _blogService.Get(id);

        var userAlreadyFavorited = user.FavoritedBlogs.Where(b => b.Id == blog.Id).FirstOrDefault();

        if (userAlreadyFavorited is null)
        {
            user.FavoritedBlogs.Add(blog);
        }
        else
        {
            user.FavoritedBlogs.Remove(blog);
        }
        var savedUser = await _userService.Update(user.UserName, user);

        List<string> blogCats = new List<string>();
        var comments = await _commentService.GetAllByBlog(blog.Id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comment = null,
            Comments = comments,
        };
    }

    public async Task<BlogDetailsViewModel> GetBlogDetailsViewModel(string id)
    {
        Blog blog = await _blogService.Get(id);
        List<string> blogCats = new List<string>();
        var blogs = await _blogService.GetPublishedOnly("");
        var comments = await _commentService.GetAllByBlog(id);
        var replies = await _commentReplyService.GetAllByBlog(id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            AllBlogs = blogs,
            BlogCategories = blogCats,
            Blog = blog,
            Comments = comments,
            CommentReplies = replies
        };
    }

    public async Task<Blog> CreateBlog(CreateBlogViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
    {
        Blog blog = createViewModel.Blog;

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        blog.CreatedBy = user;
        blog.CreatedOn = DateTime.Now;
        blog.UpdatedOn = DateTime.Now;

        blog = await _blogService.Create(blog);

        // stores image file name 
        string webRootPath = webHostEnvironment.WebRootPath;
        string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

        EnsureFolder(pathToImage);

        IFormFile headerImg = createViewModel.HeaderImage;

        using (var fileStream = new FileStream(pathToImage, FileMode.Create))
        {
            await headerImg.CopyToAsync(fileStream);
        }

        return blog;
    }

    private IFormFile GetDefaultImage()
    {
        string webRootPath = webHostEnvironment.WebRootPath;
        string pathToImage = $@"{webRootPath}\images\default-image.jpg";
        using (var stream = File.OpenRead(pathToImage))
        {
            return new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
        }
    }

    private void EnsureFolder(string path)
    {
        string directoryName = Path.GetDirectoryName(path);
        if (directoryName.Length > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }

    public async Task<Comment> CreateComment(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        Comment comment = blogDetailsViewModel.Comment;

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        comment.CommentedBlog = blogDetailsViewModel.Blog;
        comment.CreatedBy = user;
        comment.CreatedOn = DateTime.Now;
        comment.UpdatedOn = DateTime.Now;

        comment = await _commentService.Create(comment);

        return comment;
    }

    public async Task<CommentReply> CreateReply(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        CommentReply reply = blogDetailsViewModel.CommentReply;

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var blog = await _blogService.Get(blogDetailsViewModel.Blog.Id);
        var comment = await _commentService.Get(blogDetailsViewModel.Comment.Id);

        reply.RepliedBlog = blog;
        reply.RepliedComment = comment;
        reply.CreatedBy = user;
        reply.CreatedOn = DateTime.Now;
        reply.UpdatedOn = DateTime.Now;

        reply = await _commentReplyService.Create(reply);

        await _commentService.Update(comment.Id, comment);

        return reply;
    }

    public async Task<EditBlogViewModel> GetEditBlogViewModel(string blogId)
    {
        var blog = await _blogService.Get(blogId);

        return new EditBlogViewModel
        {
            Blog = blog
        };
    }

    public async Task<EditBlogViewModel> GetEditBlogViewModelViaComment(string commentId)
    {
        var comment = await _commentService.Get(commentId);
        var blog = await _blogService.Get(comment.CommentedBlog.Id);

        return new EditBlogViewModel
        {
            Blog = blog
        };
    }

    public async Task<EditBlogViewModel> GetEditBlogViewModelViaReply(string replyId)
    {
        var reply = await _commentReplyService.Get(replyId);
        var comment = await _commentService.Get(reply.RepliedComment.Id);
        var blog = await _blogService.Get(comment.CommentedBlog.Id);

        return new EditBlogViewModel
        {
            Blog = blog
        };
    }

    public async Task<ActionResult<EditBlogViewModel>> EditBlog(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = await _blogService.Get(editBlogViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        blog.Title = editBlogViewModel.Blog.Title;
        blog.Category = editBlogViewModel.Blog.Category;
        blog.Description = editBlogViewModel.Blog.Description;
        blog.Content = editBlogViewModel.Blog.Content;
        blog.IsPublished = editBlogViewModel.Blog.IsPublished;
        blog.UpdatedOn = DateTime.Now;

        if (editBlogViewModel.HeaderImage != null)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await editBlogViewModel.HeaderImage.CopyToAsync(fileStream);
            }
        }

        return new EditBlogViewModel
        {
            Blog = await _blogService.Update(editBlogViewModel.Blog.Id, blog)
        };
    }

    public async Task<ActionResult<BlogDetailsViewModel>> EditComment(string commentId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = await _blogService.Get(blogDetailsViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        var comment = await _commentService.Get(commentId);
        if (comment is null)
            return new NotFoundResult();

        comment.Content = blogDetailsViewModel.Comment.Content;

        List<string> blogCats = new List<string>();
        var comments = await _commentService.GetAllByBlog(blog.Id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comment = await _commentService.Update(commentId, comment),
            Comments = comments,
        };
    }

    public async Task<ActionResult<BlogDetailsViewModel>> EditReply(string replyId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = await _blogService.Get(blogDetailsViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        var comment = await _commentService.Get(blogDetailsViewModel.Comment.Id);
        if (comment is null)
            return new NotFoundResult();

        var reply = await _commentReplyService.Get(replyId);
        if (reply is null)
            return new NotFoundResult();

        reply.Content = blogDetailsViewModel.CommentReply.Content;

        List<string> blogCats = new List<string>();
        var comments = await _commentService.GetAllByBlog(blog.Id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comment = comment,
            Comments = comments,
            CommentReply = await _commentReplyService.Update(replyId, reply),
        };
    }

    public async Task<ActionResult<BlogDetailsViewModel>> LikeComment(string commentId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = await _blogService.Get(blogDetailsViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        var comment = await _commentService.Get(commentId);
        if (comment is null)
            return new NotFoundResult();

        var userAlreadyLiked = comment.CommentLikedByUsers.Where(u => u.Id == user.Id).FirstOrDefault();

        if (userAlreadyLiked is null)
        {
            comment.CommentLikedByUsers.Add(user);
        }
        else
        {
            comment.CommentLikedByUsers.Remove(userAlreadyLiked);
        }

        List<string> blogCats = new List<string>();
        var comments = await _commentService.GetAllByBlog(blog.Id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comment = await _commentService.Update(commentId, comment),
            Comments = comments,
        };
    }

    public async Task<ActionResult<Blog>> DeleteBlog(string blogId)
    {
        var blog = await _blogService.Get(blogId);
        if (blog is null)
            return new NotFoundResult();

        _commentService.RemoveAllByBlog(blogId);
        _commentReplyService.RemoveAllByBlog(blogId);

        string webRootPath = webHostEnvironment.WebRootPath;
        string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}";

        string[] files = Directory.GetFiles(pathToImage, "*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        //then delete folder
        Directory.Delete(pathToImage);

        _blogService.Remove(blog);
        return blog;
    }

    public void DeleteComment(string commentId)
    {
        // remove all replies for the comment
        _commentReplyService.RemoveAllByComment(commentId);

        // remove comment
        _commentService.Remove(commentId);
    }

    public void DeleteReply(string replyId)
    {
        _commentReplyService.Remove(replyId);
    }
}
