using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IBlogBusinessManager
{
    BlogListViewModel GetBlogListViewModel();
    BlogViewModel GetBlogViewModel(string id);
    Task<Blog> CreateBlog(CreateBlogViewModel createViewModel, ClaimsPrincipal claimsPrincipal);
    EditBlogViewModel GetEditBlogViewModel(string id);
    ActionResult<EditBlogViewModel> EditBlog(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal);
    ActionResult<Blog> DeleteBlog(string id);
}
