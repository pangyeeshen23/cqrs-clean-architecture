using Application.Tags.Commnads.CreateTag;
using Application.Tags.Commnads.DeleteTag;
using Application.Tags.Commnads.UpdateTag;
using Application.Tags.Queries.GetAllTags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Controllers.Tags
{
    [ApiController]
    [Route("tag")]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List([FromBody] GetAllTagQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new
            {
                sucess = true,
                data = result
            });
        }


        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateTagCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new
            {
                sucess = true,
                message = "Tag created successfully",
                data = result
            });
        }

        [Authorize]
        [HttpPatch("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UpdateTagCommand command)
        {
            await _mediator.Send(command);
            return Ok(new
            {
                sucess = true,
                message = "Tag updated successfully",
            });
        }

        [Authorize]
        [HttpPatch("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] DeleteTagCommand command)
        {
            await _mediator.Send(command);
            return Ok(new
            {
                sucess = true,
                message = "Tag updated successfully",
            });
        }
    }
}
