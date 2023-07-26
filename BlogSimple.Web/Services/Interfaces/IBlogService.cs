﻿using BlogSimple.Model.Models;

namespace BlogSimple.Model.Services.Interfaces;

public interface IBlogService
{
    List<Blog> GetAll();
    List<Blog> GetAll(string searchString);
    Blog Get(string id);
    Blog Create(Blog blog);
    Blog Update(string id, Blog blog);
    void Remove(string id);
    void Remove(Blog blog);
}