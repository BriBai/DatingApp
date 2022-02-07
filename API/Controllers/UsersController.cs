using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]

    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;

        private readonly IUserRepository _userReposity;
        public UsersController(IUserRepository userReposity, IMapper mapper)
        {
            this._mapper = mapper;
            this._userReposity = userReposity;
           
        }
       
        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var usersToReturn =  await _userReposity.GetMembersAsync();

            return Ok(usersToReturn);
         
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userReposity.GetMemberAsync(username); 
            

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userReposity.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);
            _userReposity.Update(user);

            if(await _userReposity.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

    }
}