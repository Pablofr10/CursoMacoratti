using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork uof, IConfiguration config, IMapper mapper)
        {
            _uof = uof;
            _configuration = config;
            _mapper = mapper;
        }

        [HttpGet("autor")]
        public string GetAuthor()
        {
            var autor = _configuration["autor"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];
            return $"Nome: {autor}, Conexão: {conexao}";
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDto>> Get()
        {
            var categorias = _uof.CategoriaRepository.Get().ToList();
            var categoriaDto = _mapper.Map<List<CategoriaDto>>(categorias);
            return categoriaDto;
        }

        [HttpGet("produtos")]
        [HttpGet("/produtos")]
        public ActionResult<IEnumerable<Categoria>> GetcategoriasProdutos()
        {
            return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<CategoriaDto> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);

            if (categoria == null) return NotFound("Categoria não encontrada");

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return categoriaDto;
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices]IMeuServico meuServico,
            string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpPost]
        public ActionResult Post([FromBody]CategoriaDto model)
        {
            var categoria = _mapper.Map<Categoria>(model);
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return Created($"api/categoria/{categoria.CategoriaId}", categoriaDto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]CategoriaDto model)
        {
            if (id != model.CategoriaId)
            {
                return BadRequest();
            }

            var categoria = _mapper.Map<Categoria>(model);

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return Ok(categoriaDto);
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDto> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);
            if (categoria == null) return NotFound("Categoria não encontrada.") ;

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return categoriaDto;
        }


    }
}
