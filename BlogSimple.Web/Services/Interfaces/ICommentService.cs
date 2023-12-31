﻿using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ICommentService
{
    Task<List<Comment>> GetAll();
    Task<List<Comment>> GetAll(User user);
    Task<List<Comment>> GetAllByUser(User user);
    Task<List<Comment>> GetAllByPost(string blogId);
    Task<Comment> Get(string commentId);
    Task<Comment> Create(Comment comment);
    Task<Comment> Update(string commentId, Comment comment);
    Task<Comment> UpdateForRemoval(string commentId, Comment comment);
    void Remove(string commentId);
    void Remove(Comment comment);
    void RemoveAllByPost(string blogId);
}
