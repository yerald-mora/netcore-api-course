using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Helpers;
using NETCoreMoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomBaseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        protected async Task<List<TDto>> Get<TModel, TDto>() where TModel:class
        {
            var model = await _context.Set<TModel>().AsNoTracking().ToListAsync();
            return _mapper.Map<List<TDto>>(model);
        }

        protected async Task<ActionResult<TDto>> Get<TModel, TDto>(int id) where TModel : class, IIdentifier
        {
            var model = await _context.Set<TModel>()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return (model == null) ? NotFound() : _mapper.Map<TDto>(model);
        }

        protected async Task<List<TDto>> Get<TModel, TDto>(PaginationDto paginationDto) where TModel : class 
        {
            var query = _context.Set<TModel>().AsQueryable();

            await HttpContext.InsertPaginationParameter(query, paginationDto.RecordsPerPage);
            
            var models = await query.Paginate(paginationDto).ToListAsync();
            var dtos = _mapper.Map<List<TDto>>(models);

            return dtos;
        }

        protected async Task<ActionResult> Post<TCreationDto,TModel, TDto>(TCreationDto creationDto,string actionName) where TModel : class,IIdentifier
        {
            var model = _mapper.Map<TModel>(creationDto);

            _context.Add(model);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<TDto>(model);

            return CreatedAtAction(actionName, new { id = model.Id }, dto);
        }

        protected async Task<ActionResult> Put<TCreationDto, TModel>(int id,TCreationDto creationDto) where TModel : class, IIdentifier
        {
            var model = _mapper.Map<TModel>(creationDto);
            model.Id = id;

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Patch<TModel,TDto>(int id, JsonPatchDocument<TDto> patchDocument) where TModel: class, IIdentifier
          where TDto: class
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var model = await _context.Set<TModel>().FindAsync(id);

            if (model == null)
                return NotFound();

            var modelPatchDto = _mapper.Map<TDto>(model);

            patchDocument.ApplyTo(modelPatchDto, ModelState);

            var isValid = TryValidateModel(modelPatchDto);

            if (!isValid)
                return BadRequest(ModelState);

            _mapper.Map(modelPatchDto, model);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Delete<TModel>(int id) where TModel : class, IIdentifier, new()
        {
            if (!TModelExists<TModel>(id))
                return NotFound();

            _context.Remove(new TModel() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TModelExists<TModel>(int id) where TModel : class, IIdentifier
        {
            return _context.Set<TModel>()
                .AsNoTracking()
                .Any(m => m.Id == id);
        }
    }
}
