using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DateApp.API.Data;
using DateApp.API.Dtos;
using DateApp.API.Helpers;
using DateApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DateApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // loguj 'last active' dla kazdej z metod
    //[Authorize] - po uzyciu AddMvc [...] .RequireAuthenticatedUser() w Startup.cs - juz nie potrzeba tego atrybutu
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams) 
        {   
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId, true);
            userParams.UserId = currentUserId;
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id) 
        {
            var isCurrentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == id;
            var user = await _repo.GetUser(id, isCurrentUser);

            // User user = null;
            // if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //     user = await _repo.GetUser(id);
            // else
            //     user = await _repo.GetUserWithUnapprovedPhotos(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto) 
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await _repo.GetUser(id, true);

            _mapper.Map(userForUpdateDto, userFromRepo);
            if(await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipentId}")]
        public async Task<IActionResult> LikeUser(int id, int recipentId) 
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var like = await _repo.GetLike(id, recipentId);
            if (like != null)
                return BadRequest("You already like this user");
            
            if (await _repo.GetUser(recipentId, false) == null)
                return NotFound();
            
            like = new Like
            {
                LikerId = id,
                LikeeId = recipentId
            };

            _repo.Add<Like>(like);

            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user");
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("Unapproved")]
        public async Task<IActionResult> GetUsersWithUnapprovedPhotos([FromQuery]UserParams userParams) 
        {   
            var users = await _repo.GetUsersWithUnapprovedPhotos(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }
    }
}