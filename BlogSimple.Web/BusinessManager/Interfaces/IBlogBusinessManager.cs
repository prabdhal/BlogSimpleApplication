﻿using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IBlogBusinessManager
{
    Task<DashboardIndexViewModel> GetDashboardIndexViewModel(string searchString, ClaimsPrincipal claimsPrincipal);
    BlogDetailsViewModel GetDashboardDetailViewModel(string id);
    Task<Blog> CreateBlog(CreateBlogViewModel createViewModel, ClaimsPrincipal claimsPrincipal);
    Task<Comment> CreateComment(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    EditBlogViewModel GetEditBlogViewModel(string id);
    EditBlogViewModel GetEditBlogViewModelViaComment(string id);
    ActionResult<EditBlogViewModel> EditBlog(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal);
    ActionResult<BlogDetailsViewModel> EditComment(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    ActionResult<Blog> DeleteBlog(string id);
    void DeleteComment(string id);
}
