﻿using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Model.Services;

public class PostService : IPostService
{
    private readonly IMongoCollection<Post> _posts;

    public PostService(
        IBlogSimpleDatabaseSettings postSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(postSettings.DatabaseName);
        _posts = db.GetCollection<Post>(postSettings.PostsCollectionName);
    }

    public async Task<List<Post>> GetAll()
    {
        return await _posts.Find(_ => true).ToListAsync();
    }

    public async Task<List<Post>> GetAllByUser(User user)
    {
        return await _posts.Find(p => p.CreatedById == user.Id).ToListAsync();
    }

    public async Task<List<Post>> GetAllPublishedByUser(User user)
    {
        // Need to filter by user
        var filterSearch = Builders<Post>.Filter.Where(p => p.CreatedById == user.Id);

        var filterByPublished = Builders<Post>.Filter.Where(b => b.IsPublished);

        return await _posts.Find(filterSearch & filterByPublished).ToListAsync();
    }

    public async Task<List<Post>> GetAll(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Post>.Filter.Where(p => p.Title.ToLower().Contains(search) |
            p.Description.ToLower().Contains(search) |
            p.Category.ToString().ToLower().Contains(search));

        return await _posts.Find(filterSearch).ToListAsync();
    }

    public async Task<List<Post>> GetAll(User user)
    {
        // Need to filter by by user posts
        var filterSearch = Builders<Post>.Filter.Where(p => p.CreatedById == user.Id & p.IsPublished == true);

        return await _posts.Find(filterSearch).ToListAsync();
    }

    public async Task<List<Post>> GetPublishedOnly(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Post>.Filter.Where(p => p.Title.ToLower().Contains(search) |
            p.Description.ToLower().Contains(search) |
            p.Category.ToString().ToLower().Contains(search));

        var filterByPublished = Builders<Post>.Filter.Where(p => p.IsPublished);

        return await _posts.Find(filterSearch & filterByPublished).ToListAsync();
    }

    public async Task<Post> Get(string id)
    {
        return await _posts.Find(post => post.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Post> Create(Post post)
    {
        await _posts.InsertOneAsync(post);
        return post;
    }

    public async Task<Post> Update(string id, Post post)
    {
        await _posts.ReplaceOneAsync(post => post.Id == id, post);
        return post;
    }

    public async void Remove(string id)
    {
        await _posts.DeleteOneAsync(post => post.Id == id);
    }

    public async void Remove(Post post)
    {
        await _posts.DeleteOneAsync(p => p == post);
    }
}
