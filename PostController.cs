using BlogApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostController : ControllerBase
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postService.GetPostsAsync();
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(string title, string content)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == "sub").Value);
        var post = await _postService.CreatePostAsync(userId, title, content);
        return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditPost(int id, string title, string content)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == "sub").Value);
        var post = await _postService.EditPostAsync(userId, id, title, content);
        return Ok(post);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == "sub").Value);
        var success = await _postService.DeletePostAsync(userId, id);
        return success ? NoContent() : NotFound();
    }
}

